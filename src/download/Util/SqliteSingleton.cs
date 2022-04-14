using Microsoft.Data.Sqlite;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace download.Util
{
    public class SqliteSingleton
    {
        private static SqliteSingleton _sqlite  = new SqliteSingleton();
        private SqliteSingleton() { }

        public static SqliteSingleton Instance()
        {
            return _sqlite;
        }

        public string GetCurrentProjectPath
        {
            get { return Environment.CurrentDirectory.Replace(@"\bin\Debug", ""); }
        }

        private string GetDbName
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Down.db"); }
        }

        private string GetConnectionString
        {
            get
            {
                return new SqliteConnectionStringBuilder()
                {
                    DataSource = GetDbName,
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    Password = "Ly2JR"
                }.ToString();
            }
        }

        public SqlSugarClient GetDb
        {
            get {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString= GetConnectionString,
                    DbType=DbType.Sqlite,
                    IsAutoCloseConnection=true
                });
            }
        }
    }
}
