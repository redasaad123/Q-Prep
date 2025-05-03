using Core.AuthenticationDTO;
using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("UserRole")]
    public class RolesController : ControllerBase
    {
        private readonly IAuthentication authentication;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(IAuthentication authentication,RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            this.authentication = authentication;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpGet("GetRoles")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetRols()
        {
            var roles = await roleManager.Roles.ToListAsync();
            if (roles.Count() == 0)
                return NotFound("Not Found Any Role");

            return Ok(roles);

        }

        [HttpGet("GetUserRolesfromAdmin/{userId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetUserRolesfromAdmin(string userId)
        {


            var user = await userManager.FindByIdAsync(userId);

            //var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User Not Existing");

            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count() == 0)
                 return NotFound("Not Found Any Role To This User");

            return Ok(roles);

        }


        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {


            //var user = await userManager.FindByIdAsync(userId);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User Not Existing");

            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count() == 0)
                return NotFound("Not Found Any Role To This User");

            return Ok(roles);

        }





        [HttpPost("AddRole")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddRole(RoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authentication.AddRole(dto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(dto);

        }

        [HttpPost("AddRolesToUser")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddRolesToUser([FromForm] RoleToUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authentication.AddRoleToUser(dto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(dto);

        }


        [HttpPut("UpdateRole/{roleId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateRole(string roleId , RoleDTO dto)
        {
            var RoleExist = await roleManager.FindByIdAsync(roleId);
            if (RoleExist == null)
                return NotFound("Not Found");

            if (await roleManager.RoleExistsAsync(dto.RoleName))
                return BadRequest("The Role Is Already Existing");

            RoleExist.Name = dto.RoleName;

            await roleManager.UpdateAsync(RoleExist);
            return Ok(dto);
        }

        [HttpPut("UpdateRoleToUser/{roleId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateRoleToUser(string roleId, RoleToUserDTO dto)
        {
            var RoleExist = await roleManager.FindByIdAsync(roleId);
            if (RoleExist == null)
                return NotFound("Role Is InValid");
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound("User Not Found");
            if(await userManager.IsInRoleAsync(user , RoleExist.Name))
                return BadRequest("The Role Is Already Existing");

            await userManager.RemoveFromRoleAsync(user, RoleExist.Name);
            var result = await userManager.AddToRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
                return Ok(dto);
            return BadRequest("SomeThing went wrong");

        }

        [HttpDelete("DeleteRole/{RoleId}")]
        [Authorize("AdminRole")]
        public async Task< IActionResult> DeleteRole(string roleId)
        {
            var RoleExist = await roleManager.FindByIdAsync(roleId);
            if (RoleExist == null)
                return NotFound("Role Is InValid");

            await roleManager.DeleteAsync(RoleExist);
            return Ok("The Role Is Deleted");
        }

        [HttpDelete("DeleteRoleFromUser/{RoleId}/{userId}")]
        public async Task<IActionResult> DeleteRoleFromUser(string roleId , string userId )
        {
            var RoleExist = await roleManager.FindByIdAsync(roleId);
            if (RoleExist == null)
                return NotFound("Role Is InValid");
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User Not Found");
            await userManager.RemoveFromRoleAsync(user, RoleExist.Name);
            return Ok("The Role Is Deleted");
        }



    }
}
