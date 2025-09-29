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
    public class WriteToFileShareFunction
    {
        private readonly IFileShareService _fileService;
        private readonly ILogger<WriteToFileShareFunction> _logger;

        public WriteToFileShareFunction(IFileShareService fileService, ILogger<WriteToFileShareFunction> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [FunctionName("WriteToFileShare")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "post", Route = "files/write")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(body)) return new BadRequestObjectResult("Empty body");

            // path passed by query or default
            var path = req.Query["path"].ToString();
            if (string.IsNullOrWhiteSpace(path))
                path = $"logs/{System.DateTime.UtcNow:yyyyMMdd}/log-{System.Guid.NewGuid()}.txt";

            using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));
            await _fileService.UploadLogFileAsync(path, ms);

            _logger.LogInformation("Wrote file to share at {path}", path);
            return new OkObjectResult(new { path });
        }
    }
}
