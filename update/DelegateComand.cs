using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace update
{
    public sealed class DelegateComand : ICommand
    {
        private readonly Action _command;

        public DelegateComand(Action command)
        {
            _command = command;
        }

        event EventHandler? ICommand.CanExecuteChanged
        {
            add
            {
               
            }

            remove
            {
               
            }
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return true;
        }

        void ICommand.Execute(object? parameter)
        {
            _command();
        }
    }
}
