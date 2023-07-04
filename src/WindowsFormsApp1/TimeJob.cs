using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class TimeJob : IJob, IDisposable
    {
        private ILogger _logger;
        public TimeJob(ILogger<TimeJob> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            Console.WriteLine($"{DateTime.Now}执行完同步作业");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            await Task.CompletedTask;
        }
    }
}
