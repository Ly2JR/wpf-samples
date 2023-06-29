using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace update
{
    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken,Task<TResult>> _command;
        private readonly CancelAsyncCommand _canncelCommand;
        private NotifyTaskCompletion<TResult> _execution;

        public AsyncCommand(Func<CancellationToken,Task<TResult>> command)
        {
            _command = command;
            _canncelCommand=new CancelAsyncCommand();
        }

        public override bool CanExecute(object? parameter)
        {
            return Execution == null|| Execution.IsCompleted;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _canncelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(_command(_canncelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;

            _canncelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public ICommand CancelCommand { get { return _canncelCommand; } }

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName =null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;

            public CancellationToken Token { get { return _cts.Token; } }

            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested) return;
                _cts=new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            public event EventHandler? CanExecuteChanged {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }

            bool ICommand.CanExecute(object? parameter)
            {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }

            void ICommand.Execute(object? parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }
        }
    }

    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(async _ => { await command(); return null; });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(_ => command());
        }

        public static AsyncCommand<object> Create(Func<CancellationToken,Task> command)
        {
            return new AsyncCommand<object>(async token => { await command(token); return null; });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken,Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(command);
        }
    }
}

