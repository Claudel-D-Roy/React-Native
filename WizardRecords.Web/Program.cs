using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WizardRecords.Api;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Repositories;
using WizardRecords.Api.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WizardRecords.Api.Interfaces;
using WizardRecords.Api.Data;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<StripeOptions>(options =>
    builder.Configuration.GetSection("StripeOptions").Bind(options)
);

// Database context
builder.Services.AddDbContext<WizRecDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity services
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<WizRecDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


StripeConfiguration.ApiKey = builder.Configuration["StripeOptions:SecretKey"];
// CORS
builder.Services.AddCors(options => {
	options.AddPolicy(name: "AllowReactApp",
		builder => {
			builder.WithOrigins("http://localhost:3000")
				.AllowAnyHeader()
				.AllowAnyMethod()
                .AllowCredentials();
	});
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
