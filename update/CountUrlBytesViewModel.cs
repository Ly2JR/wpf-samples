using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace update
{
    public sealed class CountUrlBytesViewModel
    {
        public string LoadingMessage { get; private set; }

        public IAsyncCommand Command { get; private set; }

        public string FileName { get; private set; }

        public ICommand RemoveCommand { get; private set; }
        public CountUrlBytesViewModel(MainWindowViewModel parent, string fileName, IAsyncCommand command)
        {
            FileName = fileName;
            LoadingMessage = $"正在下载 ({FileName})...";
            Command = command;
            RemoveCommand = new DelegateComand(() => parent.Operations.Remove(this));
        }
    }
}
