using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{

    public abstract class Levels
    {
        [Key]
        public string QuestionId {  get; set; }
        
        public string? Questions { get; set; }

        public string? Answers { get; set; }

        [ForeignKey("FrameworkId")]
        public Frameworks Frameworks { get; set; }

        public string FrameworkId { get; set; }

    }
}
