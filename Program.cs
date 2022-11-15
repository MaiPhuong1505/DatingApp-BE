using System.Text;
using DatingApp.API.Data;
using DatingApp.API.Data.Repositories;
using DatingApp.API.Data.Seed;
using DatingApp.API.Profiles;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var connectionString = builder.Configuration.GetConnectionString("Default"); 

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// services.AddCors(o =>
//     o.AddPolicy("CorsPolicy", builder =>
//         builder.WithOrigins("http://localhost:4200")
//             .AllowAnyHeader()
//             .AllowAnyMethod()));
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<iTokenService, TokenService>();
services.AddScoped<IMemberService, MemberService>();
services.AddAutoMapper(typeof(UserMapperProfile).Assembly);
 services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "https://testkhanh.ddns.net",  "*", "http://192.168.1.2","http://pbl4.ddns.net:4200","http://116.110.199.1:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(origin => true) // allow any origin
                            .AllowCredentials(); // allow credentials
                    });
            });
            
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]))
        };
    });


var app = builder.Build();
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

try
{
    var context = serviceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
    Seed.SeedUsers(context);
}
catch (System.Exception ex)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Migration Failed");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
