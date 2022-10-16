using DirectoryScanner.Core.Entities;

namespace DirectoryScanner.Core.Models;

public class ChildAddedEventArgs : EventArgs
{
    public Node Node { get; }

    public ChildAddedEventArgs(Node node)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
    }
}