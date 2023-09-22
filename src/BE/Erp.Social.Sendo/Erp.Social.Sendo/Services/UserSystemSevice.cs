using Erp.Social.Sendo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Erp.Social.Sendo.Services
{
    public class UserSystemSevice
    {
        public static JwtSecurityToken GetJWTToken(IConfiguration _configuration, List<Claim> authClaim)
        {
            try
            {
                // phần payload cua JWT
                SymmetricSecurityKey sysmmec = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])); //tao chu ky cho ma thong bao    
                JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:ValidAudience"],// dia chi nha cung cap ma thong bao ,giúp ma thong bao d
                        audience: _configuration["Jwt:ValidIssuer"],//doi tuong ma ma thong bao cap
                        expires: DateTime.UtcNow.AddMinutes(120),// thoi diem token het han
                        claims: authClaim, // danh claim
                        signingCredentials: new SigningCredentials(sysmmec, SecurityAlgorithms.HmacSha256));// tao chu ky cho ma thong bao
                return token;
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw; // lay duoc ngoai le sau khi dang nhap 
            }
        }

        public static bool Login(IConfiguration _configuration, string username, string password)
        {
            try
            {
                string coonectionString = _configuration.GetConnectionString("SqlConection");
                using (SqlConnection connect = new SqlConnection(coonectionString))
                {
                    connect.Open();
                    string sqlquery = "SELECT COUNT(*) FROM UserSystem WHERE UserName = @username AND Password = @password";
                    using (SqlCommand command = new SqlCommand(sqlquery, connect))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        // dpc va lay du lieu 
                        int usercount = (int)command.ExecuteScalar();
                        return usercount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }
        public static int Resgister(IConfiguration _configuration, [FromBody] UserSystem userSystem)
        {
            try
            {
                string connectstirng = _configuration.GetConnectionString("SqlConection");
                using (SqlConnection connect = new SqlConnection(connectstirng))
                {
                    connect.Open();
                    string insertLogin = "INSERT INTO UserSystem (DId, UserName, Password, RefreshToken , CreateAt ,CreateBy,LastModifyAt ,LastModifyBy,Flag)  VALUES ( @Did,@username ,@password , @refrestoken ,@createat ,@createby ,@Lastmodifyat,@lastmodifyBy ,@flag)";
                    using (SqlCommand command = new SqlCommand(insertLogin, connect))
                    {
                        command.Parameters.AddWithValue("@Did", userSystem.DId);
                        command.Parameters.AddWithValue("@username", userSystem.UserName);
                        command.Parameters.AddWithValue("@password", userSystem.Password);

                        if (userSystem.RefreshToken == null)
                        {
                            command.Parameters.AddWithValue("@refrestoken", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@refrestoken", userSystem.RefreshToken);
                        }
                        command.Parameters.AddWithValue("@createat", userSystem.CreateAt);
                        command.Parameters.AddWithValue("@createby", userSystem.CreateBy);
                        command.Parameters.AddWithValue("@Lastmodifyat", userSystem.LastModifyAt);
                        command.Parameters.AddWithValue("@lastmodifyBy", userSystem.LastModifyBy);
                        command.Parameters.AddWithValue("@flag", userSystem.Flag);
                        // thuc hien chay lenh 
                        int insertrespon = command.ExecuteNonQuery();
                        return insertrespon;

                    }

                }
            }
            catch (Exception ex)
            {
                MyLogger.LogError(ex.Message);
                throw;
            }
        }

    // kiem tra du lieu nguoi dung co trong du lieu khong 
    public static bool IsUserExist(IConfiguration configuration, string username)
    {
        try
        {
            string ConnectionString = configuration.GetConnectionString("SqlConection");
            using (SqlConnection conection = new SqlConnection(ConnectionString))
            {
                conection.Open();
                string sqlquery = "SELECT COUNT(*) FROM UserSystem WHERE UserName = @username";
                using (SqlCommand command = new SqlCommand(sqlquery, conection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int countname = (int)command.ExecuteScalar();
                    return countname > 0;
                }

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
