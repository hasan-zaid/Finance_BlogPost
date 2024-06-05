using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Finance_BlogPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        private readonly IMediaRepository mediaRepository;

        public MediaController(IMediaRepository mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }


        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // call a repository
            var mediaURL = await mediaRepository.UploadAsync(file);

            if (mediaURL == null)
            {
                return Problem("Sometihng went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = mediaURL });
        }
    }
}
