using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;


namespace ItHappend.RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.File("reg.log", restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Console(LogEventLevel.Verbose)
                .CreateLogger();
           
            Log.Logger = logger;
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                    { webBuilder.UseStartup<Startup>(); })
                .UseSerilog();
    }
}