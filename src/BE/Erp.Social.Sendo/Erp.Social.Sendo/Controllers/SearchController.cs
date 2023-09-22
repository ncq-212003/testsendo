using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> FindBySKU(int did, string sku)
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var detailsku = await SearchSevice.GetSKU(accessToken ,sku);
            return Ok(detailsku);
        }

        [HttpGet]
        public async Task<IActionResult> FindByID(int did, string id)
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var detailID = await SearchSevice.GetID(accessToken, id);
            return Ok(detailID);
        }

    }
}
