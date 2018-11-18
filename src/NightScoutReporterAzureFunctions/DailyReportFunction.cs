using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NightScoutReporterAzureFunctions
{
    public static class DailyReportFunction
    {
        [FunctionName("DailyReportFunction")]
        public static void Run([TimerTrigger("0 0 10 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
