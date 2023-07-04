using Microsoft.Extensions.Logging;
using SqliteDbs;
using SqlSugar;
using SqlSugar.DistributedSystem.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class DBManager
    {
        private readonly ISqlSugarClient _dbClient;
        private readonly ILogger _logger;

        public DBManager(ISqlSugarClient dbClient, ILogger<DBManager> logger)
        {
            _dbClient = dbClient;
            _logger = logger;
        }

        private List<Login> Logins = new List<Login>()
        {
            new Login(){Code="00001",Password="123"},
            new Login(){Code="00002",Password="345"}
        };

        public async Task InitDataBaseAsync()
        {
            try
            {
                //初始化表
                Type[] types = Assembly
                    .LoadFrom("SqliteDbs.dll")
                    .GetTypes()
                    .ToArray();
                _dbClient.CodeFirst.InitTables(types);

                //表数据是否存在
                var queryLogin = _dbClient.Queryable<Login>().AnyAsync();
                var queryTasks = new List<Task<bool>> { queryLogin};
                while (queryTasks.Count > 0)
                {
                    var query = await Task.WhenAny<bool>(queryTasks);
                    switch (query.Result)
                    {
                        case false:
                            //不存在则默认添加
                            await _dbClient.Insertable<Login>(Logins).ExecuteCommandAsync();
                            break;
                    }
                    var res = queryTasks.Remove(query);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "初始化数据错误");
            }
        }
    }
}
