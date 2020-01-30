using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAKafka.Domain.Models;
using CAKafka.Library.Publish;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradeBank3.BackgroundListener;
using TradeBank3.DBContext;
using TradeBank3.ServiceLayer;

namespace TradeBank3
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

            services.AddHttpClient("TradeBankProject", client => {
                client.BaseAddress = new Uri("https://fx-cts.azurewebsites.net");
            });

            services.AddControllersWithViews();

            services.AddDbContext<TradeBankContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("TradeBankDB")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddOptions();
            services.Configure<KafkaOptions>(Configuration.GetSection("Kafka"));


            services.AddControllers();

            services.AddMvc()
                .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });
            services.AddLogging();

            services.AddRouting(option => { option.LowercaseUrls = true; });

            services.For<IMediator>().Use<Mediator>().Transient();
            services.For<ServiceFactory>().Use(ctx => ctx.GetInstance);
            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });

            //Kafka
            services.For(typeof(IKafkaPublisher<>)).Add(typeof(KafkaPublisher<>)).Scoped();
            services.AddHostedService<BaselineListener>();

            services.AddTransient<IUserInput, UserInput>();
        }
    }
}
