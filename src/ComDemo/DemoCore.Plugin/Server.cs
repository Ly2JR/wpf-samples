using DemoCore.Contract;
using DemoCore.Plugin.Views;
using Prism.Ioc;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DemoCore.Plugin
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerClass)]
    public class Server : IServer
    {
        /// <summary>
        /// Ĭ��
        /// </summary>
        /// <returns></returns>
        public Window StartCore()
        {
           var main=new HelloWorld();
            return main;
        }

        /// <summary>
        /// ʹ��Prism
        /// </summary>
        public void StartWPF()
        {
           var app = new App();
           app.Run();
        }

        public int Sum(int a, int b)
        {
           return a + b;
        }
    }
}
