using System;

using Microsoft.Extensions.Configuration;
using R5T.Larissa.Configuration.Suebia;


namespace R5T.Larissa.Standard
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSvnConfiguration(this IConfigurationBuilder configurationBuilder, IServiceProvider configurationServiceProvider)
        {
            configurationBuilder.AddSvnConfigurationJsonFile(configurationServiceProvider);

            return configurationBuilder;
        }
    }
}
