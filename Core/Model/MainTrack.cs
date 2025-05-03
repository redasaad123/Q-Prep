using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Model
{
    public class MainTrack
    {
        [Key]
        public string TrackId { get; set; }

        public string? TarckName { get; set; }
        public string? Photo { get; set; }
        public string? description { get; set; }

        [JsonIgnore]
        List<Frameworks>? frameworks { get; set; }


    }
}
