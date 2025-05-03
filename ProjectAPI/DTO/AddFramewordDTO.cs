using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.DTO
{
    public class AddFramewordDTO
    {

        public string? MainTrackId { get; set; }
        public string? FramworkName { get; set; }
        public IFormFile? Photo { get; set; }
        public string? description { get; set; }

    }
}
