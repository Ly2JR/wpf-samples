using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace download.Models
{
    public enum FileStatus
    {
        Ready=0,
        Running,
        Stop,
        Waited,
        End,
    }
}
