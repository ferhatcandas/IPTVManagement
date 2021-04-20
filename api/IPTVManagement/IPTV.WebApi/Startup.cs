using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Abstract;
using Core.Concrete;
using Core.Integrations;
using Core.Integrations.Abstract;
using Core.Integrations.Concrete;
using DataLayer;
using DataLayer.Cache;
using DataLayer.Repository.Mongo;
using DataLayer.Repository.Mongo.Abstract;
using DataLayer.Repository.Mongo.Concrete;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using Model.Integration;
using MongoDB.Driver;

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
            var settings = Configuration.Get<Settings>();
            //services.AddHttpClient<M3UManager>(x =>
            //{
            //    x.Timeout = new TimeSpan(0, 0, 15);
            //});
            services.AddHttpClient();
            services.AddScoped<GenericChannelRepository>();
            //services.AddScoped<IntegrationFactory>();
            //services.AddScoped<Service>();
            //services.AddScoped<ChannelController>();
            services.AddScoped<StreamManager>();
            services.AddScoped<ChannelRepository>();
            //services.AddScoped<CacheManager>();
            services.AddScoped<M3UManager>();
            //services.AddScoped<ElahmadManager>();
            //services.AddScoped<HTAManager>();
            //services.AddScoped<HalfIntegrateManager>();
            //services.AddScoped<IFixedChannelService, FixedChannelManager>();
            //services.AddScoped<IGenericChannelService, GenericChannelManager>();

            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IScheduleService, ScheduleService>();



            services.AddTransient<IIntegration, ElahmadStrategy>();
            services.AddTransient<IIntegration, HTAStrategy>();
            services.AddTransient<IIntegration, HalfIntegratedStrategy>();

            services.AddSingleton<IIntegrationRepository<ElahmadSettings>, IntegrationRepository<ElahmadSettings>>();

            services.AddSingleton<IIntegrationRepository<HalfIntegratedSettings>, IntegrationRepository<HalfIntegratedSettings>>();

            services.AddSingleton<IIntegrationRepository<HTASettings>, IntegrationRepository<HTASettings>>();

            services.AddSingleton<IChannelRepository, ChannelMongoRepository>();


            var client = new MongoClient(settings.MongoDBSettings.ConnectionString);
            var database = client.GetDatabase(settings.MongoDBSettings.Database);
            var channelsCollection = database.GetCollection<CommonChannelModel>("channels");

            var elahmadCollection = database.GetCollection<IntegrationBase<ElahmadSettings>>("integrations");

            var halfCollection = database.GetCollection<IntegrationBase<HalfIntegratedSettings>>("integrations");


            var htaCollection = database.GetCollection<IntegrationBase<HTASettings>>("integrations");

            services.AddSingleton(channelsCollection);
            services.AddSingleton(elahmadCollection);
            services.AddSingleton(halfCollection);
            services.AddSingleton(htaCollection);

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

            var service = serviceProvider.GetService<IScheduleService>();
            service.Syncronize();
            //service.TransferIntegrations();
            //recurringJobManager.AddOrUpdate(
            //    "Run every minute",
            //    () => ,
            //    "0 */18 * ? * *"
            //    );

        }
    }
}
