using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> listCategory(int did)
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var listCategory = await CategorySevice.GetCategory(accessToken);
            return Ok(listCategory);
        }

    }
}
