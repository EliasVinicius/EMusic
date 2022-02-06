using Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Services
{
    public class ZipFileMemoryStreamService : IZipFileMemoryStream
    {
        public async Task<MemoryStream> ZipFile(string pathFolderStream)
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

            DeleteArchives(pathSoundCloud);
            return zipFileMemoryStream;
        }

        private void DeleteArchives(string pathSoundCloud)
        {
            DirectoryInfo di = new DirectoryInfo(pathSoundCloud);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            Directory.Delete(pathSoundCloud);
        }
    }
}
