using DemoWin.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoWin.Plugin
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerClass)]
    public class Server : IServer
    {
        public void StartWin()
        {
            var login=new FrmLogin();
            login.ShowDialog();
        }

        public void StartWpfCore()
        {
            var comType = Type.GetTypeFromCLSID(Guid.Parse(DemoCore.Contract.ContractGuids.ServerClass));
            var active = System.Activator.CreateInstance(comType) as DemoCore.Contract.IServer;
            //var sum=active.Sum(2,5);
            //MessageBox.Show(sum.ToString());
            var mainView = active.StartCore();
            mainView.ShowDialog();
        }
    }
}
