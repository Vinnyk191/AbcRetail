using AbcRetail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcRetail.Functions.Functions
{
    public class UploadToBlobFunction
    {
        private readonly IBlobStorageService _blob;
        private readonly ILogger<UploadToBlobFunction> _logger;

        public UploadToBlobFunction(IBlobStorageService blob, ILogger<UploadToBlobFunction> logger)
        {
            _blob = blob;
            _logger = logger;
        }

        [FunctionName("UploadToBlob")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "post", Route = "blob/upload")] HttpRequest req)
        {
            if (!req.Form.Files.Any())
                return new BadRequestObjectResult("No file uploaded");

            var file = req.Form.Files[0];
            using var st = file.OpenReadStream();
            // container name "products" used as example
            var uri = await _blob.UploadFileAsync(st, file.FileName, "products");
            _logger.LogInformation("Uploaded blob: {uri}", uri);
            return new OkObjectResult(new { url = uri });
        }
    }
}
