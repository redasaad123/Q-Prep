using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Test
    {
        [Key]
        public string Q_Id { get; set; }
        public string? Qeuestion { get; set; }
        public string? A_1 { get; set; }
        public string? A_2 { get; set; }
        public string? A_3 { get; set; }
        public string? A_4 { get; set; }
        public string CorrectAnswers { get; set; }
        public string FrameworkId { get; set; }

    }
}
