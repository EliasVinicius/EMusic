using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMusic.Infrastructure.Services
{
    public interface IZipFileMemoryStream
    {
        Task<MemoryStream> ZipFile(string pathFolderStream);
    }
}
