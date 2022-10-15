using System;

namespace DirectoryScanner.WPFApplication.Models
{
    internal class Node
    {
        public string Name { get; }
        public long SizeInBytes { get; }
        public float SizeInPercents { get; }

        public Node(string name, long sizeInBytes, float sizeInPercents)
        {
            Name = name;
            SizeInBytes = sizeInBytes;
            SizeInPercents = sizeInPercents;
        }
    }
}
