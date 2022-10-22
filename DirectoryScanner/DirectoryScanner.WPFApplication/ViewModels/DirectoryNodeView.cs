using System.ComponentModel;

namespace DirectoryScanner.WPFApplication.ViewModels
{
    internal class DirectoryNodeView : NodeView
    {
        public BindingList<NodeView> Children { get; set; } = new();

        public DirectoryNodeView(string name, long sizeInBytes, float sizeInPercents) : base(name, sizeInBytes, sizeInPercents)
        {
        }
    }
}

