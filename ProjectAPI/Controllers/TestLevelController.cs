using Core.Interfaces;
using Core.Model;
using Core.Servises;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.DTO;
using System.Linq;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLevelController : ControllerBase
    {
        private readonly Service service;
        private readonly IUnitOfWork<Test> testUnitOfWork;
        private readonly IUnitOfWork<Frameworks> frameworksUnitOfWork;
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;
     

        public TestLevelController(Service service,IUnitOfWork<Test> TestUnitOfWork, IUnitOfWork<Frameworks> FrameworksUnitOfWork, IUnitOfWork<MainTrack> mainTrackUnitOfWork)
        {
            this.service = service;
            testUnitOfWork = TestUnitOfWork;
            frameworksUnitOfWork = FrameworksUnitOfWork;
            this.mainTrackUnitOfWork = mainTrackUnitOfWork;

        }

        [HttpGet("QuestionForTesting/{framework}")]
        [Authorize("UserRole")]
        public async Task<IActionResult> GetQuestionForTesting(string framework)
        {
            var list = await service.RendomQuestions(framework);
            return Ok(list);
        }


        [HttpGet("DetermineLevel")]
        public IActionResult DetermineLevel(int count)
        {
            var Level  = service.DetermineLevel(count);
            return Ok(Level);

        }

        [HttpPost("CompareAnswer/{questionId}/{selectedAnwser}")]
        public async Task<IActionResult> CompareAnswer(string questionId, string selectedAnwser)
        {

            var vaild = await service.ComparerAnswer(questionId , selectedAnwser);
            return Ok(vaild);

        }



        [HttpPost("AddQuestionInTest")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddQuestionInTest(AddQuestionInTestDTO dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var queTest = new Test
            {
                Q_Id = Guid.NewGuid().ToString(),
                Qeuestion = dto.Qeuestion,
                A_1 = dto.A_1,
                A_2 = dto.A_2,
                A_3 = dto.A_3,
                A_4 = dto.A_4,
                CorrectAnswers = dto.CorrectAnswers,
                FrameworkId = dto.FrameworkId,

            };

            await testUnitOfWork.Entity.AddAsync(queTest);
            testUnitOfWork.Save();
            return Ok(queTest);

        }
        [HttpPut("UpdateQuestionInTest/{questionId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateQuestionInTest(UpdateQuestionInTestDTO dto, string questionId)
        {
            var question = await testUnitOfWork.Entity.GetAsync(questionId);
            if (question == null)
                return NotFound();

            question.Qeuestion = dto.Qeuestion ?? question.Qeuestion;
            question.A_1 = dto.A_1 ?? question.A_1;
            question.A_2 = dto.A_2 ?? question.A_2;
            question.A_3 = dto.A_3 ?? question.A_3;
            question.A_4 = dto.A_4 ?? question.A_4;
            question.CorrectAnswers = dto.CorrectAnswers ?? question.CorrectAnswers;

            await testUnitOfWork.Entity.UpdateAsync(question);

            testUnitOfWork.Save();

            return Ok(question);


        }
        [HttpDelete("DeleteQuestionInTest/{questionId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteQuestionInTest(string questionId)
        {
            var question = await testUnitOfWork.Entity.GetAsync(questionId);
            if (question == null)
                return NotFound();

            testUnitOfWork.Entity.Delete(question);
            testUnitOfWork.Save();
            return Ok("Question Is Deleted");

        }



    }
}
