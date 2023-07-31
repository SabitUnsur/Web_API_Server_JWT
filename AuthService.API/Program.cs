 using AuthServer.Core.Configuration;
using AuthServer.Core.Entity;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repossitories;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Configurations;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>),typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
//AddDefaultTokenProvider => �ifre s�f�rlama gibi olaylarda token� bu sayede �retiyoruz.

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //Bu �emalar ka� �yelik sistemi oldu�u ile ilgilidir. �rnek Bayi giri�i , kUllan�c� giri�i gibi 2 ayr� �yelik sist. varsa 2 �ema vard�r.
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //Jwtden gelen ve kendi default �emam� ba�lad�m

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        //birinci audience bizim direkt ana projemiz oldu�u i�in onu kontrol edersek bizden istek yap�p yapamayaca��n� buluruz,
        //di�er apilere bakmam�za gerek yok burada dolay�s�yla [0] dedik.
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
//Class ve appsetings.jsonu ba�lad�k, DI Containera bir nesne �rne�i ge�tik. 


builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

builder.Services.AddControllers();

builder.Services.AddEntityFrameworkNpgsql()
  .AddDbContext<AppDbContext>()
  .BuildServiceProvider();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
