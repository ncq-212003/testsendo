using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace Erp.Social.Sendo.Services
{
    public class CategorySevice
    {
        private static HttpClient httpClient = new HttpClient();
        public static async Task<object> GetCategory(string accsesstoken)
        {
            string urlCate = "https://open.sendo.vn/api/partner/category/1663";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);

            HttpResponseMessage response = await httpClient.GetAsync(urlCate);
            var responseBody = await response.Content.ReadAsStringAsync();


            
            return JsonSerializer.Deserialize<object>(responseBody);

        }
    }
}
