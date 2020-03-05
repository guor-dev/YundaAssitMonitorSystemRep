using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace YunDa.ISAS.Web.MVC.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                 //.UseKestrel()
                 //.UseContentRoot(Directory.GetCurrentDirectory())
                 //.UseIISIntegration()
                 .UseStartup<Startup>()
                 .Build();
        }
    }
}