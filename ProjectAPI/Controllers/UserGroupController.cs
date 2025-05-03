using Azure.Core;
using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectAPI.DTO;
using ProjectAPI.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IUnitOfWork<Groups> groupsUnitOfWork;
        private readonly IHubContext<CommunityHub> hubContext;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<UserGroup> userGroupUnitOfWork;

        public UserGroupController(IUnitOfWork<Groups> GroupsUnitOfWork, IHubContext<CommunityHub> hubContext, UserManager<AppUser> userManager, IUnitOfWork<UserGroup> UserGroupUnitOfWork)
        {
            groupsUnitOfWork = GroupsUnitOfWork;
            this.hubContext = hubContext;
            this.userManager = userManager;
            userGroupUnitOfWork = UserGroupUnitOfWork;
        }

        [HttpGet("GetAllUserGroups")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetAllUserGroups()
        {
            var userGroups = await userGroupUnitOfWork.Entity.GetAllAsync();
            if (userGroups == null || !userGroups.Any())
            {
                return NotFound("No user groups found.");
            }
            return Ok(userGroups);
        }


        [HttpGet("GetUserGroupById/{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetUserGroupById(string id)
        {
            var userGroup = await userGroupUnitOfWork.Entity.GetAsync(id);
            if (userGroup == null)
            {
                return NotFound($"User group with ID {id} not found.");
            }
            return Ok(userGroup);
        }


        [HttpPost("JoinGroup")]
        [Authorize("UserRole")]
        public async Task<IActionResult> JoinGroup([FromBody] GroupRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = await userManager.GetUserAsync(User);

            await hubContext.Groups.AddToGroupAsync(dto.ConnectionId, dto.GroupName);

            var existingUserGroup = userGroupUnitOfWork.Entity
                .Find(x => x.GroupName == dto.GroupName && x.UserID == user.Id);

            if (existingUserGroup != null) 
                return Ok(new { Message = $"{user.Name} already joined group {dto.GroupName}" });

            var userGroup = new UserGroup
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = dto.GroupName,
                UserID = user.Id
            };
            var group = groupsUnitOfWork.Entity.Find(x => x.GroupName == dto.GroupName);
            if (group == null)
            {
                return NotFound($"Group with name {dto.GroupName} not found.");
            }



            group.NumberOfMembers = (int.Parse(group.NumberOfMembers) + 1).ToString();
            await userGroupUnitOfWork.Entity.AddAsync(userGroup);
            await groupsUnitOfWork.Entity.UpdateAsync(group);
            userGroupUnitOfWork.Save();
            return Ok(new { Message = $"Joined group {dto.GroupName}" });
        }
        [HttpPost("LeaveGroup")]
        [Authorize("UserRole")]
        public async Task<IActionResult> LeaveGroup( GroupRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);

            await hubContext.Groups.RemoveFromGroupAsync(dto.ConnectionId, dto.GroupName);

            var userGroup = userGroupUnitOfWork.Entity.Find(x => x.GroupName == dto.GroupName && x.UserID == user.Id);
            if (userGroup == null)
                return NotFound($"{user.Name} already  not found");

            var group = groupsUnitOfWork.Entity.Find(x => x.GroupName == dto.GroupName);
            if (group == null)
                return NotFound($"Group with name {dto.GroupName}");

            group.NumberOfMembers = (int.Parse(group.NumberOfMembers) - 1).ToString();

            await groupsUnitOfWork.Entity.UpdateAsync(group);
            userGroupUnitOfWork.Entity.Delete(userGroup);
            userGroupUnitOfWork.Save();

            return Ok(new { Message = $"Left group {dto.GroupName}" });
        }

        

    }
}
