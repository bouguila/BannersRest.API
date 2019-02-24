using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BannerFlow.Rest.Contracts;
using BannerFlow.Rest.Entities.Models;
using BannerFlow.Rest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BannerFlow.Rest.Controllers
{
    [ApiController]
    [Route("api/banner")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        //private ILogger _logger;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        
        // GET: api/Banner
        [HttpGet]
        public ActionResult<IEnumerable<Banner>> GetAll()
        {
            return _bannerService.GetAllBanners();
        }

        [HttpGet("{id}", Name = "GetBanner")]
        public ActionResult<Banner> GetById(int id)
        {
            var banner = _bannerService.GetBannerById(id);

            if (banner == null)
            {
                return NotFound();
            }

            return banner;
        }

        [HttpPost]
        public ActionResult<Banner> Create(Banner banner)
        {
            var existinBanner = _bannerService.GetBannerById(banner.Id);

            if (existinBanner != null)
            {
                return BadRequest("A Banner with same Id exists already");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bannerService.AddBanner(banner);
             return CreatedAtRoute("GetBanner", new { id = banner.Id.ToString() }, banner);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Banner bannerIn)
        {
            var banner = _bannerService.GetBannerById(id);

            if (banner == null)
            {
                return NotFound();
            }

            if (id != bannerIn.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bannerService.UpdateBanner(id, bannerIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            var banner = _bannerService.GetBannerById(id);

            if (banner == null)
            {
                return NotFound();
            }

            _bannerService.DeleteBannerById(banner.Id);

            return NoContent();
        }

        [HttpGet("{id}/html")]
        public IActionResult GetBannerHtml(int id)
        {
            var banner = _bannerService.GetBannerById(id);

            if (banner == null)
            {
                return NotFound();
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = banner.Html
            };
        }
    }
}