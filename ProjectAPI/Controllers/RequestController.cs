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
    public class RequestController : ControllerBase
    {
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;
        private readonly IUnitOfWork<RequestQuestions> requestnUitOfWork;
        private readonly IUnitOfWork<BeginnerLevel> beginnerUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AdvancedLevel> advancedUnitOfWork;
        private readonly IUnitOfWork<IntermediateLevel> intermediateUnitOfWork;

        public RequestController(IUnitOfWork<MainTrack> MainTrackUnitOfWork, IUnitOfWork<RequestQuestions> RequestnUitOfWork, IUnitOfWork<BeginnerLevel> BeginnerUnitOfWork, UserManager<AppUser> userManager, IUnitOfWork<AdvancedLevel> AdvancedUnitOfWork, IUnitOfWork<IntermediateLevel> IntermediateUnitOfWork)
        {
            mainTrackUnitOfWork = MainTrackUnitOfWork;
            requestnUitOfWork = RequestnUitOfWork;
            beginnerUnitOfWork = BeginnerUnitOfWork;
            this.userManager = userManager;
            advancedUnitOfWork = AdvancedUnitOfWork;
            intermediateUnitOfWork = IntermediateUnitOfWork;
        }



        [HttpGet("GetRequests")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await requestnUitOfWork.Entity.GetAllAsync(); 
            if (requests == null)
                return NotFound("Not Found Any Request");


            var request =  requests.Select( x => new GetRequestsDTO
            {

                RequestId = x.RequestId,
                Answers = x.Answers,
                DateRequest = x.DateRequest,
                Questions = x.Questions,
                Email = userManager.Users.Where(i=>i.Id == x.UserId).Select(x => x.Email).FirstOrDefault() ?? "",
                UserName = userManager.Users.Where(i => i.Id == x.UserId).Select(x => x.UserName).FirstOrDefault() ?? "",
                Photo = userManager.Users.Where(i => i.Id == x.UserId).Select(x => x.Photo).FirstOrDefault() ?? "",
            });


            return Ok(request);


        }


        [HttpPost("AddQuestionRequest")]
        [Authorize("UserRole")]
        public async Task<IActionResult> AddQuestionRequest(AddQuestionsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This user Not Found");

            var Question = new RequestQuestions
            {
                RequestId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Answers = dto.Answers,
                FrameworkId = dto.FrameworkId,
                Questions = dto.Questions,
                DateRequest = DateTime.Now,
            };
            await requestnUitOfWork.Entity.AddAsync(Question);
            requestnUitOfWork.Save();
            return Ok(Question);
        }


        [HttpPost("UpdateRequest/{requestId}")]

        public async Task<IActionResult> UpdateRequest(UpdateRequestDTO dto , string requestId)
        {
            var req = await requestnUitOfWork.Entity.GetAsync(requestId);
            if(req == null)
                return NotFound("This Request Is Not Found");

            req.Questions = dto.Questions ?? req.Questions;
            req.Answers = dto.Answers ?? req.Answers;
            await requestnUitOfWork.Entity.UpdateAsync(req);
            requestnUitOfWork.Save();

            return Ok(req);
            


        }


        [HttpDelete("DeleteRequest/{requestId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteRequest(string requestId)
        {
            var req = await requestnUitOfWork.Entity.GetAsync(requestId);
            if (req == null)
                return NotFound("This Request Is Not Found");

            requestnUitOfWork.Entity.Delete(req);
            requestnUitOfWork.Save();
            
            return Ok("This Request Is Deleted");

        }


        
    }
}
