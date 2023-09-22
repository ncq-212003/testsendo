using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> ProductList(
           int did,
           string date_from,
           string date_to,
           //string product_name,
           //string status,
           int page_size = 10
          )
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var productList = await ProductService.GetListProduct(
                page_size,
                date_from,
                date_to,
                //status,
                accessToken);
            return Ok(productList);
        }


        [HttpGet]
        public async Task<IActionResult> GetDtailProduct(
            int did,
            string date_from,
            string date_to,
            string product_name,
            int page_size = 10
  )
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var productList = await ProductService.GetDetailProduct(
                page_size,
                product_name,
                date_from,
                date_to,
                accessToken);
            return Ok(productList);
        }


    }
}
