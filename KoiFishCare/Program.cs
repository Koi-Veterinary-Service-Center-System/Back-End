using KoiFishCare.Interfaces;
using KoiFishCare.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KoiFishCare.Data;
using KoiFishCare.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<KoiFishVeterinaryServiceContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);

// Add Identity 
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

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

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

