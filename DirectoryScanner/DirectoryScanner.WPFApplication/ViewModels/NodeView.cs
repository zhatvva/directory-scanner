using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DirectoryScanner.WPFApplication.ViewModels
{
    internal class NodeView 
    {
        public string Name { get; set; }

        public long SizeInBytes { get; set; }

        public float SizeInPercents { get; set; }
        
        protected NodeView(string name, long sizeInBytes, float sizeInPercents)
        {
            Name = name;
            SizeInBytes = sizeInBytes;
            SizeInPercents = sizeInPercents;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
