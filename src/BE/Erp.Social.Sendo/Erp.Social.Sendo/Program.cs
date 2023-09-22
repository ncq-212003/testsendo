using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

// Khởi tạo cấu hình Serilog
var logPath = "logs"; // Đường dẫn tới thư mục logs
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{logPath}/log.txt", rollingInterval: RollingInterval.Day) // Ghi log vào tệp tin
    .WriteTo.Console() // Ghi log lên Console
    .CreateLogger();

// logging started
Log.Information("Program started...\n");

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager _Configuration = builder.Configuration;
// Add authencation JWT
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// xac dinh cau hinh va dang ky he thong xac thuc cho he thong lay theo gia tri mac dinh cua JWTBeare
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  //dat gia trị ma dinh cho he thong 
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //dat mac dinh cho qua trinh xac thuc cua JwtBeare
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(otp =>
{
    otp.SaveToken = true; // luu token cho phep su dung sau này
    otp.RequireHttpsMetadata = false; // cho phep su dung http 
    otp.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = _Configuration["Jwt:ValidAudience"],
        ValidIssuer = _Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Secret"]))
    };
});

// Add services to the container.

builder.Services.AddControllers();

// create CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseCors(); 

app.MapControllers();

app.Run();
