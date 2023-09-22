using Erp.Social.Sendo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Social.Sendo.Controllers
{
    [Route("api/v1/[controller]/[Action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrder(
             int did,
             int orderstatus,
             string orderdatefrom,
             string orderdateto,
             string orderstatusdatefrom,
             string orderstatusdateto,
             int page_size = 10
          )
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var listOrder = await OrderService.getListOrder(
                page_size,
                orderstatus,
                orderdatefrom,
                orderdateto,
                orderstatusdatefrom,
                orderstatusdateto,
                accessToken
                );
            return Ok(listOrder);
        }


        [HttpGet]
        [Route("{did}")]
        public async Task<IActionResult> TotalByStatusOrder(
            int did,
            int orderstatus,
            string orderdatefrom,
            string orderdateto,
            string orderstatusdatefrom,
            string orderstatusdateto,
            int page_size = 10
  )
        {
            string accessToken = await AccessTokenServices.GetTokenByDid(_configuration, did.ToString());

            if (accessToken is null)
            {
                return Ok(new { message = "AccessToken NotFound !" });
            }

            var listdetailOrder = await OrderService.CaculateTotalByStatusOrder(
                page_size,
                orderstatus,
                orderdatefrom,
                orderdateto,
                orderstatusdatefrom,
                orderstatusdateto,
                accessToken
                );
            return Ok(listdetailOrder);
        }
    }
}
