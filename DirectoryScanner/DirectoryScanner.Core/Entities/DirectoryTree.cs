namespace DirectoryScanner.Core.Entities
{
    public class DirectoryTree
    {
        public Node Root { get; }

        public DirectoryTree(Node root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }
    }
}
