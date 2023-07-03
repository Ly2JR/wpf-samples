using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace update
{
    public sealed class CountUrlBytesViewModel: BaseNotifyProperty
    {
        public string LoadingMessage { get; private set; }

        public IAsyncCommand Command { get; private set; }

        public ICommand RemoveCommand { get; private set; }

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
        /// <summary>
        /// 文件名全程
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private int _currentProgress = 0;
        public int CurrentProgress
        {
            get { return _currentProgress; }
            set { _currentProgress = value; OnPropertyChanged(); }
        }

        private string _name;
        private string _fileSuffix;
        private MainWindowViewModel _parent;
        public CountUrlBytesViewModel(MainWindowViewModel parent, string name,string suffix,bool deleteOld=false)
        {
            _parent = parent;
            _fileSuffix = suffix;
            _name = name;
            FileName = $"{_name}.{_fileSuffix}";
            LoadingMessage = $"正在下载";
            Command = AsyncCommand.Create(token => DownloadAndCountBytesAsync(_name, _fileSuffix, token, deleteOld));
            Command.Execute(null);
            RemoveCommand = new DelegateComand(() => parent.Operations.Remove(this));
        }

        private async Task<string> DownloadAndCountBytesAsync(string name,string suffix,CancellationToken token = new CancellationToken(),bool deleteOld=false)
        {
            var searchPattern = $"{name}*{suffix}";
            Uri uri = new Uri($"{Consts.BaseUrl}{name}.{suffix}");
            await Task.Delay(TimeSpan.FromSeconds(3), token).ConfigureAwait(false);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                using (var response = await httpClient.GetAsync(uri, token).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode) return response.StatusCode.ToString();
                    var allFileLength = response.Content.Headers.ContentLength;
                    if (!allFileLength.HasValue) return "获取文件长度失败";

                    if (allFileLength < Consts.ByteSize)
                    {
                        FileLength = $"{allFileLength}B";
                        MaxProgress = (int)allFileLength;
                    }
                    else if (allFileLength > Consts.KByteSize)
                    {
                        FileLength = $"{allFileLength.Value / Consts.KByteSize:F2}MB";
                        MaxProgress = (int)allFileLength / 1000;
                    }
                    else
                    {
                        FileLength = $"{allFileLength.Value / Consts.ByteSize:F2}KB";
                        MaxProgress = (int)allFileLength / 1000;
                    }

                    var savePath = Environment.CurrentDirectory;
                    var saveFileName = FileName;
                    if (!deleteOld)
                    {
                        var files = Directory.GetFiles(savePath, searchPattern, SearchOption.TopDirectoryOnly);
                        var hasFiles = files.Count();
                        if (hasFiles > 0)
                        {
                            saveFileName = $"{name}({hasFiles}).{suffix}";
                        }
                    }
                    else
                    {
                        File.Delete($"{savePath}/{FileName}");
                    }
                   
                    savePath += $"/{saveFileName}";
                    var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
                    if (stream != null)
                    {
                        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                byte[] bufferByte = new byte[Consts.DEFAULT_BUFFER_SIZE];
                                int startByte = 0;
                                var downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length, token);
                                while (downByte > 0)
                                {
                                    fileStream.Position = startByte;

                                    await fileStream.WriteAsync(bufferByte, 0, downByte, token);

                                    startByte += downByte;

                                    if (startByte < Consts.ByteSize)
                                    {
                                        CurrentLength = $"{startByte}B";
                                        CurrentProgress = startByte;
                                    }
                                    else if (startByte > Consts.KByteSize)
                                    {
                                        CurrentLength = $"{startByte / Consts.KByteSize:F2}MB";
                                        CurrentProgress = startByte / 1000;
                                    }
                                    else
                                    {
                                        CurrentLength = $"{startByte / Consts.ByteSize:F2}KB";
                                        CurrentProgress = startByte / 1000;
                                    }
                                    
                                    await Task.Delay(1);
                                    downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length, token);
                                }
                            }
                            await fileStream.FlushAsync(token);
                        }
                        CurrentProgress = MaxProgress;
                        _parent.FinishedList.Add(savePath);
                    }
                }
            }
            return $"{FileName}下载完成";
        }
    }
}
