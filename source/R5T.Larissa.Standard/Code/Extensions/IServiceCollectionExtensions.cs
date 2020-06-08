using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using R5T.Caledonia;
using R5T.Caledonia.Default;
using R5T.Dacia;
using R5T.Larissa.Configuration;
using R5T.Larissa.Configuration.Sardinia;
using R5T.Larissa.Default;
using R5T.Lombardy;


namespace R5T.Larissa.Standard
{
    public static class IServiceCollectionExtensions
    {
        public static
            (
            IServiceAction<ICommandLineInvocationOperator> commandLineInvocationOperatorAction,
            IServiceAction<IOptions<SvnConfiguration>> svnConfigurationOptions,
            IServiceAction<ISvnOperator> svnOperatorAction,
            IServiceAction<ISvnExecutableFilePathProvider> svnExecutableFilePathProviderAction
            )
        AddSvnOperatorAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction,
            IServiceAction<ILogger> loggerAction)
        {
            // 0.
            var commandLineInvocationOperatorAction = ServiceAction<ICommandLineInvocationOperator>.New(() => services.AddSingleton<ICommandLineInvocationOperator, DefaultCommandLineInvocationOperator>());
            var svnConfigurationOptionsAction = services.AddSvnConfigurationAction();

            // 1.
            var svnExecutableFilePathProviderAction = ServiceAction<ISvnExecutableFilePathProvider>.New(() =>
            {
                services
                    .AddSingleton<ISvnExecutableFilePathProvider, ConfigurationBasedSvnExecutableFilePathProvider>()
                    .Run(svnConfigurationOptionsAction)
                    ;
            });

            // 2.
            var svnOperatorAction = ServiceAction<ISvnOperator>.New(() => services.AddSvnOperator(
                commandLineInvocationOperatorAction,
                stringlyTypedPathOperatorAction,
                svnExecutableFilePathProviderAction,
                loggerAction));

            var output =
            (
                commandLineInvocationOperatorAction,
                svnConfigurationOptionsAction,
                svnOperatorAction,
                svnExecutableFilePathProviderAction
            );
            return output;
        }

        public static IServiceCollection AddSvnOperator(this IServiceCollection services)
        {
            services
                .AddSingleton<ICommandLineInvocationOperator, DefaultCommandLineInvocationOperator>()
                .AddSingleton<ISvnOperator, SvnOperator>()
                .AddSingleton<ISvnExecutableFilePathProvider, ConfigurationBasedSvnExecutableFilePathProvider>()
                .AddSingleton<ISvnversionExecutableFilePathProvider, ConfigurationBasedSvnversionExecutableFilePathProvider>()
                .AddSingleton<ISvnversionOperator, SvnversionOperator>()
                .AddSvnConfiguration()
                ;

            return services;
        }

        public static 
            (
            IServiceAction<ISvnversionExecutableFilePathProvider> svnversionExecutableFilePathProviderAction,
            IServiceAction<ISvnversionOperator> svnversionOperatorAction
            )
        AddSvnversionOperatorAction(this IServiceCollection services,
            IServiceAction<ILogger> loggerAction)
        {
            // 0.
            var svnConfigurationOptionsAction = services.AddSvnConfigurationAction();

            // 1.
            var svnversionExecutableFilePathProviderAction = ServiceAction<ISvnversionExecutableFilePathProvider>.New(() =>
            {
                services
                    .AddSingleton<ISvnversionExecutableFilePathProvider, ConfigurationBasedSvnversionExecutableFilePathProvider>()
                    .Run(svnConfigurationOptionsAction)
                    ;
            });

            // 2.
            var svnversionOperatorAction = ServiceAction<ISvnversionOperator>.New(() =>
            {
                services
                    .AddSingleton<ISvnversionOperator, SvnversionOperator>()
                    .Run(svnversionExecutableFilePathProviderAction)
                    .Run(loggerAction)
                    ;
            });

            var output =
            (
                svnversionExecutableFilePathProviderAction,
                svnversionOperatorAction
            );
            return output;
        }
    }
}
