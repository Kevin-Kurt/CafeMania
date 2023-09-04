using System.Text;
using System.Text.RegularExpressions;
using PTI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PTI.Controllers;
using PTI.Data;
using PTI;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using PTI.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

string security_key = builder.Configuration.GetSection("TokenAuthentication")["SecretKey"];
var symetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(security_key));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = String.Empty;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddRoleManager<RoleManager<IdentityRole>>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

string connection = "";
if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration.GetConnectionString("localMySqlConnection");

}
else
{
    connection = builder.Configuration.GetConnectionString("productionMySqlConnection");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connection,
        ServerVersion.Create(new Version(5, 7, 9), ServerType.MySql),
        sqlServerOptions => sqlServerOptions.CommandTimeout(600))
    .EnableSensitiveDataLogging(), ServiceLifetime.Transient);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.IssuerSigningKey = symetricSecurityKey;
        options.TokenValidationParameters.ValidAudience = builder.Configuration.GetSection("TokenAuthentication")["AudienceProd"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration.GetSection("TokenAuthentication")["IssuerProd"];
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
        options.TokenValidationParameters.RequireExpirationTime = true;
        options.TokenValidationParameters.ValidateLifetime = true;
    });


builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );

});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("DashBoard",
        new CacheProfile()
        {
            Duration = 86400 // 1 dia em segundos
        }
    );
}).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddHangfire(op => op.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<UserService, UserService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MalbyApi v1"));
app.UseRouting();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;

            await context.Response.WriteAsync(new ErrorDto()
            {
                Code = 500,
                Message = ex.Message

            }.ToString(), Encoding.UTF8);
        }
    });
});



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();



public class ErrorDto
{
    public int Code { get; set; }
    public string Message { get; set; }

    // other fields

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

