using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ly2JR.iHome.Wpf
{
    public  class HomeIcon:Control
    {
        private static readonly Lazy<IDictionary<HomeIconKind, string>> _dataIndex =
            new Lazy<IDictionary<HomeIconKind, string>>();
    }
}
