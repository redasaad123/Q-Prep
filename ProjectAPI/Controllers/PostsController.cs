using Core.Interfaces;
using Core.Model;
using Core.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectAPI.DTO;
using ProjectAPI.Hubs;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("UserRole")]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork<Groups> groupsUnitOfWork;
        private readonly IHubContext<CommunityHub> hubContext;
        private readonly Service service;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<Posts> postsUnitOfWork;

        public PostsController(IUnitOfWork<Groups> GroupsUnitOfWork, IHubContext<CommunityHub> hubContext, Service service, UserManager<AppUser> userManager , IUnitOfWork<Posts> PostsUnitOfWork)
        {
            groupsUnitOfWork = GroupsUnitOfWork;
            this.hubContext = hubContext;
            this.service = service;
            this.userManager = userManager;
            postsUnitOfWork = PostsUnitOfWork;
        }


        [HttpGet("GetAllPosts/{groupId}")]
        

        public async Task<IActionResult> GetAllPosts(string groupId)
        {
            var postsList = await postsUnitOfWork.Entity.FindAll(p => p.groupId == groupId);
            if (postsList == null || !postsList.Any())
            {
                return NotFound("No posts found for this group");
            }
            var post = postsList.Select(p => new GetPost
            {
                PostId = p.PostId,
                TypeOfBody = p.TypeOfBody,
                header = p.header,
                Text = p.Text,
                Images = p.Images, // Assuming you have a way to get images
                likes = p.likes,
                postDate = p.postDate,
                UserId = p.UserId,
                UserImage = userManager.Users.FirstOrDefault(u => u.Id == p.UserId)?.Photo,
                UserName = userManager.Users.FirstOrDefault(u => u.Id == p.UserId)?.UserName
            }).ToList();
           

            return Ok(post);
        }


        [HttpPost("CreatePost")]
        
        public async Task<IActionResult> CreatePost(AddPostDTO dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);


            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var group = await groupsUnitOfWork.Entity.GetAsync(dto.groupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }
            var post = new Posts();
            post.PostId = Guid.NewGuid().ToString();
            post.TypeOfBody = dto.TypeOfBody;
            post.header = dto.header;
            post.Text = dto.Text;
            post.UserId = user.Id;

            if (dto.Images != null && dto.Images.Count > 0)
            {
                post.Images = new List<string>();
                foreach (var image in dto.Images)
                {
                    if (image.Length > 0)
                    {

                        post.Images.Add(await service.CompressAndSaveImageAsync(image, "PostPhotos"));

                    }
                }
            }
            post.likes = 0;
            post.postDate = DateTime.UtcNow;
            post.groupId = dto.groupId;
            await postsUnitOfWork.Entity.AddAsync(post);
            postsUnitOfWork.Save();
            await hubContext.Clients.Group(group.GroupName).SendAsync("Post", post);
            return Ok(post);
        }



        [HttpPut("UpdatePost/{PostId}")]
        
        public async Task<IActionResult> UpdatePost(UpdatePostDTO dto , string PostId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var post = await postsUnitOfWork.Entity.GetAsync(PostId);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            var group = await groupsUnitOfWork.Entity.GetAsync(post.groupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }


            post.header = dto.header;

            if (!post.TypeOfBody.ToLower().Contains("image"))
            {
                post.Text = dto.Text;
            }
            else
            {
                if (dto.Images != null && dto.Images.Count > 0)
                {
                    post.Images = new List<string>();
                    foreach (var image in dto.Images)
                    {
                        if (image.Length > 0)
                        {
                            post.Images.Add(await service.CompressAndSaveImageAsync(image, "PostPhotos"));
                        }
                    }
                }

            }

            await postsUnitOfWork.Entity.UpdateAsync(post);
            postsUnitOfWork.Save();

            await hubContext.Clients.Group(group.GroupName).SendAsync("Post", post);
            return Ok(post);
        }
        [HttpDelete("DeletePost/{PostId}")]
        public async Task<IActionResult> DeletePost(string PostId)
        {
            var post = await postsUnitOfWork.Entity.GetAsync(PostId);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            postsUnitOfWork.Entity.Delete(post);
            postsUnitOfWork.Save();
            return Ok("Post deleted successfully");
        }
    }
}
