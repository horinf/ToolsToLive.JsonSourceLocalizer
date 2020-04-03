using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ToolsToLive.JsonSourceLocalizer.Interfaces;

namespace ToolsToLive.JsonSourceLocalizer
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds localizer which uses JSON file as a source.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configurationSection">Configuration section with localization settings.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddJsonSourceLocalizer(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddOptions();
            services.Configure<JsonSourceLocalizerSettings>(configurationSection);
            services.AddSingleton(x => x.GetRequiredService<IOptions<JsonSourceLocalizerSettings>>().Value);

            services.AddSingleton<IJsonSourceLocalizer, JsonSourceLocalizer>();
            services.AddSingleton<IJsonResourceManager, JsonResourceManager>();

            return services;
        }

        /// <summary>
        /// Adds localizer which uses JSON file as a source.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="setupAction">Action to configure settings.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddJsonSourceLocalizer(this IServiceCollection services, Action<JsonSourceLocalizerSettings> setupAction)
        {
            var settings = new JsonSourceLocalizerSettings();
            setupAction(settings);

            services.AddSingleton(settings);

            services.AddSingleton<IJsonSourceLocalizer, JsonSourceLocalizer>();
            services.AddSingleton<IJsonResourceManager, JsonResourceManager>();

            return services;
        }
    }
}
