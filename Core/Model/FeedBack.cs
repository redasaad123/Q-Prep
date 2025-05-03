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
    public class FeedBack
    {
        [Key]
        public string FeedbackId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public AppUser? AppUser { get; set; }
    }
}
