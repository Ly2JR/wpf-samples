using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
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
    public sealed class MainWindowViewModel : BaseNotifyProperty
    {
        private ScrollViewer? _sv;
        public ObservableCollection<CountUrlBytesViewModel> Operations { get; private set; }
        private XmlDocument _updateXml;

        private string _updateUrl = Consts.UPDATE_XML_URL;
        public string UpdateUrl { get { return _updateUrl; } set { _updateUrl = value; OnPropertyChanged(); } }
        public MainWindowViewModel()
        {
            Operations = new ObservableCollection<CountUrlBytesViewModel>();

            RunCommand = new DelegateComand(() =>
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
                    var name = file.Attributes["name"].Value;
                    var suffix = file.Attributes["suffix"].Value;
                    //_sv.LineDown();                  
                    Operations.Add(new CountUrlBytesViewModel(this, name, suffix));
                }
            });
        }

        public ICommand RunCommand { get; private set; }

        public MainWindowViewModel(ScrollViewer sv) : this()
        {
            _sv = sv;
        }

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
