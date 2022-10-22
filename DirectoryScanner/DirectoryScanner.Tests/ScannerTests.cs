using DirectoryScanner.Core.Services;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using DirectoryScanner.Core.Models;
using System.Linq;
using System.IO;
using System;

namespace DirectoryScanner.Tests
{
    public class ScannerTests
    {
        [Fact]
        public async Task Scan_ReturnsCorrectScanResult_WhenThreadsCountIsCorrectAndDirectoryExists()
        {
            var scanner = new Scanner();
            var scanResult = await scanner.Scan("test_directory", 5);

            scanResult.Root.Children.Should().HaveCount(2);
            scanResult.Root.Children.Should().Contain(c => c.Type == NodeType.File && c.Name == "a.txt" 
                && c.Size == 10);
            scanResult.Root.Children.Should().Contain(c => c.Type == NodeType.Directory && c.Name == "dirA" && c.Size == 3
             && c.Children.Any(c => c.Type == NodeType.File && c.Name == "b.txt" && c.Size == 3));
        }

        [Theory]
        [InlineData("wrong", 5)]
        [InlineData("test_directory", 0)]
        public async Task Scan_ThrowsInvalidArgumentException_WhenNonExistingFolder(string path, int threadsCount)
        {
            var scanner = new Scanner();
            var scan = () => scanner.Scan(path, threadsCount);

            if (threadsCount > 1)
            {
                await scan.Should().ThrowAsync<DirectoryNotFoundException>();
            }
            else
            {
                await scan.Should().ThrowAsync<ArgumentException>();
            }
        }

        [Fact]
        public async Task Scan_ReturnsCorrectScanResult_WhenFileScanned()
        {
            var scanner = new Scanner();
            var result = await scanner.Scan("test_directory/a.txt", 5);

            result.Root.Name.Should().Be("a.txt");
            result.Root.Type.Should().Be(NodeType.File);
            result.Root.Size.Should().Be(10);
        }

        [Fact]
        public async Task Scan_RootSizeIsLess_WhenScanWasStopped()
        {
            var scanner = new Scanner();
            
            var firstResult = await scanner.Scan("D:/", 100);
            var secondResultTask = scanner.Scan("D:/", 100);
            scanner.Cancel();
            var secondResult = await secondResultTask;

            secondResult.Root.Size.Should().BeLessThan(firstResult.Root.Size);
        }
    }
}