using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectAPI.Hubs;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("UserRole")]
    public class LikesController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<Groups> groupsUnitOfWork;
        private readonly IHubContext<CommunityHub> hubContext;
        private readonly IUnitOfWork<Posts> postsUnitOfWork;

        public LikesController(UserManager<AppUser> userManager,IUnitOfWork<Groups> GroupsUnitOfWork, IHubContext<CommunityHub> hubContext,IUnitOfWork<Posts> PostsUnitOfWork)
        {
            this.userManager = userManager;
            groupsUnitOfWork = GroupsUnitOfWork;
            this.hubContext = hubContext;
            postsUnitOfWork = PostsUnitOfWork;
        }

        [HttpPost("AddLike/{postId}")]

        
        public async Task<IActionResult> AddLike(string postId )
        {
            var post = await postsUnitOfWork.Entity.GetAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var group = await groupsUnitOfWork.Entity.GetAsync(post.groupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }
            var user = await userManager.GetUserAsync(User);

            if (post.likes.Any(x => x == user.Id))
            {
                return BadRequest("You have already liked this post");
            }
            else
            {
                post.likes.Add(user.Id);
            }

            await postsUnitOfWork.Entity.UpdateAsync(post);
            postsUnitOfWork.Save();
            await hubContext.Clients.Group(group.GroupName).SendAsync("Like", post.likes.Count());
            return Ok(new { LikesCount = post.likes.Count()});

        }


        [HttpPost("IsLiked/{postId}")]
        public async Task<IActionResult> IsLiked(string postId)
        {
            var post = await postsUnitOfWork.Entity.GetAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            var user = await userManager.GetUserAsync(User);
            if (post.likes.Any(x => x == user.Id))
            {
                return Ok(new { IsLiked = true });
            }
            else
            {
                return Ok(new { IsLiked = false });
            }
        }


        [HttpPost("RemoveLike/{postId}")]
        public async Task<IActionResult> RemoveLike(string postId)
        {
            var post = await postsUnitOfWork.Entity.GetAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var user = await userManager.GetUserAsync(User);

            if (post.likes.Any(x => x == user.Id))
            {
                post.likes.Remove(user.Id);
            }
            

            var group = await groupsUnitOfWork.Entity.GetAsync(post.groupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }
            
            await postsUnitOfWork.Entity.UpdateAsync(post);
            postsUnitOfWork.Save();
            await hubContext.Clients.Group(group.GroupName).SendAsync("Like", post.likes.Count());
            return Ok(new { LikesCount = post.likes.Count()});
        }
    }
}
