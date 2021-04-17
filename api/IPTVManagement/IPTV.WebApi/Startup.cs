using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Abstract;
using Core.Concrete;
using Core.Integrations;
using DataLayer;
using DataLayer.Cache;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IPTV.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpClient();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMemoryCache();
            services.AddMvc();
            services.AddControllers().AddControllersAsServices();
            services.AddHangfire(config =>
config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
.UseSimpleAssemblyNameTypeSerializer()
.UseDefaultTypeSerializer().UseMemoryStorage());
            services.AddHangfireServer();

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient<M3UManager>(x =>
            {
                x.Timeout = new TimeSpan(0, 0, 15);
            });
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<GenericChannelRepository>();
            services.AddScoped<IntegrationFactory>();
            services.AddScoped<Service>();
            services.AddScoped<ChannelController>();
            services.AddScoped<StreamManager>();
            services.AddScoped<ChannelRepository>();
            services.AddScoped<CacheManager>();
            services.AddScoped<M3UManager>();
            services.AddScoped<ElahmadManager>();
            services.AddScoped<HTAManager>();
            services.AddScoped<HalfIntegrateManager>();
            services.AddScoped<IFixedChannelService, FixedChannelManager>();
            services.AddScoped<IGenericChannelService, GenericChannelManager>();

 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard();
            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => serviceProvider.GetService<Service>().ReCache(),
                "0 */18 * ? * *"
                );

        }
    }
}
