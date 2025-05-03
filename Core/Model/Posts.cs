using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Posts
    {
        [Key]
        public string PostId { get; set; }
        public string? header { get; set; }
        public string? Text { get; set; }
        public List<string>? Images { get; set; } 
        public string? TypeOfBody { get; set; }

        public int? likes { get; set; } = 0;
        public DateTime? postDate { get; set; }
 

        public string? UserId { get; set; }

        public string? groupId { get; set; }

    }
}
