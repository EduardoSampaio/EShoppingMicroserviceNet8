using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Basket.Application;
using Basket.Application.GrpcService;

namespace Basket.API;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();

        //Redis Settings
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });
        services.AddApplicationService();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<DiscountGrpcService>();
        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
            (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));
        services.AddSwaggerGen(c =>
           c.SwaggerDoc("v1",
           new OpenApiInfo()
           {
               Title = "Basket.APi",
               Version = "v1",
           }
           ));

        services.AddHealthChecks()
          .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Basket.API V1"));
           
        }

        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}
