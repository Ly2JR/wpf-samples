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
    }
}
