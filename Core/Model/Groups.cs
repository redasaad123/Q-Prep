using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Groups
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }

        public string? NumberOfMembers { get; set; }

    }
}
