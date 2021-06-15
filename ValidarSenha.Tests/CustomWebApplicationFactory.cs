using System;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ValidarSenha.API;
using ValidarSenha.Core.Services;


namespace ValidarSenha.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        // Configura o mock do Core para testar apenas a Controller
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection().BuildServiceProvider();
                var coreService = new ValidaSenha();
                services.AddSingleton(coreService);
                services.AddSingleton<IStartupFilter, CustomStartupFilter>();
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                services.AddMvc(opts => opts.EnableEndpointRouting = false);
                services.AddMvc(opts => { opts.Filters.Add(new AllowAnonymousFilter()); });

            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null).UseStartup<Startup>();
        }

    }

    public class CustomStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<FakeRemoteAddressMiddleware>();
                next(app);
            };
        }
    }
}
