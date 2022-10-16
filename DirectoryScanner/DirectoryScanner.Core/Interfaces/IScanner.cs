using DirectoryScanner.Core.Entities;
using DirectoryScanner.Core.Models;

namespace DirectoryScanner.Core.Interfaces
{
    public interface IScanner
    {
        public Task<DirectoryTree> Scan(string path, int threadsCount, ChildAddedDelegate tracker);
        public void Cancel();
    }
}
