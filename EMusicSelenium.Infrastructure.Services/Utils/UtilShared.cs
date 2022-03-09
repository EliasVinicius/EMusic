using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMusic.Infra.Services.Utils
{
    public static class UtilShared
    {
        public static void DeleteArquives(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            Directory.Delete(path);
        }

        public static async Task<MemoryStream> ZipFiles(string pathFolderStream)
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathSoundCloud = Path.Combine(userPath, pathFolderStream);
            var musicPath = Directory.GetFiles(pathSoundCloud);

            var zipFileMemoryStream = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
            {
                foreach (var music in musicPath)
                {
                    var musicFileName = Path.GetFileName(music);
                    var entry = archive.CreateEntry(musicFileName);
                    using var entryStream = entry.Open();
                    using var fileStream = System.IO.File.OpenRead(music);
                    await fileStream.CopyToAsync(entryStream);
                }
            }

            zipFileMemoryStream.Seek(0, SeekOrigin.Begin);

            return zipFileMemoryStream;
        }
    }
}
