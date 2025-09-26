using Fortin.API;
using Fortin.API.Configuration;
using Fortin.Common.Configuration;
using Fortin.Infrastructure;
using Fortin.Infrastructure.Implementation;
using Fortin.Infrastructure.Interface;
using Fortin.Proxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.Configure<ConnectionStrings>(connectionStrings);
builder.Services.Configure<ProxyBaseUrls>(builder.Configuration.GetSection("ProxyBaseUrls"));
// Add services to the container.
builder.Services.AddRepositoryServices();
builder.Services.AddProxyServices();
builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FortinCommon")));

builder.Services.AddDbContext<AdventureWorksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks")));

builder.Services.AddControllers();
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("https://localhost:7027")
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

app.MapControllers();

app.Run();
