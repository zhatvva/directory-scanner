using DirectoryScanner.Core.Entities;
using DirectoryScanner.Core.Interfaces;
using DirectoryScanner.Core.Models;
using System.Collections.Concurrent;
using DirectoryScanner.Core.Extensions;

namespace DirectoryScanner.Core.Services
{
    public class Scanner : IScanner
    {
        private ConcurrentQueue<Node> _nodesToScan;
        private CancellationTokenSource _cancellationTokenSource;

        public async Task<DirectoryTree> Scan(string path, int threadsCount)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Path: {path}");
            }

            if (threadsCount <= 1)
            {
                throw new ArgumentException("Threads count should be at least 2");
            }
            
            _nodesToScan = new ConcurrentQueue<Node>();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            var tree = new DirectoryTree(new Node(path, path, NodeType.Directory));
            ScanNode(tree.Root);
            var tasks = new List<Task>(threadsCount);
            var activeThreads = 1;

            do
            {
                if (activeThreads >= threadsCount)
                {
                    await Task.WhenAny(tasks);
                }

                if (_nodesToScan.TryDequeue(out var node))
                {
                    var task = Task.Run(() =>
                    {
                        ScanNode(node);
                        Interlocked.Decrement(ref activeThreads);
                    });
                    tasks.ReplaceCompletedWithNewOrAdd(task);
                    Interlocked.Increment(ref activeThreads);
                }
            }
            while (!cancellationToken.IsCancellationRequested && (activeThreads > 1 || !_nodesToScan.IsEmpty));

            await Task.WhenAll(tasks);
            SetNodeSize(tree.Root);
            return tree;
        }

        public void Cancel() => _cancellationTokenSource?.Cancel();

        private void ScanNode(Node node)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(node.FullPath);

                var directories = directoryInfo.GetDirectories();
                foreach (var directory in directories)
                {
                    var directoryNode = new Node(directory.Name, directory.FullName, NodeType.Directory);
                    node.AddChild(directoryNode);
                    _nodesToScan.Enqueue(directoryNode);
                }

                var files = directoryInfo.GetFiles();
                foreach (var file in files)
                {
                    var fileNode = new Node(file.Name, file.FullName, NodeType.File, file.Length);
                    node.AddChild(fileNode);
                }
            }
            catch (Exception ex)
            {
                node.LoadException = ex;
            }
        }

        private void SetNodeSize(Node node)
        {
            foreach (var child in node.Children)
            {
                if (child.Type == NodeType.Directory)
                {
                    SetNodeSize(child);
                }
                node.Size += child.Size;
            }
        }
    }
}
