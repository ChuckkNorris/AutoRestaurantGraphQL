using AutoRestaurant.Api.Modules.Common.Interfaces;
using AutoRestaurant.Api.Modules.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRestaurant.Api.Modules.Configuration {
    public static class AppServiceConfigurator {
        public static void AddAppServices(this IServiceCollection services) {
            services.AddServicesOfType<IRepository>(ServiceLifetime.Transient);
            services.AddServicesOfType<ISingleton>(ServiceLifetime.Singleton);
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new AutoRestaurantSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            //var serviceImplementations = GetAllImplementationsOfType<IRepository>();

            //// Add each service as a transient
            //foreach (var serviceType in serviceImplementations) {
            //    services.AddTransient(serviceType);
            //}
        }

        private static void AddServicesOfType<T>(this IServiceCollection services, ServiceLifetime serviceLifetime) where T : class {
            // Retrieve list of types that implement specified class/interface
            var serviceImplementations = GetAllImplementationsOfType<T>();

            // Add each service as a transient
            foreach (var serviceType in serviceImplementations) {
                switch (serviceLifetime) {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(serviceType);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(serviceType);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(serviceType);
                        break;
                    default:
                        services.AddTransient(serviceType);
                        break;
                }
            }
        }

        private static IEnumerable<Type> GetAllImplementationsOfType<T>() where T : class {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(type =>
                typeof(T).IsAssignableFrom(type) && !type.IsInterface
            ));
        }
    }
}
