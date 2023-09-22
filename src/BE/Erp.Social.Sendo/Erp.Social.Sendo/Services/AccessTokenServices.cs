
using Erp.Social.Sendo.Model;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Erp.Social.Sendo.Services
{
    public class AccessTokenServices
    {
        public static HttpClient HttpClient = new HttpClient();

        public static async Task<object> GetToken(string shop_key_token, string secret_key_token)
        {
            var requestUrl = "https://open.sendo.vn/login";

            // Create the request body
            var requestBody = new
            {
                shop_key = shop_key_token,
                secret_key = secret_key_token
            };
            string jsonBody = JsonSerializer.Serialize(requestBody);

            // Set the Content-Type header and UTF-8 encoding
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync(requestUrl, content);

            // Read the response
            var responseBody = await response.Content.ReadAsStringAsync();

            // Handle the response
            return JsonSerializer.Deserialize<object>(responseBody);
        }

        public static async Task<int> AddOrUpdateInDatabase( IConfiguration configuration , AccessToken accessToken)
        {
            try {
                string connectionString = configuration.GetConnectionString("SqlConection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string querysql = "SELECT COUNT(*) FROM AccessToken WHERE did = @did";
                    using (SqlCommand command = new SqlCommand(querysql, connection))
                    {
                        command.Parameters.AddWithValue("@did", accessToken.did);
                        int existingRecordCount = (int)await command.ExecuteScalarAsync();

                        if (existingRecordCount > 0)
                        {
                            // Perform update
                            string updateQuery = "UPDATE AccessToken SET token = @token, expires = @expires, success = @success, error = @error, status_code = @status_code, params_one = @params_one, track_id = @track_id WHERE did = @did";

                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {

                                updateCommand.Parameters.AddWithValue("@token", accessToken.token);
                                updateCommand.Parameters.AddWithValue("@expires", accessToken.expires);
                                updateCommand.Parameters.AddWithValue("@success", accessToken.success);
                                updateCommand.Parameters.AddWithValue("@error", accessToken.error);
                                updateCommand.Parameters.AddWithValue("@status_code", accessToken.status_code);
                                updateCommand.Parameters.AddWithValue("@params_one", accessToken.params_one);
                                updateCommand.Parameters.AddWithValue("@track_id", accessToken.track_id);
                                updateCommand.Parameters.AddWithValue("@did", accessToken.did);

                                int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                                return rowsAffected;
                            }
                        }
                        else
                        {
                            string Insert = "INSERT INTO AccessToken(did,token,expires,success,error,status_code,params_one,track_id) VALUES (@did,@token,@expires,@success,@error,@status_code,@params_one,@track_id)";
                            using (SqlCommand InsertCommand = new SqlCommand(Insert, connection))
                            {
                                InsertCommand.Parameters.AddWithValue("@did", accessToken.did);
                                InsertCommand.Parameters.AddWithValue("@token", accessToken.token);
                                InsertCommand.Parameters.AddWithValue("@expires", accessToken.expires);
                                InsertCommand.Parameters.AddWithValue("@success", accessToken.success);
                                InsertCommand.Parameters.AddWithValue("@error", accessToken.error);
                                InsertCommand.Parameters.AddWithValue("@status_code", accessToken.status_code);
                                InsertCommand.Parameters.AddWithValue("@params_one", accessToken.params_one);
                                InsertCommand.Parameters.AddWithValue("@track_id", accessToken.track_id);

                                // tra ve so luong dong duoc insert
                                int rowEffect = await InsertCommand.ExecuteNonQueryAsync();
                                return rowEffect;
                            }
                        }
                    }

                }

            }
            catch(Exception ex) {
                MyLogger.LogError(ex.Message);
                    throw;
            }
        }

         
        public static async Task<string> GetTokenByDid (IConfiguration configuration , string did)
        {
            string connectionString = configuration.GetConnectionString("SqlConection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT token FROM AccessToken WHERE did = @did";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@did", did);

                    object result = await command.ExecuteScalarAsync();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



    }
}