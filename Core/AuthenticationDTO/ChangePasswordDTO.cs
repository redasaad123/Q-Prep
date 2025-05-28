using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AuthenticationDTO
{
    public class ChangePasswordDTO : BasePasswordDTO
    {
        public string OldPassword { get; set; }

    }
}
