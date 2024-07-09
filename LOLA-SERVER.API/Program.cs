namespace LOLA_SERVER.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    string port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; 
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls($"http://*:{port}");
                });

    }
}