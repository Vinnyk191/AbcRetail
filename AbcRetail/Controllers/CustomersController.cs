﻿using Microsoft.AspNetCore.Mvc;
using AbcRetail.Models;
using AbcRetail.Services;
using System;
using System.Threading.Tasks;

namespace AbcRetail.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerTableService _customerService;

        public CustomersController(ICustomerTableService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var customer = await _customerService.GetCustomerAsync("CUSTOMERS", id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                customer.PartitionKey = "CUSTOMERS";
                customer.RowKey = Guid.NewGuid().ToString();
                //customer.CreatedAt = DateTime.UtcNow;

                await _customerService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var customer = await _customerService.GetCustomerAsync("CUSTOMERS", id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerEntity customer)
        {
            if (id != customer.RowKey)
                return NotFound();

            if (ModelState.IsValid)
            {
                customer.PartitionKey = "CUSTOMERS";
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var customer = await _customerService.GetCustomerAsync("CUSTOMERS", id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _customerService.DeleteCustomerAsync("CUSTOMERS", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
