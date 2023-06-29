using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace update
{
    public sealed class Consts
    {
        /// <summary>
        /// 资源基地址
        /// </summary>
        public const string BaseUrl = "http://localhost:80/";
        /// <summary>
        /// 更新文件
        /// </summary>
        public const string UPDATE_XML_URL = BaseUrl+"update.xml";
        /// <summary>
        /// Byte字节大小
        /// </summary>
        public const double ByteSize = 1024.00D;
        /// <summary>
        /// KByte字节大小
        /// </summary>
        public const double KByteSize = 1024D * 1024D;
        /// <summary>
        /// 默认缓存
        /// </summary>
        public const int DEFAULT_BUFFER_SIZE = 1024 * 10;
    }
}
