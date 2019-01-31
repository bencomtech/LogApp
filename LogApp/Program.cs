using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using System;
using System.Net;

namespace LogApp
{
    class Program
    {
        private const string DebugLogFile = @"Logs\Debug\debug.txt";
        private const string InfoLogFile = @"Logs\Info\info.txt";
        private const string ErrorLogFile = @"Logs\Error\error.txt";

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                    .WriteTo.File(DebugLogFile, rollingInterval: RollingInterval.Hour, rollOnFileSizeLimit: true))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(InfoLogFile, rollingInterval: RollingInterval.Hour, rollOnFileSizeLimit: true))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(ErrorLogFile, rollingInterval: RollingInterval.Hour, rollOnFileSizeLimit: true))
                .WriteTo.Email(new EmailConnectionInfo()
                {
                    EmailSubject = "LogApp Errors",
                    EnableSsl = false,
                    FromEmail = "bencomtech@example.com",
                    ToEmail = "bencomtech@example.com",
                    MailServer = "smtp.mailtrap.io",
                    NetworkCredentials = new NetworkCredential("f7cd207ba6806f", "208816bde48b18")
                }, restrictedToMinimumLevel: LogEventLevel.Error)
                .CreateLogger();

            try
            {
                Log.Information("Application Start");

                double result = 0;

                for (int index = 10; index >= 0; index--)
                {
                    Log.Debug("Index: {0}", index);

                    result = 10 / index;

                    Log.Information("Result: {0}", result);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
