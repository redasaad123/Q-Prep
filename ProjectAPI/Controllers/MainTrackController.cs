using Core.Interfaces;
using Core.Model;
using Core.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.DTO;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainTrackController : ControllerBase
    {
        private readonly Service service;
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;

        public MainTrackController(Service service, IUnitOfWork<MainTrack> mainTrackUnitOfWork)
        {
            this.service = service;
            this.mainTrackUnitOfWork = mainTrackUnitOfWork;
        }


        [HttpGet("GetMainTrack")]
        public async Task<IActionResult> GetMainTrack()
        {
            var Tracks = await mainTrackUnitOfWork.Entity.GetAllAsync();
            return Ok(Tracks);

        }

        [HttpPost("AddMainTrack")]
        [Authorize("AdminRole")]

        public async Task<IActionResult> AddMainTrack(AddMainTrackDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var vaild = await mainTrackUnitOfWork.Entity.Any(x => x.TarckName == dto.TarckName);
            if (vaild)
                return BadRequest("This Framework is already existing");

            var UrlPhoto = "69c3c85f8aca980abdcfb79fe815dfbb.png";

            if (dto.Photo != null)
            {
                var compressedImage = await service.CompressAndSaveImageAsync(dto.Photo, "TrackandFrameworkPhoto", 800, 50);
                UrlPhoto = compressedImage;
            }

            var track = new MainTrack
            {
                TrackId = Guid.NewGuid().ToString(),
                TarckName = dto.TarckName,
                Photo = UrlPhoto,
                description = dto.description,

            };

            await mainTrackUnitOfWork.Entity.AddAsync(track);

            mainTrackUnitOfWork.Save();

            return Ok(track);



        }
        [HttpPut("UpdateMaintrack/{TrackId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateMaintrack(UpdateMainTrackDTO dto , string TrackId)
        {
            var track = await mainTrackUnitOfWork.Entity.GetAsync(TrackId);

            if (track == null)
                return NotFound("This Track Not Found");

            if (dto.TarckName == track.TarckName)
                return BadRequest("This Track Is already Exist");

            if (dto.Photo != null)
            {
                var compressedImage = await service.CompressAndSaveImageAsync(dto.Photo, "TrackandFrameworkPhoto", 800, 50);
                track.Photo = compressedImage;
            }

            track.TarckName = dto.TarckName ?? track.TarckName;
            track.description = dto.description ?? track.description;

            await mainTrackUnitOfWork.Entity.UpdateAsync(track);

            mainTrackUnitOfWork.Save();

            return Ok(track);

        }
        [HttpDelete("DeleteTrack/{trackID}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteTrack(string trackID)
        {
            var track = await mainTrackUnitOfWork.Entity.GetAsync(trackID);

            if (track == null)
                return NotFound("This Track Not Found");

            mainTrackUnitOfWork.Entity.Delete(track);
            mainTrackUnitOfWork.Save();
            return Ok(track);

        }

    }
}
