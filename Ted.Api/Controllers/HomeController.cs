using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ted.Bll.Interfaces;
using Ted.Model.DTO;


namespace Ted.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [Authorize]
        [HttpGet]
        [Route("LastExperience")]
        public async Task<IActionResult> GetUserSkills()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetLastExperience(userId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.InsertImage(userId, file, Path.GetExtension(file.FileName));
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UploadAudio")]
        public async Task<IActionResult> UploadAudio()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.InsertAudio(userId, file, Path.GetExtension(file.FileName));
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UploadVideo")]
        public async Task<IActionResult> UploadVideo()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.InsertVideo(userId, file, Path.GetExtension(file.FileName));
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PostMetadata")]
        public async Task<IActionResult> AddPostMetadata([FromBody] PostMetadataDTO postMetadata)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.AddPostMetadata(userId, postMetadata.Title, postMetadata.PostId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Post/{postId}")]
        public async Task<IActionResult> GetPost(string postId)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetPost(userId, postId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Posts/{page}")]
        public async Task<IActionResult> GetPosts(int page)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetPosts(userId, page);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ConnectionsCount")]
        public async Task<IActionResult> GetConnectionsCount()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetConnectionsCount(userId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("InsertArticle")]
        public async Task<IActionResult> PostArticle(List<string> content)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.InsertArticle(userId, content[0], content[1]);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.DeletePost(userId, id);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception e)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("SubscribeToPost/{id}")]
        public async Task<IActionResult> SubscribeToPost(string id)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.SubscribeToPost(userId, id);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("UnsubscribeFromPost/{id}")]
        public async Task<IActionResult> UnSubscribeToPost(string id)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.UnsubscribeFromPost(userId, id);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost(string id, PostDTO post)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.UpdatePost(userId, post);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PostComment/{postId}")]
        public async Task<IActionResult> PostComment(string postId, CommentDTO comment)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.PostComment(userId, postId, comment);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.DeleteComment(userId, commentId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("DownloadPhoto/{userToGetId}")]
        public async Task<IActionResult> DownloadPhoto(string userToGetId)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetPhoto(userId, userToGetId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);
            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }

        }

        [Authorize]
        [HttpGet]
        [Route("GetContent/{contentId}")]
        public async Task<IActionResult> GetContent(string contentId)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetContent(userId, contentId);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);
            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }

        }

    }
}