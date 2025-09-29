using AbcRetail.AzureDemo.Services;
using AbcRetail.Models;
using AbcRetail.Services; //  Make sure your services namespace is correct
using AbcRetailServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static AbcRetail.Services.ProductTableService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

//Register HttpClient
builder.Services.AddHttpClient();

// Register AzureStorageOption
builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection("AzureStorage"));

//  Register services correctly (interface -> implementation)
builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IFileShareService, FileShareService>();
builder.Services.AddScoped<ICustomerTableService, CustomerTableService>();
builder.Services.AddScoped<IProductTableService, ProductTableService>();
builder.Services.AddScoped<IOrderQueueService, OrderQueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // good practice
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
