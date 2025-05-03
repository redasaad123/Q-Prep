using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.DTO;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("UserRole")]
    public class RecentController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<Recent> recentUnitOfWork;
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;

        public RecentController(UserManager<AppUser> userManager , IUnitOfWork<Recent> recentUnitOfWork , IUnitOfWork<MainTrack> mainTrackUnitOfWork)
        {
            this.userManager = userManager;
            this.recentUnitOfWork = recentUnitOfWork;
            this.mainTrackUnitOfWork = mainTrackUnitOfWork;
        }




        [HttpGet("GetRecently")]
        public async Task<IActionResult> GetRecently()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) 
                return NotFound();

            var trackList = await recentUnitOfWork.Entity.FindAll(x => x.UserId == user.Id);
            if (trackList == null)
                return NotFound();

            var trackOrder = trackList.OrderByDescending(d => d.DateTime).Select(async x => new GetTracksRecentlyGTO
            {
                MainTrack = await mainTrackUnitOfWork.Entity.GetAsync(x.MainTrackId)
            }).ToList();

            return Ok(trackOrder);
        }

        [HttpPost("AddInRecentTable/{mainTrackId}")]
        public async Task<IActionResult> AddInRecentTable(string mainTrackId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            if(user == null)
                return BadRequest("User Not Registed");
            var recent = new Recent
            {
                Id = Guid.NewGuid().ToString(),
                MainTrackId = mainTrackId,
                UserId = user.Id
            };
            await recentUnitOfWork.Entity.AddAsync(recent);
            recentUnitOfWork.Save();
            return Ok();


        }

    }
}
