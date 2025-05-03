using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.DTO
{
    public class GetQuestionsDTO
    {
        public string QuestionId { get; set; }

        public string? LevelName { get; set; }
        public string? Questions { get; set; }

        public string? Answers { get; set; }

        public string FrameworkName { get; set; }
    }
}
