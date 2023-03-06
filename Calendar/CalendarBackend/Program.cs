using System.Text;
using CalendarBackend.Db;
using CalendarBackend.Identity.Entities;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<CalendarDevContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services
    .AddIdentity<CalendarUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 8;
        })
    .AddEntityFrameworkStores<CalendarDevContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(auth =>
		{
			auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
				ValidAudience = "url",
				ValidIssuer = "url",
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret key")),
                ValidateIssuerSigningKey = true
            };
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthenticationService, JwtAuthenticationService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
