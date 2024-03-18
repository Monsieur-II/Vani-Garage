using System.Text;
using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Vani.Domain.Models;
using Vani.Infras;
using Vani.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Vani.Api.Extensions;
using Vani.Services.Cache;
using Vani.Services.Cars;
using Vani.Services.Makes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    })
);

builder.Services.AddControllers();

// hangfire
builder.Services.AddHangfire(options =>
    options.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSQLiteStorage(builder.Configuration.GetConnectionString("hangfire")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IMakeService, MakeService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VaniGarage API", Version = "v1" });
    
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

// Caching
builder.Services.AddSingleton<ICacheService, CacheService>();

// dbcontext
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddGarageDbContext(builder.Configuration);


builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();
    

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["JWTConfig:ValidAudience"],
            ValidIssuer = builder.Configuration["JWTConfig:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:TokenKey"]!))
        };
    });
builder.Services.AddAuthorization();

// JWTConfig / EmailConfig
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection(nameof(JWTConfig)));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(nameof(EmailConfig)));

var hangfireUser = builder.Configuration["HangfireConfig:username"];
var hangfirePass = builder.Configuration["HangfireConfig:password"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(webapp => webapp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
    }));
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            Pass = hangfirePass,
            User = hangfireUser
        }
    }
});
app.MapHangfireDashboard();
app.MapControllers();
app.Run();
