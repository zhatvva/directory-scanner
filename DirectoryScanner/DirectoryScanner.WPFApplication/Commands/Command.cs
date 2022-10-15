using System;
using System.Windows.Input;

namespace DirectoryScanner.WPFApplication.Commands
{
    internal class Command : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public Command(Func<object, bool> canExecute, Action<object> execute)
        {
            if (canExecute is null) throw new ArgumentNullException(nameof(canExecute));
            if (execute is null) throw new ArgumentNullException(nameof(execute));

            _canExecute = canExecute;
            _execute = execute;
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
