using EMusic.Application.Interfaces;
using EMusic.Application.Services;
using EMusic.Infrastructure.Services;
using Infrastructure.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Emusic.Infra.Ioc
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IDownloadSoundCloudlService, DownloadSoundCloudlService>();
            services.AddScoped<IDownloadYoutubeService, DownloadYoutubeService>();
            services.AddScoped<IZipFileMemoryStream, ZipFileMemoryStreamService>();
            return services;
        }
    }
}