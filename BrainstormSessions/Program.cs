using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MSUtil;
using System;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var logPath = config["r"] ?? config["report"] ?? @"C:\Projects\net-basics-mentoring-program-1\Logging\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log";
            TryCreateReport(logPath);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static void TryCreateReport(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), $"Report {DateTime.Now:yyyy-MM-dd HH.mm.ss}.txt");
            var logQuery = new LogQueryClass();
            var inputFormat = new COMFileSystemInputContextClass
            {
                recurse = 0
            };

            var strQuery = $@"SELECT Count(*) AS Count FROM '{path}'";
            using var writer = new StreamWriter(reportPath) { AutoFlush = true };
            ILogRecordset results = logQuery.Execute(strQuery, inputFormat);
            while (!results.atEnd())
            {
                writer.WriteLine(results.getRecord().getValue("Count"));
            }
        }
    }
}
