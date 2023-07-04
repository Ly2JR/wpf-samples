using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out bool createNew))
            {
                if (createNew)
                {
                    try
                    {
                        IServiceCollection services = new ServiceCollection();
                        var serviceProvider = services.BuildWinform();
                        var frmLogin = serviceProvider.GetRequiredService<FrmLogin>();
                        var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
                        var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
                        scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
                        var db = serviceProvider.GetRequiredService<DBManager>();
                        db.InitDataBaseAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        System.Windows.Forms.Application.Run(frmLogin);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "提示");
                    }
                    finally
                    {
                        Log.CloseAndFlush();
                    }
                }
                else
                {
                    MessageBox.Show("应用程序已经在运行中...");
                    System.Threading.Thread.Sleep(1000);
                    System.Environment.Exit(1);
                }
            }
        }
    }
}
