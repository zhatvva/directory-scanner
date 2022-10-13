﻿using DirectoryScanner.Core.Entities;
using DirectoryScanner.Core.Interfaces;
using DirectoryScanner.Core.Models;
using System.Collections.Concurrent;
using DirectoryScanner.Core.Extensions;

namespace DirectoryScanner.Core.Services
{
    public class Scanner : IScanner
    {
        private readonly ConcurrentQueue<Node> _nodesToScan = new();
        
        public async Task<DirectoryTree> Scan(string path, int threadsCount, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Path: {path}");
            }

            var tree = new DirectoryTree(new Node(path, NodeType.Directory));
            ScanNode(tree.Root);
            var tasks = new List<Task>(threadsCount);
            var semaphore = new SemaphoreSlim(1, threadsCount);
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
            while (!_nodesToScan.IsEmpty && !cancellationToken.IsCancellationRequested || activeThreads > 1);

            await Task.WhenAll(tasks);
            SetNodeSize(tree.Root);
            return tree;
        }

        private void ScanNode(Node node)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(node.FullPath);

                var directories = directoryInfo.GetDirectories();
                foreach (var directory in directories)
                {
                    var directoryNode = new Node(directory.FullName, NodeType.Directory);
                    node.AddChild(directoryNode);
                    _nodesToScan.Enqueue(directoryNode);
                }

                var files = directoryInfo.GetFiles();
                foreach (var file in files)
                {
                    var fileNode = new Node(file.FullName, NodeType.File, file.Length);
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