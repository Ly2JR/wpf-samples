using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ly2JR.iHome.Wpf.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object?> _execute;
        private Predicate<object?> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action<object?> execute):this(execute,(o)=>true)
        {

        }

        public RelayCommand(Action<object?> execute,Predicate<object?> canExecute) {
            if(execute==null) throw new ArgumentNullException(nameof(execute));
            if(canExecute==null) throw new ArgumentNullException(nameof(canExecute));
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
