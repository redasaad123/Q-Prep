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
    public class SaveController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<SaveQuestions> saveQuestionUnitOfWork;


        public SaveController(UserManager<AppUser> userManager , IUnitOfWork<SaveQuestions> saveQuestionUnitOfWork )
        {
            this.userManager = userManager;
            this.saveQuestionUnitOfWork = saveQuestionUnitOfWork;
        }

        [HttpGet("GetSaveQuestions")]

        public async Task<IActionResult> GetSaveQuestions()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            var Saves = await saveQuestionUnitOfWork.Entity.FindAll(x => x.UserId == user.Id);
            if (Saves.Count() == 0)
                return NotFound("Not Found Any Saves");

            var question = Saves.Select(x => new GetSaveQuestionDTO
            {
                Id = x.Id,
                questionId = x.QueId,
                UserId = x.UserId,
                Question = x.Question,
                Answer = x.Answer,
            });

            return Ok(question);


        }

        [HttpPost("AddtoSave")]
        public async Task<IActionResult> AddIntoSave(AddIntoSaveDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var valid =await saveQuestionUnitOfWork.Entity.Any(x=>x.UserId == user.Id && dto.Question == x.Question && x.Answer == dto.Answer);
            if (valid)
                return Ok("The Question is already existing ");
            var Save = new SaveQuestions
            {
                Id = Guid.NewGuid().ToString(),
                QueId = dto.QuestionId,
                Question = dto.Question,
                Answer = dto.Answer,
                UserId = user.Id
            };

            await saveQuestionUnitOfWork.Entity.AddAsync(Save);
            saveQuestionUnitOfWork.Save();
            return Ok(Save);
        }

        [HttpDelete("DeleteFromSave")]

        public async Task< IActionResult> DeleteFromSave(string Id)
        {
            var question = await saveQuestionUnitOfWork.Entity.GetAsync(Id);
            if (question == null) 
                return NotFound();
            saveQuestionUnitOfWork.Entity.Delete(question);
            saveQuestionUnitOfWork.Save();
            return Ok(question);

        }
    }
}
