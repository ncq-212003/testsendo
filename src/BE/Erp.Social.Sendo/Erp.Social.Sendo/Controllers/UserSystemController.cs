using Erp.Social.Sendo.Model;
using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class UserSystemController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserSystemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserSystem userSystem)
        {
            try
            {
                bool checkusername = UserSystemSevice.IsUserExist(_configuration, userSystem.UserName);
                if (checkusername)
                {
                    return Ok(new { message = "UserName is Duplicate" });
                }
                int checkregister = UserSystemSevice.Resgister(_configuration, userSystem);
                if (checkregister > 0)
                {
                    return Ok(new { message = "Susscess" });
                }
                else
                {
                    return Ok(new { message = "Failed" });
                }
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                return Ok(new { message = "An error occurred" });
            }

        }
        [HttpPost]

        public async Task<IActionResult> Login(string UserName ,string PassWord)
        {
            try
            {
                bool iLogin = UserSystemSevice.Login(_configuration, UserName, PassWord);
                if (!iLogin) return Unauthorized();
                var authenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,UserName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), // dinh danh 1 ma thong bao
            };
                var token = UserSystemSevice.GetJWTToken(_configuration, authenClaims);

                return Ok(
                        new
                        {
                            message = "Success",
                            token = new JwtSecurityTokenHandler().WriteToken(token), //in ra 1 chuoi token
                            expiration = token.ValidTo //thong bao thoi gian het han cua token
                        }
                    );
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                return Ok(new { message = "An error occurred" });
                throw;
            }

        }
    }
}
