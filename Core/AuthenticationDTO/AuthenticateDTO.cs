using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.AuthenticationDTO
{
    public class AuthenticateDTO
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Message { get; set; }

        public List<string>? Roles { get; set; }

        public string? Photo { get; set; }


        public string? Token { get; set; }


        public DateTime Expireson { get; set; }


        [JsonIgnore]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
