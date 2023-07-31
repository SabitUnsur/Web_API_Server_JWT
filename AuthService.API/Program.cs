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
//AddDefaultTokenProvider => þifre sýfýrlama gibi olaylarda tokený bu sayede üretiyoruz.

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //Bu Þemalar kaç üyelik sistemi olduðu ile ilgilidir. Örnek Bayi giriþi , kUllanýcý giriþi gibi 2 ayrý üyelik sist. varsa 2 þema vardýr.
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //Jwtden gelen ve kendi default þemamý baðladým

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        //birinci audience bizim direkt ana projemiz olduðu için onu kontrol edersek bizden istek yapýp yapamayacaðýný buluruz,
        //diðer apilere bakmamýza gerek yok burada dolayýsýyla [0] dedik.
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
//Class ve appsetings.jsonu baðladýk, DI Containera bir nesne örneði geçtik. 


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
