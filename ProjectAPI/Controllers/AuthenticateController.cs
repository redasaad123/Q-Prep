using Core.AuthenticationDTO;
using Core.Interfaces;
using Core.Model;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly PasswordHasher<AppUser> passwordHasher;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AppUser> appUserUnitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;
        private readonly IAuthentication authentication;
        private readonly JwtSettings jwtSettings;
        

        public AuthenticateController(PasswordHasher<AppUser> passwordHasher,IOptions<JwtSettings> options, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager
            , IUnitOfWork<AppUser> appUserUnitOfWork, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting , IAuthentication authentication )
        {
            this.passwordHasher = passwordHasher;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.appUserUnitOfWork = appUserUnitOfWork;
            this.hosting = hosting;
            this.authentication = authentication;
            jwtSettings = options.Value;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register( RegisterDTO dto)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authentication.RegisterAsync(dto);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LogInDTo dto)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authentication.LoginAsync(dto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

                setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);


            return Ok(result);

        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return NotFound("Email Is NotFound");


            return Ok(user.Id);
        }


        [HttpPost("ChangePassword/{id}")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> ChangePassword([FromForm] BasePasswordDTO dto , string id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            string idUser;

            if (id is null)
                idUser = userManager.GetUserId(HttpContext.User);
            else idUser = id.ToString();

            var user = await userManager.FindByIdAsync(idUser);
            if (user == null) 
                return NotFound("User Is NotFound");

            var hashPassword = passwordHasher.HashPassword(user, dto.NewPassword + "Abcd123#");
            user.PasswordHash = hashPassword;
            await userManager.UpdateAsync(user);
            appUserUnitOfWork.Save();
            return Ok("The Password Is Changed");


        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var result = await authentication.RefreshTokenAsync(refreshToken);

            if(!result.IsAuthenticated)
                return BadRequest(result);

            setRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);

            return Ok(result);

        }


        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken(RevokeTokenDTO dto)
        {
            var token = dto.Token ?? Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token Is Empty");

            var result  = await authentication.RevokeTokenAsync(token);

            if (!result) 
                return BadRequest("Token Is Invalid");


            return Ok(result);
        }



        private void setRefreshTokenInCookie(string refreshToken, DateTime expire)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expire.ToLocalTime(),
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);

        }




    }
}
