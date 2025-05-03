using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUnitOfWork<Groups> groupsUnitOfWork;
        private readonly IHubContext<CommunityHub> hubContext;
        private readonly IUnitOfWork<Posts> postsUnitOfWork;

        public LikesController(IUnitOfWork<Groups> GroupsUnitOfWork, IHubContext<CommunityHub> hubContext,IUnitOfWork<Posts> PostsUnitOfWork)
        {
            groupsUnitOfWork = GroupsUnitOfWork;
            this.hubContext = hubContext;
            postsUnitOfWork = PostsUnitOfWork;
        }

        [HttpPost("AddLike/{postId}")]
        
        public async Task<IActionResult> AddLike(string postId)
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
            post.likes++;
            await postsUnitOfWork.Entity.UpdateAsync(post);
            postsUnitOfWork.Save();
            await hubContext.Clients.Group(group.GroupName).SendAsync("Like", post);
            return Ok(new { LikesCount = post.likes });

        }


        [HttpPost("RemoveLike/{postId}")]
        public async Task<IActionResult> RemoveLike(string postId)
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
            post.likes--;
            await postsUnitOfWork.Entity.UpdateAsync(post);
            postsUnitOfWork.Save();
            await hubContext.Clients.Group(group.GroupName).SendAsync("Like", post);
            return Ok(new { LikesCount = post.likes });
        }
    }
}
