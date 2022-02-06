using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IZipFileMemoryStream
    {
        Task<MemoryStream> ZipFile(string pathFolderStream);
    }
}
