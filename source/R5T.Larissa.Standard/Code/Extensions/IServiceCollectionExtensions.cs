using System;

using Microsoft.Extensions.DependencyInjection;


using R5T.Caledonia;
using R5T.Caledonia.Default;
using R5T.Larissa.Configuration;
using R5T.Larissa.Configuration.Sardinia;
using R5T.Larissa.Default;


namespace R5T.Larissa.Standard
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSvnOperator(this IServiceCollection services)
        {
            services
                .AddSingleton<ISvnOperator, SvnOperator>()
                .AddSingleton<ISvnExecutableFilePathProvider, ConfigurationBasedSvnExecutableFilePathProvider>()
                .AddSvnConfiguration()
                .AddSingleton<ICommandLineInvocationOperator, DefaultCommandLineInvocationOperator>()
                ;

            return services;
        }
    }
}
