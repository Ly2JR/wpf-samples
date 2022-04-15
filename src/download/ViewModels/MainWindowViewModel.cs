using download.EventAggregator;
using download.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace download.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "WPF-文件下载";
        /// <summary>
        /// 默认下载地址
        /// </summary>
        private const string BASEURL = "http://localhost:9090";
        private const double ByteSize = 1024.00D;
        private const double KByteSize = 1024D * 1024D;
        private const int DEFAULT_BUFFER_SIZE = 1024 * 10;

        private string _fileLength = string.Empty;
        private string _currentLength = string.Empty;
        private string _fileName = "";
        private int _maxProgress = 100;
        private int _currentProgress = 0;

        private object? _aboutDialog;
        private bool _isAboutDialogOpen;
        public enum FileStatus
        {
            Ready = 0,
            Running,
            Stop,
            End
        }


        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileLength
        {
            get { return _fileLength; }
            set { SetProperty(ref _fileLength, value); }
        }
        /// <summary>
        /// 已下载大小
        /// </summary>
        public string CurrentLength
        {
            get { return _currentLength; }
            set { SetProperty(ref _currentLength, value); }
        }
        /// <summary>
        /// 进度条最大长度
        /// </summary>
        public int MaxProgress
        {
            get { return _maxProgress; }
            set { SetProperty(ref _maxProgress, value); }
        }
        /// <summary>
        /// 进度条当前值
        /// </summary>
        public int CurrentProgress
        {
            get { return _currentProgress; }
            set { SetProperty(ref _currentProgress, value); }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public object? AboutDialog
        {
            get { return _aboutDialog; }
            set { SetProperty(ref _aboutDialog, value); }
        }

        public bool IsAboutDialogOpen
        {
            get { return _isAboutDialogOpen; }
            set { SetProperty(ref _isAboutDialogOpen, value); }
        }

        public DelegateCommand<string> DelegateCommandDownFile { get; private set; }

        public DelegateCommand<string> DelegateCommandWindowClose { get; private set; }

        public DelegateCommand<object> DelegateCommandTaskType { get; private set; }

        public DelegateCommand<string> DelegateComandNavigationRail { get; private set; }

        private readonly IEventAggregator _ea;

        public MainWindowViewModel(IEventAggregator ea)
        {
            _ea = ea;
            DelegateCommandDownFile = new DelegateCommand<string>(ExecuteDownFile);
            DelegateCommandWindowClose = new DelegateCommand<string>(ExecuteWindowsButton);
            DelegateCommandTaskType = new DelegateCommand<object>(ExecuteTaskType);
            DelegateComandNavigationRail = new DelegateCommand<string>(ExecuteNavigationRail);
        }

        private void ExecuteTaskType(object task)
        {
            //switch (task)
            //{
            //    case "play":
            //        break;
            //    case "pause":
            //        break;
            //    case "stop":
            //        break;
            //}
            _ea.GetEvent<TaskTypeEventArgss>().Publish(task);
        }

        private void ExecuteNavigationRail(string navigation)
        {
            switch (navigation)
            {
                case "help":
                    AboutDialog = new About();
                    IsAboutDialogOpen = true;
                    break;
            }
            _ea.GetEvent<NavigationRailEventArgs>().Publish(navigation);
        }

        private void ExecuteWindowsButton(string windowButton)
        {

            switch (windowButton)
            {
                case "min":
                    break;
                case "max":
                    break;
                case "close":
                    Application.Current.Shutdown();
                    break;
            }
        }

        private async void ExecuteDownFile(string fileName)
        {
            var lastIndex = fileName.LastIndexOf('.');
            if (lastIndex == -1) return;

            var suffix = fileName.Substring(lastIndex, fileName.Length - lastIndex);
            var prefix = fileName.Substring(0, lastIndex);
            var searchPattern = $"{prefix}*{suffix}";

            Uri uri = new Uri($"{BASEURL}/{fileName}");

            using (var httpClient = new HttpClient())
            {
                var cancellationSource = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, 5000));
                var cancellationToken = cancellationSource.Token;
                var response = await httpClient.GetAsync(uri, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    if (stream != null)
                    {
                        var allFileLength = stream.Length;
                        if (allFileLength < ByteSize)
                        {
                            FileLength = $"{allFileLength}B";
                            MaxProgress = (int)allFileLength;
                        }
                        else if (allFileLength > KByteSize)
                        {
                            var size = Math.Round(allFileLength / KByteSize, 2);
                            FileLength = $"{size}MB";
                            MaxProgress = (int)allFileLength / 1000;
                        }
                        else
                        {
                            var size = Math.Round(allFileLength / ByteSize, 2);
                            FileLength = $"{size}KB";
                            MaxProgress = (int)allFileLength / 1000;
                        }
                        var savePath = Environment.CurrentDirectory;
                        var files = Directory.GetFiles(savePath, searchPattern, SearchOption.TopDirectoryOnly);
                        var hasFiles = files.Count();
                        if (hasFiles > 0)
                        {
                            FileName = $"{prefix}({hasFiles}){suffix}";
                        }
                        else
                        {
                            FileName = fileName;
                        }
                        savePath += $"/{FileName}";
                        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                var bufferByte = new byte[DEFAULT_BUFFER_SIZE];
                                int startByte = 0;
                                var downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length);
                                while (downByte > 0)
                                {
                                    fileStream.Position = startByte;
                                    await fileStream.WriteAsync(bufferByte, 0, bufferByte.Length);

                                    downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length);

                                    startByte += downByte;

                                    if (startByte < ByteSize)
                                    {
                                        CurrentLength = $"{startByte}B";
                                        CurrentProgress = startByte;
                                    }
                                    else if (startByte > KByteSize)
                                    {
                                        CurrentLength = $"{Math.Round(startByte / KByteSize, 2)}MB";
                                        CurrentProgress = startByte / 1000;
                                    }
                                    else
                                    {
                                        CurrentLength = $"{Math.Round(startByte / ByteSize, 2)}KB";
                                        CurrentProgress = startByte / 1000;
                                    }
                                    fileStream.Flush(true);
                                    await Task.Delay(1);
                                }
                            }
                        }
                        CurrentProgress = MaxProgress;
                    }
                }
            }
        }
    }
}
