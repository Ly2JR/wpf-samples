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
        public const int ByteSize = 1024;
        /// <summary>
        /// KByte字节大小
        /// </summary>
        public const int KByteSize = 1024 * 1024;
        /// <summary>
        /// 默认缓存
        /// </summary>
        public const int DEFAULT_BUFFER_SIZE = 1024 * 10;

        /// <summary>
        /// 自动下载
        /// </summary>
        public const string AutoDownloadArg = "adl";
        /// <summary>
        /// 自动运行,启动程序后关闭自身
        /// </summary>
        public const string AutoRunArg = "ar";
        public static bool AutoDownLoad = false;
        public static bool AutoRun = false;
    }
}
