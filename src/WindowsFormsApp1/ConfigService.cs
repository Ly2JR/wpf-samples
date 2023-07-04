using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    public static class ConfigService
    {


        public static IServiceProvider BuildWinform(this IServiceCollection services)
        {
            services.AddScoped<FrmLogin>();
            //注册单例
            services.AddSingleton<DBManager>();
            //注册Quartz作业
            services.AddTransient<TimeJob>();
            //配置文件
            IConfigurationBuilder configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configbuilder.Build();
            //配置文件的实体类
            var section = configuration.GetSection(nameof(AppServiceOptions));
            var options = new AppServiceOptions();
            section.Bind(options);
            services.AddSingleton<IConfiguration>(configuration);
            services.AddOptions();


            //注册Logger
            LogProvider.IsDisabled = true;
            var providers = new LoggerProviderCollection();
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.SQLite(options.LogPath)
                    .WriteTo.Providers(providers)
                    .CreateLogger();
            services.AddSingleton(providers);
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            //注册Quartz
            services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core";
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.ScheduleJob<TimeJob>(trigger => trigger
                .WithIdentity("userJobTrigger")
                .StartNow()
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(options.JobTimeInterval, IntervalUnit.Minute))
                );
            });
            services.AddQuartzHostedService(o =>
            {
                o.WaitForJobsToComplete = true;
                o.StartDelay = TimeSpan.FromSeconds(10);
            });
            //注册SugarSql
            SqlSugarScope sqlSugar = new SqlSugarScope(new List<ConnectionConfig>{
        new ConnectionConfig()
    {
        ConfigId=1,
        DbType = SqlSugar.DbType.Sqlite,
        ConnectionString = "DataSource=" + options.DbPath,
        IsAutoCloseConnection = true,
    },
        new ConnectionConfig()
    {
        ConfigId=2,
        DbType = SqlSugar.DbType.Sqlite,
        ConnectionString = "DataSource=" + options.LogPath,
        IsAutoCloseConnection = true,
    }
    },
            db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    var cmd = UtilMethods.GetSqlString(DbType.Sqlite, sql, pars);
                    Debug.WriteLine(cmd);
                };
            });
            services.AddSingleton<ISqlSugarClient>(sqlSugar);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
