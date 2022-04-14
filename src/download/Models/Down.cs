using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace download.Models
{
    public  class Down
    {
        [SugarColumn(IsIdentity =true,IsPrimaryKey =true)]
        public int Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 文件状态
        /// </summary>
        public FileStatus FileStatus { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        public string ETag { get; set; }
    }
}
