namespace DirectoryScanner.Core.Entities
{
    public class DirectoryTree
    {
        public Node Root { get; }

        public DirectoryTree(Node root)
        {
            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            Root = root;
        }
    }
}
