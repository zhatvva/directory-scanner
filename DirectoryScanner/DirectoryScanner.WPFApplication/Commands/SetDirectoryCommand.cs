using System;

namespace DirectoryScanner.WPFApplication.Commands
{
    internal class SetDirectoryCommand : Command
    {
        public SetDirectoryCommand(Func<object, bool> canExecute, Action<object> execute) : base(canExecute, execute)
        {

        }
    }
}
