using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoWin.Contract
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerInterface)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServer
    {
        /// <summary>
        /// 启动Winform程序
        /// </summary>
        void StartWin();

        /// <summary>
        /// 启动NET Core WPF程序
        /// </summary>
        void StartWpfCore();
    }
}
