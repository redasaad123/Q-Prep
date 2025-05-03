using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AuthenticationDTO
{
    public class BasePasswordDTO
    {
        public string NewPassword { get; set; }

        public string ConfirmedNewPassword { get; set; }
    }
}
