using HotelManagementSystem;
using HotelManagementSystem.Data;
using HotelManagementSystem.Identity;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.Repository;
using HotelManagementSystem.ServiceContract;
using HotelManagementSystem.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelManagementSystem.DTOMapping;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();

//****
builder.Services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
builder.Services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
builder.Services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
builder.Services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IOtpService, OtpService>();
builder.Services.AddTransient<IHotelService, HotelService>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddUserStore<ApplicationUserStore>()
.AddUserManager<ApplicationUserManager>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>()
.AddRoleStore<ApplicationRoleStore>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<ApplicationRoleStore>();
builder.Services.AddScoped<ApplicationUserStore>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Email
var emailSettingsSection = builder.Configuration.GetSection("EmailSettings");
builder.Services.Configure<EmailSettings>(emailSettingsSection);
var emailSetting = emailSettingsSection.Get<EmailSettings>();
//jwt
var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);
var appSetting = appSettingSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSetting.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "abc",
       builder =>
       {
           builder.WithOrigins("http://localhost:4200")
              .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
       });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("abc");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
