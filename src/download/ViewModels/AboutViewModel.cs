using download.EventAggregator;
using download.Shared.About;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace download.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        private string _title = "关于";
        private string _author = "2022 Ly2JR";
        private string _license = "开源许可";
        private string _contactUs = "关于我们";
        private string _help = "帮助支持";
        private string _upgrade = "更新日志";
        private string _version = "Version 0.0.1";
        private string _engineVersion = "引擎版本 0.0.1";

        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }

        public string Author { get { return _author; } set { SetProperty(ref _author, value);} }

        public string License { get { return _license; } set { SetProperty(ref _license, value);} }

        public string ContactUs { get { return _contactUs; } set { SetProperty(ref _contactUs, value);} }
        
        public string Help { get { return _help; } set { SetProperty(ref _help, value);} }

        public string Upgrade { get { return _upgrade; } set { SetProperty(ref _upgrade, value);} }

        public string Version { get { return _version; } set { SetProperty(ref _version, value);} }

        public string EngineVersion { get { return _engineVersion; } set { SetProperty(ref _engineVersion, value);} }

        public DelegateCommand DelegateCommandAboutClose { get; private set; }

        private readonly IEventAggregator _ea;

        public AboutViewModel()
        {

        }

        public AboutViewModel(IEventAggregator ea)
        {
            _ea = ea;
            DelegateCommandAboutClose = new DelegateCommand(ExecuteAboutCloseButton);
        }

        private void ExecuteAboutCloseButton()
        {
            _ea.GetEvent<WindowButtonEventArgs>().Publish(AboutButtonConsts.ABOUT_CLOSE_BUTTON);
        }

    }
}
