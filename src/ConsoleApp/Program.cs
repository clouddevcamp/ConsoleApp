using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

namespace ConsoleApp
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public IConfigurationRoot Configuration { get; set; }

        public Program(IApplicationEnvironment env)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<ISampleApi, SampleApi>();
        }

        public void Main(string[] args)
        {
            var apiInstance = _serviceProvider.GetService<ISampleApi>();
            Console.WriteLine(apiInstance.GetMessage());
            Console.ReadLine();
        }
    }

    public class AppSettings
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public interface ISampleApi
    {
        string GetMessage();
    }

    public class SampleApi : ISampleApi
    {
        private readonly AppSettings _settings;
        public SampleApi(IConfigureOptions<AppSettings> configuration)
        {
            _settings = new AppSettings();
            configuration.Configure(_settings);
        }

        public string GetMessage()
        {
            return $"Cloud Dev Camp: {_settings.Name}";
        }
    }
}
