using EMusic.Application.Interfaces;
using EMusic.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMusic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        readonly IDownloadSoundCloudAppService _downloadService;

        public DownloadController(IDownloadSoundCloudAppService downloadService)
        {
            _downloadService = downloadService;
        }


        [HttpGet("soundCloud")]
        public async Task<IActionResult> DownloadMusicSoundCloud([FromQuery] Uri urlSoundCloud)
        {
            return File(await _downloadService.Download(urlSoundCloud), "application/octet-stream", "SoundCloudMusic.zip");
        }


        [HttpGet("youTube")]
        public async Task<IActionResult> DownloadMusicYoutube(string urlyouTube)
        {
            return Ok();
        }
    }
}
