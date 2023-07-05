using Demo.Entities;
using Demo.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<List<SubSystem>>(Consts.DefaultSubs,nameof(SubSystem));
            containerRegistry.RegisterInstance<List<SubMenu>>(Consts.DefaultMenus,nameof(SubMenu));
            containerRegistry.RegisterInstance<List<MenuLink>>(Consts.DefautlLinks,nameof(MenuLink));
        }

        private void CheckUpdate()
        {
            var links = Container.Resolve<List<MenuLink>>(nameof(MenuLink));
            var assemblys= links.Select(it=>it.Assembly).Distinct().ToList();

            var xml = new XmlDocument();
            xml.Load("http://localhost:80/update.xml");

            var needUpdate = false;
            foreach (var assembly in assemblys)
            {
                var link = links.FirstOrDefault(it => it.Assembly == assembly);
                if (link == null) continue;
                var fileVersion = xml.SelectSingleNode($"update/file[@name='{assembly}']/@version");
                if (fileVersion == null) continue;
                var newVersion = new Version(fileVersion.Value);
                var version = new Version(link.Version);
                if (newVersion.CompareTo(version) > 0)
                {
                    link.Version = fileVersion.Value;
                    needUpdate = true;
                }
            }
            if (!needUpdate) return;
            //检查文件更新
            using (var process = new Process())
            {
                var info = new ProcessStartInfo
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "/update.exe",
                    Arguments = $"adl"
                };
                process.StartInfo = info;
                process.EnableRaisingEvents = true;

                process.Exited += (o, e) =>
                {
                };
                process.Start();
                process.WaitForExit();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            CheckUpdate();
            return Container.Resolve<MainView>();
        }
    }
}
