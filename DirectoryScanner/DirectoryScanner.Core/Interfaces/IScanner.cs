using DirectoryScanner.Core.Entities;

namespace DirectoryScanner.Core.Interfaces
{
    public interface IScanner
    {
        public Task<DirectoryTree> Scan(string path, int threadsCount, CancellationToken cancellationToken); 
    }
}
