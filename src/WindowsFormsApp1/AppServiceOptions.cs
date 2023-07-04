using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public  class AppServiceOptions
    {
        public int JobTimeInterval { get; set; }

        /// <summary>
        /// 获取当前程序目录
        /// </summary>
        public string GetCurrentProjectPath
        {
            get
            {
                return Environment.CurrentDirectory.Replace(@"\bin\Debug", "");
            }
        }

        public string LogPath
        {
            get { return GetCurrentProjectPath + "\\log.db3" ; }
        }

        public string DbPath
        {
            get { return GetCurrentProjectPath + "\\demo.db3"; }
        }

    }
}
