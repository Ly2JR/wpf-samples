using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace update
{
    public sealed class CountUrlBytesViewModel : BaseNotifyProperty
    {
        public string LoadingMessage { get; private set; }

        public IAsyncCommand Command { get; private set; }

        public ICommand RemoveCommand { get; private set; }
        private string  _savePath = Environment.CurrentDirectory;
        private string _fileLength = "0";
        private string _currentLength = "0";
        private string _speed = "0B/s";
        private string _consumingTime = "0";
        private DateTime _lastTime;

        /// <summary>
        /// 耗时
        /// </summary>
        public string ConsumingTime
        {
            get { return _consumingTime; }
            set { _consumingTime = value;OnPropertyChanged(); }
        }
        /// <summary>
        /// 下载速度
        /// </summary>
        public string Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged(); }
        }
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

        private MainWindowViewModel _parent;
        public CountUrlBytesViewModel(MainWindowViewModel parent, Dictionary<string,string> fileAttributes)
        {
            _parent = parent;
            var fileSuffix = fileAttributes[Consts.XML_SUFFIX_ATTRIBUTE];
            var name = fileAttributes[Consts.XML_NAME_ATTRIBUTE];
            FileName = $"{name}.{fileSuffix}";
            LoadingMessage = $"正在下载";
            Command = AsyncCommand.Create(token => DownloadAndCountBytesAsync(fileAttributes, token));
            Command.Execute(null);
            RemoveCommand = new DelegateComand(() => parent.Operations.Remove(this));
        }

        private async Task<string> DownloadAndCountBytesAsync(Dictionary<string, string> fileAttributes,CancellationToken token = new CancellationToken())
        {
            if(!fileAttributes.ContainsKey(Consts.XML_NAME_ATTRIBUTE)||!fileAttributes.ContainsKey(Consts.XML_SUFFIX_ATTRIBUTE))
            {
                return "文件配置错误";
            }
            var name = fileAttributes[Consts.XML_NAME_ATTRIBUTE];
            var suffix= fileAttributes[Consts.XML_SUFFIX_ATTRIBUTE];
            var saveFileName = FileName;
            //检查文件是否可以重命名
            if (fileAttributes.ContainsKey(Consts.XML_RENAME_ATTRIBUTE))
            {
                Consts.CAN_RENAME = true;
                saveFileName = $"{fileAttributes[Consts.XML_RENAME_ATTRIBUTE]}.{suffix}";
            }
            //检查是否删除原文件
            if (fileAttributes.ContainsKey(Consts.XML_DELETE_ATTRIBUTE))
            {
                Consts.CAN_DELETE = fileAttributes[Consts.XML_DELETE_ATTRIBUTE].Equals("1");
            }
            //检查下载完成后是否退出
            if (fileAttributes.ContainsKey(Consts.XML_EXIT_ATTRIBUTE))
            {
                Consts.CAN_EXIT = fileAttributes[Consts.XML_EXIT_ATTRIBUTE].Equals("1");
            }
            var searchPattern = $"{name}*{suffix}";
            Uri uri = new Uri($"{Consts.BASE_URL}{name}.{suffix}");
            await Task.Delay(TimeSpan.FromSeconds(3), token).ConfigureAwait(false);
            using (var httpClient = new HttpClient())
            {
               
                httpClient.DefaultRequestHeaders.Accept.Clear();
                var mime = GetMIME(suffix);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mime));
                using (var response = await httpClient.GetAsync(uri, token).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode) return response.StatusCode.ToString();
                    var allFileLength = response.Content.Headers.ContentLength;
                    if (!allFileLength.HasValue) return "获取文件长度失败";

                    if (allFileLength < Consts.K_BYTE_SIZE)
                    {
                        FileLength = $"{allFileLength}B";
                        MaxProgress = (int)allFileLength;
                    }
                    else if (allFileLength > Consts.M_BYTE_SIZE)
                    {
                        FileLength = $"{allFileLength.Value / Consts.M_BYTE_SIZE:F2}MB";
                        MaxProgress = (int)allFileLength / 1000;
                    }
                    else
                    {
                        FileLength = $"{allFileLength.Value / Consts.K_BYTE_SIZE:F2}KB";
                        MaxProgress = (int)allFileLength / 1000;
                    }

                    //删除原文件
                    if (Consts.CAN_DELETE)
                    {
                        File.Delete($"{_savePath}/{FileName}");
                    }

                    if (!Consts.CAN_RENAME)
                    {
                        var files = Directory.GetFiles(_savePath, searchPattern, SearchOption.TopDirectoryOnly);
                        var hasFiles = files.Count();
                        if (hasFiles > 0)
                        {
                            saveFileName = $"{name}({hasFiles}).{suffix}";
                        }
                    }

                    _savePath += $"/{saveFileName}";

                    _lastTime = DateTime.Now;

                    var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
                    if (stream != null)
                    {
                        using (var fileStream = new FileStream(_savePath, FileMode.Create, FileAccess.Write))
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

                                    if (startByte < Consts.K_BYTE_SIZE)
                                    {
                                        CurrentLength = $"{startByte}B";
                                        CurrentProgress = startByte;
                                    }
                                    else if (startByte > Consts.M_BYTE_SIZE)
                                    {
                                        CurrentLength = $"{startByte / Consts.M_BYTE_SIZE:F2}MB";
                                        CurrentProgress = startByte / 1000;
                                    }
                                    else
                                    {
                                        CurrentLength = $"{startByte / Consts.K_BYTE_SIZE:F2}KB";
                                        CurrentProgress = startByte / 1000;
                                    }

                                    downByte = await stream.ReadAsync(bufferByte, 0, bufferByte.Length, token);
                                    //计算下载速度
                                    var second = (DateTime.Now - _lastTime).TotalSeconds;
                                    Speed = GetAutoSizeString(Convert.ToDouble(startByte / second));
                                    await Task.Delay(1);
                                }
                            }
                            await fileStream.FlushAsync(token);
                        }
                        CurrentProgress = MaxProgress;
                        
                    }
                }
            }
            ConsumingTime = $"{(DateTime.Now - _lastTime).Minutes}分钟{(DateTime.Now - _lastTime).Seconds}秒";

            _parent.FinishedList.Add(_savePath);
            return $"{FileName}下载完成";
        }

        public static string GetAutoSizeString(double size)
        {
            if (Consts.K_BYTE_SIZE > size) return $"{size:F2}B/s";
            else if (Consts.M_BYTE_SIZE > size) return $"{size / Consts.K_BYTE_SIZE:F2}KB/s";
            else if (Consts.G_BYTE_SIZE > size) return $"{size / Consts.M_BYTE_SIZE:F2}MB/s";
            else return $"{size / Consts.G_BYTE_SIZE:F2}GB/s";
        }

        /// <summary>
        /// MIME： 
        ///  <see href="https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types">
        ///  或者查看IIS中的MIME
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string GetMIME(string suffix)
        {
            switch (suffix.ToLower())
            {
                case "bmp":
                    return  "image/bmp";
                case "doc":
                    return  "application/msword";
                case "docx":
                    return  "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "json":
                    return   "application/json";
                case "xml":
                    return  "application/xml";
                case "exe":
                    return  "application/octet-stream";
                case "zip":
                    return "application/x-zip-compressed";
                case "dll":
                    return "application/x-msdownload";
                case "dll.config":
                case "exe.config":
                    return "text/xml";
                default:return "application/octet-stream";
            }
        }
    }
}
