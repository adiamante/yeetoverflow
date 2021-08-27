using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;

namespace YeetOverFlow.Logging
{
    public static class YeetLoggerServiceCollectionExtensions
    {
        public static IServiceCollection AddYeetLogger(this IServiceCollection services, 
            Action<LoggerConfiguration> setupAction = null)
        {
            services.AddSingleton<YeetSinkActionProvider>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddYeetLogger(setupAction));

            return services;
        }

        //https://github.com/serilog/serilog-extensions-logging/blob/dev/src/Serilog.Extensions.Logging/SerilogLoggingBuilderExtensions.cs
        public static ILoggingBuilder AddYeetLogger(this ILoggingBuilder builder, Action<LoggerConfiguration> setupAction = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>(sp => 
            {
                var actionProvider = sp.GetRequiredService<YeetSinkActionProvider>();

                //https://improveandrepeat.com/2014/08/structured-logging-with-serilog/
                //https://github.com/serilog/serilog/wiki/Formatting-Output
                //https://github.com/serilog/serilog-formatting-compact
                var configuration = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    //.WriteTo.Debug(outputTemplate: "[{Message:lj}]{NewLine}{Properties:j}{NewLine}")
                    //.WriteTo.Debug(formatter : new RenderedCompactJsonFormatter())
                    .WriteTo.Debug(formatter: new YeetSimpleCompactFormatter())
                    .WriteTo.File(path: "logs/log_.txt", formatter: new JsonFormatter(), rollingInterval: RollingInterval.Day)
                    .WriteTo.YeetSink(actionProvider);

                setupAction?.Invoke(configuration);
                var logger = configuration.CreateLogger();

                return new SerilogLoggerProvider(logger, true);
            });


            //builder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
            builder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Information);

            return builder;
        }
    }
}
