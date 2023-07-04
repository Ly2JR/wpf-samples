using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Dto
{
    public class TreeMenu
    {
        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        public int Order { get; set; }

        public bool IsGrouping
        {
            get { return (Children != null && Children.Count > 0); }
        }

        public List<TreeMenu> Children { get; set; }
    }
}
