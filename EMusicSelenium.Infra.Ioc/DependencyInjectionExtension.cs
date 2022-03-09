using EMusic.Application.Interfaces;
using EMusic.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Emusic.Infra.Ioc
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IDownloadSoundCloudAppService, DownloadSoundCloudAppService>();
            services.AddScoped<IDownloadYoutubeAppService, DownloadYoutubeAppService>();
            return services;
        }
    }
}