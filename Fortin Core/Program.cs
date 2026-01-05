using Fortin.API;
using Fortin.API.Configuration;
using Fortin.API.Filters;
using Fortin.Common.Configuration;
using Fortin.Infrastructure;
using Fortin.Infrastructure.Implementation;
using Fortin.Infrastructure.Interface;
using Fortin.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.Configure<ConnectionStrings>(connectionStrings);
builder.Services.Configure<ProxyBaseUrls>(builder.Configuration.GetSection("ProxyBaseUrls"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
// Add services to the container.
builder.Services.AddRepositoryServices();
builder.Services.AddProxyServices();
builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FortinCommon")));

builder.Services.AddDbContext<AdventureWorksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogFilter>();
    //options.Filters.Add(new ServiceFilterAttribute(typeof(LogFilter)));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHttpClient();
//builder.Services.AddHttpClient("GitHub", httpClient =>
//{
//    httpClient.BaseAddress = new Uri("https://api.github.com/");
//    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
//    httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
//});

//builder.Services.AddHttpClient<HttpProxy>();

var allowedCorsOrigins = builder.Configuration.GetSection("Cors:AllowedCorsOrigins").Get<string[]>()?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins(allowedCorsOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader()
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseCustomMiddleware();

app.MapControllers();

app.Run();
