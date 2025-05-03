using Core.Interfaces;
using Core.Model;
using Core.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.DTO;
using System.Text.RegularExpressions;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly Service service;
        private readonly IUnitOfWork<Groups> groupUnitOfWork;

        public GroupsController(Service service , IUnitOfWork<Groups> GroupUnitOfWork)
        {
            this.service = service;
            groupUnitOfWork = GroupUnitOfWork;
        }

        [HttpGet("GetAllGroups")]
        [Authorize("UserRole")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await groupUnitOfWork.Entity.GetAllAsync();
            if (groups == null || !groups.Any())
            {
                return NotFound("No groups found.");
            }
            return Ok(groups);
        }


        [HttpGet("GetGroupById/{id}")]
        [Authorize("UserRole")]
        public async Task<IActionResult> GetGroupById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Group ID cannot be null or empty.");
            }
            var group = await groupUnitOfWork.Entity.GetAsync(id);
            if (group == null)
            {
                return NotFound($"Group with ID {id} not found.");
            }
            return Ok(group);
        }


        [HttpPost("CreateGroup")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> CreateGroup(AddGroupDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingGroup = groupUnitOfWork.Entity
                .Find(x => x.GroupName == dto.GroupName);
            if(existingGroup != null)
            {
                return BadRequest("Group with this name already exists.");
            }


            string photo = null;
            if (dto.Photo != null) 
                  photo = await service.CompressAndSaveImageAsync(dto.Photo, "GroupsPhoto");


            var group = new Groups
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = dto.GroupName,
                Photo = photo,
                Description = dto.Description,
                NumberOfMembers = "0"
            };

            await groupUnitOfWork.Entity.AddAsync(group);
            groupUnitOfWork.Save();

            return Ok(group);

        }

        [HttpPut("UpdateGroup/{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult>  UpdateGroup(string id, UpdateGroupDTO dto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Group ID cannot be null or empty.");
            }
            var group = await groupUnitOfWork.Entity.GetAsync(id);
            if (group == null)
            {
                return NotFound($"Group with ID {id} not found.");
            }
            group.GroupName = dto.GroupName ?? group.GroupName;
            group.Description = dto.Description ?? group.Description;
            if (dto.Photo != null)
            {
                group.Photo = await service.CompressAndSaveImageAsync(dto.Photo, "GroupsPhoto");
            }
           await groupUnitOfWork.Entity.UpdateAsync(group);
            groupUnitOfWork.Save();
            return Ok(group);
        }

        [HttpDelete("DeleteGroup/{id}")]
        [Authorize("AdminRole")]
        public async Task< IActionResult> DeleteGroup(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Group ID cannot be null or empty.");
            }
            var group = await groupUnitOfWork.Entity.GetAsync(id);
            if (group == null)
            {
                return NotFound($"Group with ID {id} not found.");
            }
            groupUnitOfWork.Entity.Delete(group);
            groupUnitOfWork.Save();
            return Ok($"Group with ID {id} deleted successfully.");
        }
    }
}
