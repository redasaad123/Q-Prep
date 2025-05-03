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
    public class QuestionsController : ControllerBase
    {
        private readonly IUnitOfWork<Frameworks> frameworksUnitOfWork;
        private readonly IUnitOfWork<RequestQuestions> requestnUitOfWork;
        private readonly IUnitOfWork<BeginnerLevel> beginnerUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AdvancedLevel> advancedUnitOfWork;
        private readonly IUnitOfWork<IntermediateLevel> intermediateUnitOfWork;

        public QuestionsController(IUnitOfWork<Frameworks> frameworksUnitOfWork, IUnitOfWork<RequestQuestions> RequestnUitOfWork, IUnitOfWork<BeginnerLevel> BeginnerUnitOfWork, UserManager<AppUser> userManager, IUnitOfWork<AdvancedLevel> AdvancedUnitOfWork, IUnitOfWork<IntermediateLevel> IntermediateUnitOfWork)
        {
            this.frameworksUnitOfWork = frameworksUnitOfWork;
            requestnUitOfWork = RequestnUitOfWork;
            beginnerUnitOfWork = BeginnerUnitOfWork;
            this.userManager = userManager;
            advancedUnitOfWork = AdvancedUnitOfWork;
            intermediateUnitOfWork = IntermediateUnitOfWork;
        }

        [HttpGet("Q_BeginnerLevel/{frameworkId}")]
        public async Task<IActionResult> Q_beginner(string frameworkId)
        {
            var Questions = await beginnerUnitOfWork.Entity.FindAll(x=>x.FrameworkId == frameworkId);
            if (Questions == null)
                return NotFound("Not Found Any Questions !");
            var Question = Questions.Select( x => new GetQuestionsDTO
            {
                QuestionId = x.QuestionId,
                LevelName = x.LevelName,
                Questions = x.Questions,
                Answers = x.Answers,
                FrameworkName = frameworksUnitOfWork.Entity.Find(x => x.FrameworkId == frameworkId).FrameworkName ?? "",


            }).ToList();

            return Ok(Question);

        }

        [HttpGet("Q_IntermediateLevel/{frameworkId}")]
        public async Task<IActionResult> Q_Intermediate(string frameworkId)
        {
            var Questions = await intermediateUnitOfWork.Entity.FindAll(x => x.FrameworkId == frameworkId);
            if (Questions == null)
                return NotFound("Not Found Any Questions !");
            var Question = Questions.Select( x => new GetQuestionsDTO
            {
                QuestionId = x.QuestionId,
                LevelName = x.LevelName,
                Questions = x.Questions,
                Answers = x.Answers,
                FrameworkName = frameworksUnitOfWork.Entity.Find(x => x.FrameworkId == frameworkId).FrameworkName ?? "",


            }).ToList();

            return Ok(Question);
        }

        [HttpGet("Q_AdvancedLevel/{frameworkId}")]
        public async Task<IActionResult> Q_Advanced(string frameworkId)
        {
            var Questions = await advancedUnitOfWork.Entity.FindAll(x => x.FrameworkId == frameworkId);
            if (Questions == null)
                return NotFound("Not Found Any Questions !");
            var Question = Questions.Select( x => new GetQuestionsDTO
            {
                QuestionId = x.QuestionId,
                LevelName = x.LevelName,
                Questions = x.Questions,
                Answers = x.Answers,
                FrameworkName = frameworksUnitOfWork.Entity.Find(x => x.FrameworkId == frameworkId).FrameworkName ?? "",


            }).ToList();

            return Ok(Question);
        }

        [HttpPost("AddQuestion/{level}/{requestQuestionId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddQuestionFromRequest(string requestQuestionId, string level)
        {
            var questionsInRequest = await requestnUitOfWork.Entity.GetAsync(requestQuestionId);
            if (questionsInRequest == null)
                return NotFound("This Question Not Found");

            //var frameworkid = frameworksUnitOfWork.Entity.Find(x => x.FrameworkName == questionsInRequest.FrameworkName).FrameworkId;


            if (level.Contains("beginner"))
            {
                var Question = new BeginnerLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = questionsInRequest.FrameworkId,
                    Answers = questionsInRequest.Answers,
                    Questions = questionsInRequest.Questions,


                };
                await beginnerUnitOfWork.Entity.AddAsync(Question);
            }
            else if (level.Contains("Intermediate"))
            {
                var Question = new IntermediateLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = questionsInRequest.FrameworkId,
                    Answers = questionsInRequest.Answers,
                    Questions = questionsInRequest.Questions,


                };
                await intermediateUnitOfWork.Entity.AddAsync(Question);


            }
            else if (level.Contains("Advanced"))
            {
                var Question = new AdvancedLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = questionsInRequest.FrameworkId,
                    Answers = questionsInRequest.Answers,
                    Questions = questionsInRequest.Questions,


                };
                await advancedUnitOfWork.Entity.AddAsync(Question);

            }
            else
            {
                return NotFound("This level Not Found");
            }
            advancedUnitOfWork.Save();


            return Ok();
        }


        [HttpPost("AddQuestionFromAdmin/{level}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddQuestionFromAdmin(AddQuestionsDTO dto,string level)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var frameworkid = frameworksUnitOfWork.Entity.Find(x=>x.FrameworkName == dto.FrameworkName);


            if (level.Contains("Beginner"))
            {
                var Question = new BeginnerLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = dto.FrameworkId,
                    Answers = dto.Answers,
                    Questions = dto.Questions,


                };
                await beginnerUnitOfWork.Entity.AddAsync(Question);
            }
            else if (level.Contains("Intermediate"))
            {
                var Question = new IntermediateLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = dto.FrameworkId,
                    Answers = dto.Answers,
                    Questions = dto.Questions,


                };
                await intermediateUnitOfWork.Entity.AddAsync(Question);


            }
            else if (level.Contains("Advanced"))
            {
                var Question = new AdvancedLevel
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    FrameworkId = dto.FrameworkId,
                    Answers = dto.Answers,
                    Questions = dto.Questions,


                };
                await advancedUnitOfWork.Entity.AddAsync(Question);

            }
            else
            {
                return NotFound("This level Not Found");
            }
            advancedUnitOfWork.Save();
            return Ok();



        }

        [HttpPut("UpdateQuestion/{questionId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> UpdateQuestion(UpdateQuestionDTO dto  , string questionId )
        {
            if(await beginnerUnitOfWork.Entity.Any(x=>x.QuestionId == questionId))
            {
                var question = await beginnerUnitOfWork.Entity.GetAsync(questionId);
                question.Answers = dto.Answers ?? question.Answers;
                question.Questions = dto.Questions ?? question.Questions;

                await beginnerUnitOfWork.Entity.UpdateAsync(question);

            }
            else if(await intermediateUnitOfWork.Entity.Any(x => x.QuestionId == questionId))
            {
                var question = await intermediateUnitOfWork.Entity.GetAsync(questionId);
                question.Answers = dto.Answers ?? question.Answers;
                question.Questions = dto.Questions ?? question.Questions;

                await intermediateUnitOfWork.Entity.UpdateAsync(question);

            }
            else if(await advancedUnitOfWork.Entity.Any(x => x.QuestionId == questionId))
            {
                var question = await advancedUnitOfWork.Entity.GetAsync(questionId);
                question.Answers = dto.Answers ?? question.Answers;
                question.Questions = dto.Questions ?? question.Questions;

                await advancedUnitOfWork.Entity.UpdateAsync(question);
            }
            else
            {
                return BadRequest("This ID Of Question Not Found");
            }

            advancedUnitOfWork.Save();
            return Ok();

        }

        [HttpDelete("DeleteQuestion/{questionId}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteQuestion(string questionId)
        {
            if (await beginnerUnitOfWork.Entity.Any(x => x.QuestionId == questionId))
            {
                var question = await beginnerUnitOfWork.Entity.GetAsync(questionId);
                beginnerUnitOfWork.Entity.Delete(question);

            }
            else if (await intermediateUnitOfWork.Entity.Any(x => x.QuestionId == questionId))
            {
                var question = await intermediateUnitOfWork.Entity.GetAsync(questionId);

                 intermediateUnitOfWork.Entity.Delete(question);

            }
            else if (await advancedUnitOfWork.Entity.Any(x => x.QuestionId == questionId))
            {
                var question = await advancedUnitOfWork.Entity.GetAsync(questionId);
                advancedUnitOfWork.Entity.Delete(question);
            }
            else
            {
                return BadRequest("This ID Of Question Not Found");
            }

            advancedUnitOfWork.Save();
            return Ok();

        }









    }
}
