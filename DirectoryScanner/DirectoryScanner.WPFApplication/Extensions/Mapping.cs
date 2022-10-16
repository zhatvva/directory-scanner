using DirectoryScanner.Core.Entities;
using DirectoryScanner.Core.Models;
using DirectoryScanner.WPFApplication.ViewModels;

namespace DirectoryScanner.WPFApplication.Extensions
{
    internal static class Mapping
    {
        public static NodeView ToViewModel(this DirectoryTree tree)
        {
            var node = ToViewModel(tree.Root, tree.Root.Size);
            return node;
        }

        private static NodeView ToViewModel(Node node, long parentDirectorySize)
        {
            var sizeInPercents = (float)node.Size / parentDirectorySize * 100;
            var directoryView = new DirectoryNodeView(node.Name, node.Size, sizeInPercents);

            foreach (var child in node.Children)
            {
                if (child.Type == NodeType.Directory)
                {
                    var directoryNode = ToViewModel(child, node.Size);
                    directoryView.Children.Add(directoryNode);
                }
                else
                {
                    sizeInPercents = (float)child.Size / node.Size * 100;
                    var fileNode = new FileNodeView(child.Name, child.Size, sizeInPercents);
                    directoryView.Children.Add(fileNode);
                }
            }
            
            return directoryView;
        }
    }    
}

