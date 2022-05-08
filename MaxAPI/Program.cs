using MaxAPI.Contexts;
using MaxAPI.Services;
using MaxAPI.Services.Interfaces;
using MaxAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MainContext>(opt => opt.UseInMemoryDatabase("Main"));
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });
}

builder.Services
    .AddTransient<IAuthenticationService, AuthenticationService>()
    .AddTransient<IUserService, UserService>()
    .AddTransient<IRegistrationService, RegistrationService>()
    .AddTransient<IPatientService, PatientService>()
    .AddTransient<IDoctorService, DoctorService>()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var signingKey = "A31kSjhFw1-pJupaTMd-pYdZmkSwAC7v5JPVa1wfcYHSKnpdZmPUyi94i4fYxu5uZpNi8ugaWuJAK9Zr79SjtA";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Base64Url.Decode(signingKey)),
            ValidAudience = "max",
            ValidIssuer = "max",
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
        };
    });


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
