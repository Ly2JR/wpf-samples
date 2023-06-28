using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace update
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ScrollViewer? _sv;
        private const string BASEURL = "http://localhost:80";
        private const string UPDATE_XML = "update.xml";
        public ObservableCollection<CountUrlBytesViewModel> Operations { get; private set; }
        private XmlDocument _updateXml;
        private const double ByteSize = 1024.00D;
        private const double KByteSize = 1024D * 1024D;
        private const int DEFAULT_BUFFER_SIZE = 1024 * 30;
        public MainWindowViewModel()
        {
            Operations = new ObservableCollection<CountUrlBytesViewModel>();

            RunCommand = new DelegateComand(() =>
            {
                _updateXml = new XmlDocument();
                var url = $"{BASEURL}/{UPDATE_XML}";
                _updateXml.Load(url);
                var files = _updateXml.SelectNodes("update/file");
                if (files == null) return;

                var logs = _updateXml.SelectNodes("update/logs/log");
                foreach (XmlNode log in logs)
                {
                    Description = log.InnerText;
                }

                foreach (XmlNode file in files)
                {
                    Clear();
                    var name = file.Attributes["name"].Value;
                    var suffix = file.Attributes["suffix"].Value;
                    FileName = $"{name}.{suffix}";
                    Description = $"正在下载:{FileName}";
                    //_sv.LineDown();                  
                    var fileUrl = $"{BASEURL}/{name}.{suffix}";
                    var countBytes = AsyncCommand.Create(token => DownloadAndCountBytesAsync(fileUrl, token));
                    countBytes.Execute(null);
                    Operations.Add(new CountUrlBytesViewModel(this, $"{name}.{suffix}", countBytes));
                }
            });
        }

        public int Count { get { return Operations.Count; } }

        private async Task DownloadAndCountBytesAsync(string url, CancellationToken token = new CancellationToken())
        {
            var tempUrl = url;
            var lastIndex = tempUrl.LastIndexOf('.');
            var lastSplitIndex=tempUrl.LastIndexOf("/");
            if (lastIndex == -1) return;

            var suffix = tempUrl.Substring(lastIndex, tempUrl.Length - lastIndex);
            var name = tempUrl.Substring(lastSplitIndex+1, lastIndex-lastSplitIndex-1);
            var searchPattern = $"{name}*{suffix}";
            Uri uri = new Uri(tempUrl);
            await Task.Delay(TimeSpan.FromSeconds(3), token).ConfigureAwait(false);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri, token).ConfigureAwait(false))
                {
                    var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
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
                        var saveFileName =$"{name}{suffix}";
                        if (hasFiles > 0)
                        {
                            saveFileName = $"{name}({hasFiles}){suffix}";
                        }
                        savePath += $"/{saveFileName}";
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
                                    await Task.Delay(100);
                                    downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length);
                                }
                            }
                        }
                        CurrentProgress = MaxProgress;

                        Description = $"{name}.{suffix}下载完成...";
                    }
                }
            }
        }

        private void Clear()
        {
            FileName = "";
            FileLength = "";
            CurrentLength = "";
            MaxProgress = 0;
            CurrentProgress = 0;
        }

        public ICommand RunCommand { get; private set; }

        public MainWindowViewModel(ScrollViewer sv) : this()
        {
            _sv = sv;
        }

        private StringBuilder _description = new StringBuilder();

        private string _fileLength = "0";
        private string _currentLength = "0";
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileLength
        {
            get { return _fileLength; }
            set { _fileLength = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 已下载大小
        /// </summary>
        public string CurrentLength
        {
            get { return _currentLength; }
            set { _currentLength = value; OnPropertyChanged(); }
        }
        private int _maxProgress = 100;
        /// <summary>
        /// 进度条最大长度
        /// </summary>
        public int MaxProgress
        {
            get { return _maxProgress; }
            set { _maxProgress = value; OnPropertyChanged(); }
        }
        private string _fileName = "";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get { return _description.ToString(); }
            set
            {
                _description.AppendLine(value);
                OnPropertyChanged();
            }
        }

        private int _currentProgress = 0;
        public int CurrentProgress
        {
            get { return _currentProgress; }
            set { _currentProgress = value; OnPropertyChanged(); }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
