using System;

namespace DirectoryScanner.WPFApplication.Commands
{
    internal class CancelDirectoryScanningCommand : Command
    {
        public CancelDirectoryScanningCommand(Func<object, bool> canExecute, Action<object> execute) 
            : base(canExecute, execute)
        {

        }
    }
}
