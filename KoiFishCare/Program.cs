using KoiFishCare.Interfaces;
using KoiFishCare.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using KoiFishCare.Data;
using KoiFishCare.Models;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using KoiFishCare.service;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using KoiFishCare.Models.Enum;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using KoiFishCare.service.VnpayService.Services;
// using KoiFishCare.Data;
// using KoiFishCare.Models;
// using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
//add to remove the $id in the return list
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
        });

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "KoiFishCareService", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//format date for booking
builder.Services.AddSwaggerGen(c =>
{
    // Mapping DateOnly to a string with date format
    c.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString("2024-10-15")
    });

    // Mapping TimeOnly to a string with time format (e.g., "07:00")
    c.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",  // you can also use custom format string if needed
        Example = new OpenApiString("07:00")
    });
});

// Show enum string
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "API", Version = "v2" });
    c.MapType<BookingStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = new List<IOpenApiAny>
        {
            new OpenApiString("Pending"),
            new OpenApiString("Confirmed"),
            new OpenApiString("Scheduled"),
            new OpenApiString("Ongoing"),
            new OpenApiString("Completed"),
            new OpenApiString("Received Money"),
            new OpenApiString("Succeeded"),
            new OpenApiString("Refunded"),
            new OpenApiString("Cancelled")
        }
    });
});

// Add CORS policy for your Vite frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();  // If you need to send cookies or Authorization header
        });
});

// Configure Entity Framework and Identity
builder.Services.AddDbContext<KoiFishVeterinaryServiceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
})
  .AddEntityFrameworkStores<KoiFishVeterinaryServiceContext>()
  .AddDefaultTokenProviders()
  .AddUserStore<UserStore<User, IdentityRole, KoiFishVeterinaryServiceContext>>()
  .AddRoleStore<RoleStore<IdentityRole, KoiFishVeterinaryServiceContext>>();

// Add other services (repositories, etc.)
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFishOrPoolRepository, FishOrPoolRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
builder.Services.AddScoped<ItokenService, TokenService>();
builder.Services.AddScoped<IVetRepository, VetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IVetSlotRepository, VetSlotRepository>();
builder.Services.AddScoped<IDistanceRepository, DistanceRepository>();
builder.Services.AddScoped<IPrescriptionRecordRepository, PrescriptionRecordRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IBookingRecordRepository,BookingRecordRepository>();

builder.Services.AddAuthorization(options =>
{

});

builder.Services.AddControllers(options =>
{
    //add authorization Policy
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for the specified policy before handling requests


app.UseHttpsRedirection();

app.UseStaticFiles();  // Optional, if serving static files

app.UseRouting();
app.UseCors("AllowViteFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });


app.Run();
