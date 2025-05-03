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
    public class FrameworksController : ControllerBase
    {
        private readonly Service service;
        private readonly IUnitOfWork<Frameworks> frameworksUnitOfWork;
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;

        public FrameworksController(Service service, IUnitOfWork<Frameworks> FrameworksUnitOfWork ,IUnitOfWork<MainTrack> mainTrackUnitOfWork )
        {
            this.service = service;
            frameworksUnitOfWork = FrameworksUnitOfWork;
            this.mainTrackUnitOfWork = mainTrackUnitOfWork;

        }


        [HttpGet("GetFramework/{mainTrackId}")]
        public async Task<IActionResult> GetFramework(string mainTrackId)
        {
            var track = await mainTrackUnitOfWork.Entity.GetAsync(mainTrackId);
            if (track == null) 
                return NotFound("This track Not Found");

            var frameworks = await frameworksUnitOfWork.Entity.FindAll(x=>x.MainTrackId == mainTrackId);

            if (frameworks.Count == 0)
                return NotFound("This Track Hasn't Any Framwork!");

            return Ok(frameworks);

        }

        [HttpPost ("AddFramework")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddFramework(AddFramewordDTO dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var track = await mainTrackUnitOfWork.Entity.GetAsync(dto.MainTrackId);
            if (track == null) 
                return NotFound("This track Not Found");

            var vaild = await frameworksUnitOfWork.Entity.Any(x => x.FrameworkName == dto.FramworkName);
            if (vaild)
                return BadRequest("This Framework is already existing");


            string photo = "69c3c85f8aca980abdcfb79fe815dfbb.png";
            if (dto.Photo != null)
            {
                var compressedImage = await service.CompressAndSaveImageAsync(dto.Photo, "TrackandFrameworkPhoto", 800, 50);
                photo = compressedImage;
            }

            var framework = new Frameworks
            {
                FrameworkId = Guid.NewGuid().ToString(),
                FrameworkName = dto.FramworkName,
                description = dto.description,
                MainTrackId = dto.MainTrackId,
                Photo = photo,
            };

            await frameworksUnitOfWork.Entity.AddAsync(framework);
            frameworksUnitOfWork.Save();

            return Ok(framework);



        }

        [HttpPut("UpdateFramewrok/{frameworkId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateFramewrok(string frameworkId , UpdateFramewordDTO dto)
        {
            var framework = await frameworksUnitOfWork.Entity.GetAsync(frameworkId);

            if (framework == null)
                return NotFound("This Framwork Not Found");

            if (dto.FrameworkName == framework.FrameworkName)
                return BadRequest("This FrameworkName Is already Exist");

            if (dto.Photo != null)
            {
                var compressedImage = await service.CompressAndSaveImageAsync(dto.Photo, "TrackandFrameworkPhoto", 800, 50);
                framework.Photo = compressedImage;
            }
            framework.description = dto.description ?? framework.description;
            framework.FrameworkName = dto.FrameworkName ?? framework.FrameworkName;

            await frameworksUnitOfWork.Entity.UpdateAsync(framework);
            frameworksUnitOfWork.Save();
            return Ok(framework);


        }


        [HttpDelete("DeleteFramework/{frameworkId}")]
        [Authorize("AdminRole")]

        public async Task<IActionResult> DeleteFramework(string frameworkId)
        {
            var framework = await frameworksUnitOfWork.Entity.GetAsync(frameworkId);

            if (framework == null)
                return NotFound("This Framework Not Found");

             frameworksUnitOfWork.Entity.Delete(framework);
            frameworksUnitOfWork.Save();

            return Ok("The Framework Is Deleted");


        }









    }
}
