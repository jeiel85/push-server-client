using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.WebSocket.Server;
using System;
using System.IO;
using System.Threading.Tasks;
using ws002.Commands;
using ws002.Services;

namespace ws002
{
    class Program
    {
        private static ILogger<Program> _logger;

        static async Task Main(string[] args)
        {
            // Log 디렉토리 생성
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);

            // Serilog 설정
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    Path.Combine(logDirectory, "server-.log"),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .CreateLogger();

            try
            {
                Log.Information("=== WebSocket Push Server Starting ===");
                Log.Information("Log directory: {LogDirectory}", logDirectory);

                var host = WebSocketHostBuilder.Create(args)
                    .UseSession<UserSession>()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddSingleton<UserService>();
                    })
                    .ConfigureLogging((hostCtx, loggingBuilder) =>
                    {
                        loggingBuilder.AddSerilog(Log.Logger);
                    })
                    .UseCommand<StringPackageInfo, StringPackageConverter>(commandOptions =>
                    {
                        commandOptions.AddCommand<noneProc>();
                        commandOptions.AddCommand<connectList>();
                        commandOptions.AddCommand<CON>();
                        commandOptions.AddCommand<MSG>();
                    })
                    .Build();

                // StartupLogger 주입
                var serviceProvider = host.GetServiceProvider();
                _logger = serviceProvider.GetService<ILogger<Program>>();

                Log.Information("Server configured. Starting host...");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.Information("=== WebSocket Push Server Shutdown ===");
                Log.CloseAndFlush();
            }
        }
    }
}
