using System.Reflection;
using System.Text;
using ContactHubApi.Context;
using ContactHubApi.Repositories.Addresses;
using ContactHubApi.Repositories.Contacts;
using ContactHubApi.Repositories.Users;
using ContactHubApi.Services.Addresses;
using ContactHubApi.Services.Contacts;
using ContactHubApi.Services.Tokens;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContactHubContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("ContactHubDb")));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Add security definition to swagger
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

    // Add header documentation in swagger
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User Contact Information Management System",
        Description = "The most effective API for managing user contact information.",
        Contact = new OpenApiContact
        {
            Name = "Contact Hub",
            Url = new Uri("https://contacthubapp.azurewebsites.net/")
        },
    });

    // Feed generated xml api docs to swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Initialize the configuration object
IConfiguration configuration = builder.Configuration;

// Configure our services
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

void ConfigureServices(IServiceCollection services)
{
    // Add CORS policy
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Configure Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Register Services 
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IContactService, ContactService>();
    services.AddScoped<IAddressService, AddressService>();
    services.AddScoped<ITokenService, TokenService>();

    // Register Repos
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IContactRepository, ContactRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();

    // Add Authentication
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });

    services.AddMvc();
}