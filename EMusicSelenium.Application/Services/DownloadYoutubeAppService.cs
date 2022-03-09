using EMusic.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMusic.Application.Services
{
    public class DownloadYoutubeAppService : IDownloadYoutubeAppService
    {
        public Task<MemoryStream> Download(Uri url)
        {
            throw new NotImplementedException();
        }

        public async Task<MemoryStream> DownloadService(Uri urlProfileYoutube)
        {
            throw new NotImplementedException();
        }
    }
}
