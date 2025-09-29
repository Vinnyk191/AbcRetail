
using Microsoft.AspNetCore.Mvc;
using AbcRetail.Models;
using AbcRetail.Services;
using System.Threading.Tasks;

/*namespace AbcRetail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderQueueService _orders;
        private readonly CustomerTableService _customers;
        private readonly ProductTableService _products;

        public OrdersController(
            OrderQueueService orders,
            CustomerTableService customers,
            ProductTableService products)
        {
            _orders = orders;
            _customers = customers;
            _products = products;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orders.GetAllOrdersAsync();
            return View(orders);
        }

        // GET: /Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _customers.GetAllCustomersAsync();
            ViewBag.Products = await _products.GetAllProductsAsync();
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderEntity order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customers.GetAllCustomersAsync();
                ViewBag.Products = await _products.GetAllProductsAsync();
                return View(order);
            }

            // Map OrderEntity → OrderMessage for queue
            var message = new OrderMessage
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };

            await _orders.EnqueueOrderAsync(message);

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
*/



namespace AbcRetail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderQueueService _orders;
        private readonly ICustomerTableService _customers;
        private readonly IProductTableService _products;

        public OrdersController(
            IOrderQueueService orders,
            ICustomerTableService customers,
            IProductTableService products)
        {
            _orders = orders;
            _customers = customers;
            _products = products;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            var orderMessages = await _orders.GetAllOrdersAsync();
            return View(orderMessages);
        }

        // GET: /Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _customers.GetAllCustomersAsync();
            ViewBag.Products = await _products.GetAllProductsAsync();
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderMessage orderMessage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customers.GetAllCustomersAsync();
                ViewBag.Products = await _products.GetAllProductsAsync();
                return View(orderMessage);
            }

            // Assign unique OrderId
            orderMessage.OrderId = Guid.NewGuid().ToString();
            orderMessage.OrderDate = DateTime.UtcNow;

            // Enqueue order into Azure Queue
           // await _orders.EnqueueOrderAsync(message);

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
