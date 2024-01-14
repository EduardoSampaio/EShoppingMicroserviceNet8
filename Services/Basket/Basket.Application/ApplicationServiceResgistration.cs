using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basket.Application;
public static class ApplicationServiceResgistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}