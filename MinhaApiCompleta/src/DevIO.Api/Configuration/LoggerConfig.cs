using System;
using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {                        
            services.AddElmahIo(o =>
            {
                o.ApiKey = "5c2e96e18de54911bf83974c3dea849b";
                o.LogId = new Guid("630865eb-3cb4-4470-8827-5e5047fc9552");
            });

            ///*Insere Logs Injetados manualmente*/
            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "5c2e96e18de54911bf83974c3dea849b";
            //        o.LogId = new Guid("630865eb-3cb4-4470-8827-5e5047fc9552");
            //    });
            //    //builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Information);
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});

            
            services.AddHealthChecks()
                    .AddElmahIoPublisher("5c2e96e18de54911bf83974c3dea849b", new Guid("630865eb-3cb4-4470-8827-5e5047fc9552"), "API Fornecedores")
                    .AddCheck("SelectProdutos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQLServer");

            services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();
            
            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/api/hc-ui"; });
            return app;
        }
    }
}