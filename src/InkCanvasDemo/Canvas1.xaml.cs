using System;
using System.Collections.Generic;
using System.Linq;
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

namespace InkCanvasDemo
{
    /// <summary>
    /// Canvas1.xaml 的交互逻辑
    /// </summary>
    public partial class Canvas1 : UserControl
    {
        public Canvas1()
        {
            InitializeComponent();
        }
        private void RightMouseUpHandler(object sender,
                               System.Windows.Input.MouseButtonEventArgs e)
        {
            Matrix m = new Matrix();
            m.Scale(1.1d, 1.1d);
            ((InkCanvas)sender).Strokes.Transform(m, true);
        }
    }
}
