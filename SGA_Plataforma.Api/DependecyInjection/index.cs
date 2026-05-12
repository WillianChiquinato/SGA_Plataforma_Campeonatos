using System.Reflection;

namespace SGA_Plataforma.Api.DependecyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Services
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // Repositories
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // Jobs
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Jobs")))
            .AsSelf()
            .WithScopedLifetime()
        );

        return services;
    }
}