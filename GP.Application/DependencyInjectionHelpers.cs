using GP.Application;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

internal static class DependencyInjectionHelpers
{


    public static IServiceCollection AddMediator(this IServiceCollection services)
    {

        //services.AddMediatR(Assembly.GetExecutingAssembly());


        return services;
    }
}