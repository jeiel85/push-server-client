using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket;
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
        static async Task Main(string[] args)
        {
            // Log 디렉토리 생성
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);

            // 로그 파일 작성 헬퍼
            void Log(string level, string message)
            {
                var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                var logFile = Path.Combine(logDirectory, $"server-{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(logFile, logLine + Environment.NewLine);
                Console.WriteLine(logLine);
            }

            try
            {
                Log("INFO", "=== WebSocket Push Server Starting ===");
                Log("INFO", $"Log directory: {logDirectory}");

                var host = WebSocketHostBuilder.Create(args)
                    .UseSession<UserSession>()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddSingleton<UserService>();
                    })
                    .UseCommand<StringPackageInfo, StringPackageConverter>(commandOptions =>
                    {
                        commandOptions.AddCommand<noneProc>();
                        commandOptions.AddCommand<connectList>();
                        commandOptions.AddCommand<CON>();
                        commandOptions.AddCommand<MSG>();
                    })
                    .Build();

                Log("INFO", "Server configured. Starting host...");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log("ERROR", $"Application terminated unexpectedly: {ex.Message}");
                Log("ERROR", ex.StackTrace ?? "");
            }
            finally
            {
                Log("INFO", "=== WebSocket Push Server Shutdown ===");
            }
        }
    }
}
