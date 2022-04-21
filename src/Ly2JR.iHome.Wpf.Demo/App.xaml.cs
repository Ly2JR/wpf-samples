using Ly2JR.iHome.Wpf.Demo.ViewModels;
using Ly2JR.iHome.Wpf.Demo.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Ly2JR.iHome.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var main = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };
            main.Show();
        }
    }
}
