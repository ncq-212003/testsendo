using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Erp.Social.Sendo.Services
{
    public class OrderService
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<double> CaculateTotalByStatusOrder(
          int pagesize,
          int orderStatus,
          string orderdatefrom,
          string orderdateto,
          string orderstatusdatefrom,
          string orderstatusdateto,
          string accsesstoken)
        {
            try
            {
                string requestUrl = "https://open.sendo.vn/api/partner/salesorder/search";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);
                httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                var requestData = new
                {
                    page_size = pagesize,
                    order_status = orderStatus,
                    order_date_from = Datetimeformat(orderdatefrom),
                    order_date_to = Datetimeformat(orderdateto),
                    order_status_date_from = Datetimeformat(orderstatusdatefrom),
                    order_status_date_to = Datetimeformat(orderstatusdateto),
                    token = (string)null
                };
                string jsonBody = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Send the request
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                // Handle the response
                double total = 0;
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    var root = doc.RootElement;// lấy phần tử cấp cao nhát bọc ngoài json
                    if (root.TryGetProperty("result", out var resultProperty) && resultProperty.TryGetProperty("data", out var dataProperty))
                    {
                        foreach (var data in dataProperty.EnumerateArray())
                        {
                            if (data.TryGetProperty("sales_order", out var salesorderProperty))
                            {
                                if (salesorderProperty.TryGetProperty("sub_total", out var subtotalProperty))
                                {
                                    if (subtotalProperty.TryGetDouble(out double orderTotalAmount))
                                    {
                                        total += orderTotalAmount;
                                    }
                                }
                            }
                        }
                    }
                }
                return total;
            }
            catch (Exception ex) {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }


        public static async Task<object> getListOrder(
            int pagesize,
            int orderStatus,
            string orderdatefrom,
            string orderdateto,
            string orderstatusdatefrom,
            string orderstatusdateto,
            string accsesstoken)
        {
            try
            {
                string requestUrl = "https://open.sendo.vn/api/partner/salesorder/search";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accsesstoken);
                httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                var requestData = new
                {
                    page_size = pagesize,
                    order_status = orderStatus,
                    order_date_from = Datetimeformat(orderdatefrom),
                    order_date_to = Datetimeformat(orderdateto),
                    order_status_date_from = Datetimeformat(orderstatusdatefrom),
                    order_status_date_to = Datetimeformat(orderstatusdateto),
                    token = (string)null
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

        public static string Datetimeformat(string inputdate)
        {
            string inputDate = inputdate;
            DateTime date = DateTime.ParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("yyyy/MM/dd");
            return formattedDate;
        }


    }
}
