using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BraintreeSample.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseIISIntegration()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseKestrel()
               .CaptureStartupErrors(true)
               .Build();
  }
}
