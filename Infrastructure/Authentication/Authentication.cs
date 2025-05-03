using Core.AuthenticationDTO;
using Core.Interfaces;
using Core.Model;
using Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;





namespace Infrastructure.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        private readonly JwtSettings jwtSettings;

        public Authentication(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, IOptions<JwtSettings> options)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

            jwtSettings = options.Value;
        }

        private async Task<JwtSecurityToken> CreateToken(AppUser user)
        {
            var userRole = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);


            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (var role in userRole)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.key));

            var signing = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken
                (
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    expires: DateTime.UtcNow.AddHours(jwtSettings.DurationInDays),
                    claims: claims,
                    signingCredentials: signing

                );

            return token;


        }

        public async Task<AuthenticateDTO> RegisterAsync(RegisterDTO dto)
        {
            if (await userManager.FindByEmailAsync(dto.Email) != null)
                return new AuthenticateDTO { Message = "Email Is  already Registed" };

            var user = new AppUser
            {
                Email = dto.Email,
                Name = dto.Name,
                UserName = new MailAddress(dto.Email).User,
                Photo = "user-blue-gradient_78370-4692.jpg",
            };

            var result = await userManager.CreateAsync(user, dto.Password + "Abcd123#");

            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                    errors += $"{error.Description} , ";
                await userManager.DeleteAsync(user);

                return new AuthenticateDTO { Message = errors };

            }
            var Claim = new Claim("User", "User");
            await userManager.AddClaimAsync(user, Claim);

            var roleIsExists = await roleManager.RoleExistsAsync("User");
            if (roleIsExists)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
                await userManager.AddToRoleAsync(user, "User");
            }

            var jwtSecurtyToken = await CreateToken(user);
            return new AuthenticateDTO
            {
                Name = dto.Name,
                Email = dto.Email,
                Expireson = jwtSecurtyToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Users" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurtyToken),
                Message = "This Email Is Created",
            };


        }

        public async Task<AuthenticateDTO> LoginAsync(LogInDTo dto)
        {
            var Authenticate = new AuthenticateDTO();

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password + "Abcd123#"))
            {
                Authenticate.Message = "Email Or Password InCorrect";
                return Authenticate;
            }

            var jwtSecurtyToken = await CreateToken(user);
            var roles = await userManager.GetRolesAsync(user);

            Authenticate.Email = dto.Email;
            Authenticate.IsAuthenticated = true;
            Authenticate.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurtyToken);
            Authenticate.Expireson = jwtSecurtyToken.ValidTo;
            Authenticate.Message = "Login Successfully";
            Authenticate.Roles = roles.ToList();
            Authenticate.Name = user.Name;
            Authenticate.Photo = user.Photo;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshTokens = user.RefreshTokens.FirstOrDefault(x => x.IsActive);
                Authenticate.RefreshToken = activeRefreshTokens.Token;
                Authenticate.RefreshTokenExpiration = activeRefreshTokens.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();

                Authenticate.RefreshToken = refreshToken.Token;
                Authenticate.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);

            }


            return Authenticate;

        }


        public async Task<AuthenticateDTO> RefreshTokenAsync(string token)
        {
            var Authenticate = new AuthenticateDTO();

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                Authenticate.Message = "Invaild Token";
                return Authenticate;
            }

            var refreshToken = user.RefreshTokens.Single(x=>x.Token == token);

            if (!refreshToken.IsActive)
            {
                Authenticate.Message = "InActive Token";
                return Authenticate;
            }

            refreshToken.RevokeOn = DateTime.UtcNow;
            var newrefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newrefreshToken);
            await userManager.UpdateAsync(user);

            var roles = await userManager.GetRolesAsync(user);
            var jwtToken = await CreateToken(user);
            Authenticate.IsAuthenticated = true;
            Authenticate.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            Authenticate.Email = user.Email;
            Authenticate.Roles = roles.ToList();
            Authenticate.RefreshToken = newrefreshToken.Token;
            Authenticate.RefreshTokenExpiration = newrefreshToken.ExpiresOn;


            return Authenticate;

        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var Authenticate = new AuthenticateDTO();

            var user = await userManager.Users.SingleOrDefaultAsync(x=>x.RefreshTokens.Any(t=>t.Token == token));

            if(user == null) 
                return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if(!refreshToken.IsActive)
                return false;

            refreshToken.RevokeOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;


        }

        


        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var genetator = new RNGCryptoServiceProvider();
            genetator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreateOn = DateTime.UtcNow,

            };
        }


        public async Task<string> AddRoleToUser(RoleToUserDTO dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return "Invalid User Id";
            if (!await roleManager.RoleExistsAsync(dto.RoleName))
                return "Invalid Role ";
            if (await userManager.IsInRoleAsync(user, dto.RoleName))
                return "User Already Assigned To This Role";
            var result = await userManager.AddToRoleAsync(user, dto.RoleName);
            if (result.Succeeded)
                return string.Empty;
            return "SomeThing went wrong";


        }
        public async Task<string> AddRole(RoleDTO dto)
        {
            if (await roleManager.RoleExistsAsync(dto.RoleName))
                return "The Role Is Already Existing";

            await roleManager.CreateAsync(new IdentityRole(dto.RoleName));

            return string.Empty;
        }

    }
}
