using Erp.Social.Sendo.Model;
using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccessTokenController(IConfiguration configuration) {
            _configuration = configuration;
        }
        [HttpPost]

        public async Task<IActionResult> getToken(string shop_key, string secret_key)
        {
            var getToken = await AccessTokenServices.GetToken(shop_key, secret_key);
            return Ok(getToken);
        }

        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] AccessToken accessToken)
        {
            try
            {
                int AddTokensendo = await AccessTokenServices.AddOrUpdateInDatabase(_configuration, accessToken);
                if (AddTokensendo > 0)
                {
                    return Ok(new { message = "Success" });
                }
                else
                {
                    return Ok(new { message = "Fail" });
                }
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }
    }
}
