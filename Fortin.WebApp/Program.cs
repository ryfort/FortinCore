using Fortin.Common.Configuration;
using Fortin.Proxy;
using Fortin.WebApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection; // Add this using directive
using System.Net.Http; // Add this using directive

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//to add configuration settings, need to install NuGet package Microsoft.Extensions.Options.ConfigurationExtensions
builder.Services.Configure<ProxyBaseUrls>(builder.Configuration.GetSection("ProxyBaseUrls"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<FortinAPIClient>();
builder.Services.AddScoped<AdventureWorksClient>();

await builder.Build().RunAsync();