using DemoWin.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoWin.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var comType = Type.GetTypeFromCLSID(Guid.Parse(ContractGuids.ServerClass));
            var active = System.Activator.CreateInstance(comType) as IServer;
            active.StartWin();
        }
    }

    //namespace Activation
    //{
    //    /// <summary>
    //    /// Managed definition of CoClass
    //    /// </summary>
    //    [ComImport]
    //    [CoClass(typeof(ServerClass))]
    //    [Guid(ContractGuids.ServerInferface)] // By TlbImp convention, set this to the GUID of the parent interface
    //    internal interface Server : IServer
    //    {
    //    }

    //    /// <summary>
    //    /// Managed activation for CoClass
    //    /// </summary>
    //    [ComImport]
    //    [Guid(ContractGuids.ServerClass)]
    //    internal class ServerClass
    //    {
    //    }
    //}
}
