using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WMS.API.Middleware;
using WMS.Application;
using WMS.Application.Common.Mappings;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Application.Validators.Auth;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Data.Seeders;
using WMS.Infrastructure.Repositories;
using WMS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<WmsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsAssembly("WMS.Infrastructure")
    )
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});


var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();


// --- DI


//--- CommonRepository
builder.Services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));

//---Auth Service
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//---Product-Management
builder.Services.AddScoped<IProductService, ProductService>();

//--AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

//---Vehicle-Management
builder.Services.AddScoped<IVehicleService,VehicleService>();

//--State-Management
builder.Services.AddScoped<IStateService, StateService>();

//--City-Management
builder.Services.AddScoped<ICityService, CityService>();

//--Cart-Management
builder.Services.AddScoped<ICartService,CartService>();

//-User-Address
builder.Services.AddScoped<IUserAddressService,UserAddressService>();

//-Driver-Management 

builder.Services.AddScoped<IDriverService,DriverService>();

//--Order 
builder.Services.AddScoped<IOrderService,OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WMS API",
        Version = "v1"  
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

    await db.Database.MigrateAsync();
    await AdminSeeder.SeedAsync(db);
}

app.Run();