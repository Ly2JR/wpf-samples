using Demo.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Plugin
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerClass)]
    public class Server : IServer
    {
        public void Start()
        {
            var main = new Window1();
            main.Show();
        }
    }
}
