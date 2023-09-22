using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace Erp.Social.Sendo.Services
{
    public class ProductService
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<object> GetListProduct(
             int pagesize,
             //string productname,
             string datefrom,
             string dateto,
             //string status_one,
            string accsesstoken)
        {
            try
            {
                string requestUrl = "https://open.sendo.vn/api/partner/product/search";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);
                httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                var requestData = new
                {
                    page_size = pagesize,
                    product_name = "",
                    date_from = datefrom,
                    date_to = dateto,
                    status = (string)null,
                    token = ""
                };
                string jsonBody = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send the request
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                // Handle the response
                return JsonSerializer.Deserialize<object>(responseBody);
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }

        public static async Task<object> GetDetailProduct(
             int pagesize,
             string productname,
             string datefrom,
             string dateto,
            //string status_one,
            string accsesstoken)
        {
            try
            {
                string requestUrl = "https://open.sendo.vn/api/partner/product/search";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);
                httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                var requestData = new
                {
                    page_size = pagesize,
                    product_name = productname,
                    date_from = datefrom,
                    date_to = dateto,
                    status = (string)null,
                    token = ""
                };
                string jsonBody = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send the request
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                // Handle the response
                return JsonSerializer.Deserialize<object>(responseBody);
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }

        }
    }
}
