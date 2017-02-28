// © 2017, kCura LLC 

using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using System;
using System.Linq;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingExtensions
    {
        public static void ConfigureLogging(this ILoggerFactory loggerFactory, IConfigurationSection config)
        {
            var options = config.Get<LoggingOptions>();
            loggerFactory.ConfigureLogging(options);
        }

        public static void ConfigureLogging(this ILoggerFactory loggerFactory, LoggingOptions options)
        {
            options = options ?? new LoggingOptions();

            var serilogConfig = new LoggerConfiguration();

            if (options.Verbose)
            {
                serilogConfig.MinimumLevel.Verbose();
            }
            else
            {
                serilogConfig.MinimumLevel.Information();
            }

            if (options.Filters != null)
            {
                Func<LogEvent, bool> serilogFilter = (e) =>
                {
                    var context = e.Properties["SourceContext"]?.ToString() ?? "";
                    return options.Filters.Any(filter => context.StartsWith("\"" + filter));
                };
                serilogConfig.Filter.ByIncludingOnly(serilogFilter);
            }

            if (!String.IsNullOrEmpty(options.FileName))
            {
                serilogConfig.WriteTo.File(options.FileName);
            }

            if (options.WriteToConsole)
            {
                serilogConfig.WriteTo.Console();
            }

            loggerFactory.AddSerilog(serilogConfig.CreateLogger());
        }
    }
}

