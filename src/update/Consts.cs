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
        public const string BASE_URL = "http://localhost:80/";
        /// <summary>
        /// 更新文件
        /// </summary>
        public const string UPDATE_XML_URL = BASE_URL + "update.xml";
        /// <summary>
        /// Byte字节大小
        /// </summary>
        public const int K_BYTE_SIZE = 1024;
        /// <summary>
        /// MByte字节大小
        /// </summary>
        public const int M_BYTE_SIZE = K_BYTE_SIZE * 1024;

        /// <summary>
        /// GByte字节大小
        /// </summary>
        public const int G_BYTE_SIZE = M_BYTE_SIZE * 1024;
        /// <summary>
        /// 默认缓存
        /// </summary>
        public const int DEFAULT_BUFFER_SIZE = 1024 * 10;


        /// <summary>
        /// 来自exe的参数-自动下载
        /// </summary>
        public const string EXE_AUTODOWNLOAD_ARG = "adl";
        /// <summary>
        /// 来自exe的参数-自动运行,启动程序后关闭自身
        /// </summary>
        public const string EXE_AUTORUN_ARG = "ar";
        /// <summary>
        /// 配置文件属性-文件名
        /// </summary>
        public const string XML_NAME_ATTRIBUTE = "name";
        /// <summary>
        /// 配置文件属性-后缀名
        /// </summary>
        public const string XML_SUFFIX_ATTRIBUTE = "suffix";
        /// <summary>
        /// 配置文件属性-1_自动运行并退出更新程序
        /// </summary>
        public const string XML_AUTORUN_ATTRIBUTE = "autorun";
        /// <summary>
        /// 配置文件属性-1_删除原文件,
        /// </summary>
        public const string XML_DELETE_ATTRIBUTE = "delete";
        /// <summary>
        /// 配置文件属性-文件版本号,
        /// </summary>
        public const string XML_VERSION_ATTRIBUTE = "version";
        /// <summary>
        /// 配置文件属性-下载后关闭,
        /// </summary>
        public const string XML_EXIT_ATTRIBUTE = "exit";
        /// <summary>
        /// 配置文件属性-下载后重命名,
        /// </summary>
        public const string XML_RENAME_ATTRIBUTE = "rename";

        public static bool CAN_AUTO_DOWNLOAD = false;
        public static bool CAN_DELETE = false;
        public static bool CAN_EXIT = false;
        public static bool CAN_RENAME = false;
    }
}
