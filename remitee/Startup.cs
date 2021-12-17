using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using remitee.Middlewares;
using remitee.Repositories;
using remitee.Repositories.Interfaces;
using remitee.Services;
using remitee.Services.Interfaces;
using System;

namespace remitee
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

            services.AddDbContext<AppRemiteeContext>(option => option.UseInMemoryDatabase("test"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "remitee", Version = "v1" });
            });

            services.AddHangfire(option => 
                option.UseMemoryStorage()
            );

            services.AddHangfireServer();

            services.AddTransient<IQuoteService, QuoteService>();
            services.AddTransient<IQuoteRepository, QuoteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "remitee v1"));
            }

            app.UseErrorHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
            var quoteService = (IQuoteService)serviceProvider.GetService(typeof(IQuoteService));
            quoteService.UpdateQuote().Wait();
            recurringJobManager.AddOrUpdate("UpdateQuote", () => quoteService.UpdateQuote(), "*/30 * * * *");
        }
    }
}
