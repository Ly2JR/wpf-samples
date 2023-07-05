using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace update
{
    public sealed class MainWindowViewModel : BaseNotifyProperty
    {
        public ObservableCollection<CountUrlBytesViewModel> Operations { get; private set; }
        private XmlDocument _updateXml;

        private string _updateUrl = Consts.UPDATE_XML_URL;
        public string UpdateUrl { get { return _updateUrl; } set { _updateUrl = value; OnPropertyChanged(); } }
        public ObservableCollection<string> FinishedList = new ObservableCollection<string>();

        public MainWindowViewModel()
        {
            Operations = new ObservableCollection<CountUrlBytesViewModel>();
            FinishedList.CollectionChanged += FinishedList_CollectionChanged;
            //手动点击
            RunCommand = new DelegateComand(Run);

            //自动运行
            if (Consts.CAN_AUTO_DOWNLOAD)
            {
                Task.Factory.StartNew(Run);
            }
        }
        private bool autoRun = false;
        private string name = "";
        private string suffix = "";

        private void Run()
        {
            _updateXml = new XmlDocument();
            _updateXml.Load(UpdateUrl);
            var files = _updateXml.SelectNodes("update/file");
            if (files == null) return;

            var logs = _updateXml.SelectNodes("update/logs/log");
            foreach (XmlNode log in logs)
            {
                Logs = log.InnerText;
            }

           
            foreach (XmlNode file in files)
            {
                if (file.Attributes == null) continue;
                var dic=new Dictionary<string, string>();
                foreach(XmlAttribute att in file.Attributes)
                {
                    dic.Add(att.Name, att.Value);
                }
                Operations.Add(new CountUrlBytesViewModel(this, dic));
            }
        }

        private async void FinishedList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (Consts.CAN_EXIT)
                {
                    await Task.Delay(1600);
                    Environment.Exit(0);
                }
                //只有exe文件并且自动运行才有效
                if (autoRun && suffix == "exe")
                {
                    if (FinishedList.Count == Operations.Count)
                    {
                        Thread.Sleep(1500);
                        var info = new ProcessStartInfo
                        {
                            FileName = $"{AppDomain.CurrentDomain.BaseDirectory}/{name}.{suffix}",
                        };
                        Process.Start(info);
                        Environment.Exit(0);
                    }
                }
            }
        }

        public ICommand RunCommand { get; private set; }

        private StringBuilder _logs = new StringBuilder();


        public string Logs
        {
            get { return _logs.ToString(); }
            set
            {
                _logs.AppendLine(value);
                OnPropertyChanged();
            }
        }
    }
}
