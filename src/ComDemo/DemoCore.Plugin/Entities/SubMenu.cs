using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCore.Plugin.Entities
{
    public class SubMenu
    {
        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        public string SubCode { get; set; }

        public byte Grade { get; set; }

        public string SupMenuCode { get; set; }

        public bool EndGrade { get; set; }

        public int Order { get; set; }

    }
}
