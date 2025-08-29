using AbcRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbcRetail.Controllers
{
    public class LogsController : Controller
    {
        private readonly IFileShareService _fileShareService;

        public LogsController(IFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }

        // GET: /Logs
        public async Task<IActionResult> Index()
        {
            // Example: fetch logs from Azure Files
            var logs = await _fileShareService.GetLogFilesAsync();
            return View(logs);
        }
    }
}
