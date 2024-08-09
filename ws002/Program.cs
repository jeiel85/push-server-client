using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.WebSocket.Server;
using System.Threading.Tasks;
using ws002.Commands;
using ws002.Services;

namespace ws002
{
    class Program
    {
        static async Task Main(string[] args)
        {
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


                .ConfigureLogging((hostCtx, loggingBuilder) =>
                {
                    //var logger = new LoggerConfiguration()
                    //    .ReadFrom.Configuration(hostCtx.Configuration)
                    //    .MinimumLevel.Verbose()
                    //    .WriteTo.Console()
                    //    .WriteTo.File("logs_.txt",
                    //        outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    //        rollingInterval: RollingInterval.Day)
                    //    .CreateLogger();

                    //Log.Logger = logger;
                    //loggingBuilder.AddSerilog(logger);

                })
                .Build();

            await host.RunAsync();

        }

    }
}
