using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Recent
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; }

        public string MainTrackId { get; set; }
        [JsonIgnore]
        public MainTrack? MainTrack { get; set; }

        public DateTime DateTime { get; set; }
    }
}
