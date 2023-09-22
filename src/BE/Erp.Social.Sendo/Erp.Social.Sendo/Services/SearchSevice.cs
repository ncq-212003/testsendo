using System.Net.Http.Headers;
using System.Text.Json;

namespace Erp.Social.Sendo.Services
{
    public class SearchSevice
    {
        private static HttpClient httpClient = new HttpClient();
        public static async Task<object> GetSKU(string accsesstoken ,string sku)
        {
            try
            {
                string url = $"https://open.sendo.vn/api/partner/product?sku={sku}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);

                HttpResponseMessage response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(responseBody);
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }

        public static async Task<object> GetID(string accsesstoken, string id)
        {
            try
            {
                string url = $"https://open.sendo.vn/api/partner/product?id={id}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);

                HttpResponseMessage response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
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
