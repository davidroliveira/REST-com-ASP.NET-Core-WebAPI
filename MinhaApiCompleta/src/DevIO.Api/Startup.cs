using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();

            services.AddSwaggerConfig();

            services.AddLoggingConfiguration(Configuration);

            /*Movido para o config de logger do elmah
            services.AddHealthChecks()
                    .AddCheck("SelectProdutos", new SqlServerHealthCheck(Configuration.GetConnectionString("DefaultConnection")))
                    .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), name:"BancoSQLServer");

            services.AddHealthChecksUI();
            */

            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvcConfiguration();

            app.UseSwaggerConfig(provider);

            app.UseLoggingConfiguration();

            /*Movido para o config de logger do elmah
            app.UseHealthChecks("/api/hc", new HealthCheckOptions() 
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/api/hc-ui"; });
            */
        }
    }
}
