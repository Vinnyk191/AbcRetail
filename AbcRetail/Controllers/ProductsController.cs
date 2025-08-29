using Microsoft.AspNetCore.Mvc;
using AbcRetail.Models;
using AbcRetail.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using AbcRetail.Models.ViewModels;

namespace AbcRetail.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductTableService _productService;
        private readonly IBlobStorageService _blobService;
        private readonly IFileShareService _fileService;

        public ProductsController(
            IProductTableService productService,
            IBlobStorageService blobService,
            IFileShareService fileService)
        {
            _productService = productService;
            _blobService = blobService;
            _fileService = fileService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModels vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var product = new ProductEntity
            {
                RowKey = Guid.NewGuid().ToString(),
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Stock = vm.Stock,
                PartitionKey = "PRODUCTS"
            };

            // Upload image to Blob Storage
            if (vm.Image != null && vm.Image.Length > 0)
            {
                using var stream = vm.Image.OpenReadStream();
                product.ImageUrl = await _blobService.UploadFileAsync(stream, vm.Image.FileName, "products");
            }

            await _productService.AddProductAsync(product);

            // Optionally log to Azure File Share
            using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes($"Product created: {product.Name}"));
            await _fileService.UploadLogFileAsync($"products/{DateTime.UtcNow:yyyyMMdd}/prod-{product.RowKey}.log", mem);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEntity product)
        {
            if (!ModelState.IsValid) return View(product);

            await _productService.UpdateProductAsync(product);

            // Log update
            using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes($"Product updated: {product.Name}"));
            await _fileService.UploadLogFileAsync($"products/{DateTime.UtcNow:yyyyMMdd}/prod-{product.RowKey}.log", mem);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            await _productService.DeleteProductAsync(id);

            // Log deletion
            using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes($"Product deleted: {id}"));
            await _fileService.UploadLogFileAsync($"products/{DateTime.UtcNow:yyyyMMdd}/prod-{id}.log", mem);

            return RedirectToAction(nameof(Index));
        }
    }
}
