using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AuthenticationDTO
{
    public class VerifyCodeDTO : BasePasswordDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
