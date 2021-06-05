using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public AsyncCommand(Func<object, Task> execute) : this(execute, obj => true)
        {
        }

        public AsyncCommand(Func<Task> execute) : this(obj => execute(), obj => true)
        {
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute) : this(obj => execute(), obj => canExecute())
        {
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool IsExecuting => _isExecuting;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            try
            {
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }
    }
}
