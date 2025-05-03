using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Model
{
    public class RequestQuestions
    {
        [Key]
        public string RequestId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public string FrameworkId { get; set; }

        public string Questions { get; set; }

        public string? Answers { get; set; }

        public DateTime? DateRequest { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; }






    }
}
