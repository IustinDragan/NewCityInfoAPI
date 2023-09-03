using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewCityInfo.API;
using NewCityInfo.API.DbContexts;
using NewCityInfo.API.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
     .MinimumLevel.Debug()
     .WriteTo.Console()
     .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
     .CreateLogger();

     var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
     options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
     
     //cod pentru adaugarea mesajelor ajutatoare in documentatie (pentru a functiona ok trebuie mers la proprietati -> build -> generate xml document...
     // setupAction =>
     // {
     //      var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
     //      var xmlCommensFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
     //      
     //      setupAction.IncludeXmlComments(xmlCommensFullPath);
     // });
     
     //cod pentru adaugareabutonului Autoraze in Swagger
     // setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
     // {
     //      {
     //           new OpenApiSecurityScheme
     //           {
     //                Reference = new OpenApiReference
     //                {
     //                     Type = ReferenceType.SecurityScheme,
     //                     Id = "CityInfoApiBearerAuth"
     //                }
     //           }, new List<string>()
     //      }
     // });
     );


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService();>
#endif

builder.Services.AddSingleton<CitiesDataStore>();

//varianta implementate de Alex:
//builder.Services.AddDbContext<CityInfoContext>(dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["Database:ConnectionString"]));

builder.Services.AddDbContext<CityInfoContext>(
     dbContextOptions => dbContextOptions.UseSqlite(
          builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer")
     .AddJwtBearer(options =>
     {
          options.TokenValidationParameters = new()
          {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration["Authentication:Issuer"],
               ValidAudience = builder.Configuration["Authentication:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
          };
     });

builder.Services.AddAuthorization(options =>
{
     options.AddPolicy("MustBeFromAntwerp", policy =>
     {
          policy.RequireAuthenticatedUser();
          policy.RequireClaim("city", "Antwerp");
     });
});

builder.Services.AddApiVersioning(setupAction =>
{
     setupAction.AssumeDefaultVersionWhenUnspecified = true;
     setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
     setupAction.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
     endpoints.MapControllers();
});

//app.MapControllers();

// app.Run(async (context) =>
// {
//     await context.Response.WriteAsync("Hello World!");
// });

app.Run();