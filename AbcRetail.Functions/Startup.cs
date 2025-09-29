using AbcRetail.AzureDemo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AbcRetail.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbcRetail.Models;

[assembly: FunctionsStartup(typeof(AbcRetail.Functions.Startup))]
namespace AbcRetail.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var config = builder.GetContext().Configuration;
            builder.Services.Configure<AzureStorageOptions>(
            builder.GetContext().Configuration.GetSection("AzureStorage"));

            // Example DI registration
           // builder.Services.Configure<AbcRetail.Models.AzureStorageOptions>(config.GetSection("AzureStorage"));
            builder.Services.AddScoped<ICustomerTableService, CustomerTableService>();
            builder.Services.AddScoped<IProductTableService, ProductTableService>();
            builder.Services.AddScoped<IOrderQueueService, OrderQueueService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddScoped<IFileShareService, FileShareService>();
        }
    }
}
