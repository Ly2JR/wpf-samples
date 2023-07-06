using DemoCore.Contract;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DemoCore.Plugin
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerClass)]
    public class Server : IServer
    {
        public Window StartCore()
        {
            return new MainView();
        }
    }
}
