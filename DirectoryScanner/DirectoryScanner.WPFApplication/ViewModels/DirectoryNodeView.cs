using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DirectoryScanner.Core.Models;

namespace DirectoryScanner.WPFApplication.ViewModels
{
    internal class DirectoryNodeView : NodeView
    {
        public ObservableCollection<NodeView> Children { get; } = new();

        public DirectoryNodeView(string name, long sizeInBytes, float sizeInPercents) : base(name, sizeInBytes, sizeInPercents)
        {
            
        }

        public void ChildAddedTracker(object source, EventArgs args)
        {
            var eventArgs = args as ChildAddedEventArgs
                            ?? throw new ArgumentException($"Cannot cast to {nameof(ChildAddedEventArgs)}", nameof(args));
            NodeView node;
            var sizeInPercent = (float)eventArgs.Node.Size / SizeInBytes * 100;
            if (eventArgs.Node.Type == NodeType.Directory)
            {
                var directoryNode = new DirectoryNodeView(eventArgs.Node.Name, eventArgs.Node.Size, sizeInPercent);
                eventArgs.Node.ChildAddedEvent += directoryNode.ChildAddedTracker;
                Children.Add(directoryNode);
            }
            else
            {
                var fileNode = new FileNodeView(eventArgs.Node.Name, eventArgs.Node.Size, sizeInPercent);
                Children.Add(fileNode);
            }
        }
    }
}

