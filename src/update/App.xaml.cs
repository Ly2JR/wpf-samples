using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace update
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args != null && e.Args.Length>0)
            {
                var args = e.Args;
                Consts.CAN_AUTO_DOWNLOAD = args.Contains(Consts.EXE_AUTODOWNLOAD_ARG);
            }
            base.OnStartup(e);
        }
    }
}
