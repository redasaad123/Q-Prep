
using Core.AuthenticationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Core.Interfaces
{
    public interface IAuthentication
    {
        Task<AuthenticateDTO> RegisterAsync(RegisterDTO dto );
        Task<AuthenticateDTO> LoginAsync(LogInDTo dto );
        Task<AuthenticateDTO> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<string> AddRoleToUser(RoleToUserDTO dto);
        Task<string> AddRole(RoleDTO dto);

    }
}
