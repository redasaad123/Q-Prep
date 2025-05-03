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
    public class Frameworks
    {
        [Key]
        public string FrameworkId { get; set; }

        public string? FrameworkName { get; set; }
        public string? Photo { get; set; }
        public string? description { get; set; }

        [ForeignKey("MainTrackId")]
        public MainTrack? MainTrack { get; set; }
        public string? MainTrackId {  get; set; }
        [JsonIgnore]     

        List<BeginnerLevel>? BeginnerLevel { get; set; }
        List<IntermediateLevel>? IntermediateLevel { get; set; }
        List<AdvancedLevel>? AdvancedLevel { get; set; }


    }
}
