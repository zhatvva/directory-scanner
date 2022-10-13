using DirectoryScanner.Core.Models;

namespace DirectoryScanner.Core.Entities
{
    public class Node
    {
        public string Name { get; }
        public NodeType Type { get; }
        public string FullPath { get; }
        public long Size { get; internal set; }
        public Exception LoadException { get; internal set; } = null;
        public IReadOnlyList<Node> Children => _children;

        private readonly List<Node> _children = new();

        public Node(string fullPath, NodeType type)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new ArgumentException("Path cannot be null or empty");
            }

            Name = type == NodeType.File ? Path.GetFileName(fullPath) : Path.GetDirectoryName($"{fullPath}\\");
            Type = type;
            FullPath = fullPath;
        }

        public Node(string fullPath, NodeType type, long size) : this(fullPath, type)
        {
            if (size < 0)
            {
                throw new ArgumentException("File size cannot be less than 0");
            }

            Size = size;
        }

        internal void AddChild(Node node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            _children.Add(node);
        }
    }
}
