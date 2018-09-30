using System;
using System.Collections.Generic;
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
    public class JobController : ControllerBase
    {
        private readonly IAdService _adService;

        public JobController(IAdService adService)
        {
            _adService = adService;
        }

        [Authorize]
        [HttpPost]
        [Route("AddJob")]
        public async Task<IActionResult> AddAd([FromBody]AdDTO ad)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adService.AddAd(userId, ad);
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
        [Route("GetJobs")]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adService.GetAds(userId);
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
        [Route("GetMyJobs")]
        public async Task<IActionResult> GetMyJobs()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adService.GetMyAds(userId);
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
        [Route("applyToAd/{id}")]
        public async Task<IActionResult> GetMyJobs(string id)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adService.ApplyToAd(userId, id);
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
        [Route("deleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adService.DeleteAd(userId, id);
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