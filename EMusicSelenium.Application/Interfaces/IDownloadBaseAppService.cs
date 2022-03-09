using EMusic.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMusic.Application.Interfaces
{
    public interface IDownloadBaseAppService
    {
        Task<MemoryStream> Download(Uri url);
    }
}
