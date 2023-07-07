using DemoWin.Client.Activation;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var comType = Type.GetTypeFromCLSID(Guid.Parse(DemoCore.Contract.ContractGuids.ServerClass));
            var active = System.Activator.CreateInstance(comType) as DemoCore.Contract.IServer;

            //var sum = active.Sum(2, 5);
            //MessageBox.Show(sum.ToString());

            //var mainView = active.StartCore();
            //mainView.ShowDialog();

            this.Hide();
            active.StartWPF();
            Close();

            ////失败
            //DemoCore.Contract.IServer b = new ServerProxy();
            //MessageBox.Show(b.Sum(2, 5).ToString());
        }
    }

    namespace Activation
    {
        /// <summary>
        /// Managed definition of CoClass
        /// </summary>
        [ComImport]
        [CoClass(typeof(ServerClass))]
        [Guid(DemoCore.Contract.ContractGuids.ServerInterface)] // By TlbImp convention, set this to the GUID of the parent interface
        internal interface ServerProxy : DemoCore.Contract.IServer
        {
        }

        /// <summary>
        /// Managed activation for CoClass
        /// </summary>
        [ComImport]
        [Guid(DemoCore.Contract.ContractGuids.ServerClass)]
        internal class ServerClass
        {
        }
    }
}
