using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectAPI.DTO
{
    public class GetRequestsDTO
    {
        public string RequestId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string Photo { get; set; }

        public string Questions { get; set; }

        public string? Answers { get; set; }

        public DateTime? DateRequest { get; set; }



    }
}
