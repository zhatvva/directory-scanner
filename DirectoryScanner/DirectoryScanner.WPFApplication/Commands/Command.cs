using System;
using System.Windows.Input;

namespace DirectoryScanner.WPFApplication.Commands
{
    internal class Command : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        protected Command(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
            
        public virtual event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public virtual bool CanExecute(object parameter) => _canExecute(parameter);
        public virtual void Execute(object parameter) => _execute(parameter);
    }
}
