using DemoCore.Client.Activation;
using DemoCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoCore.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new Server();
            var mainView= s.StartCore();
            mainView.ShowDialog();
            Application.Current.MainWindow = mainView;
        }
    }

    // The following classes are typically defined in a PIA, but for this example
    // are being defined here to simplify the scenario.
    namespace Activation
    {
        /// <summary>
        /// Managed definition of CoClass
        /// </summary>
        [ComImport]
        [CoClass(typeof(ServerClass))]
        [Guid(ContractGuids.ServerInterface)] // By TlbImp convention, set this to the GUID of the parent interface
        internal interface Server : IServer
        {
        }

        /// <summary>
        /// Managed activation for CoClass
        /// </summary>
        [ComImport]
        [Guid(ContractGuids.ServerClass)]
        internal class ServerClass
        {
        }
    }
}
