using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using System.Threading.Tasks;
using System.Linq;
using ShoppingMall.Web.Infrastructure.Common;

namespace ShoppingMall.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private const int PageSize = 10;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CitySortParm"] = sortOrder == "City" ? "city_desc" : "City";
            ViewData["CountrySortParm"] = sortOrder == "Country" ? "country_desc" : "Country";
            ViewData["CurrentFilter"] = searchString;

            var customers = await _customerService.GetAllCustomersAsync();
            var query = customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.CompanyName.Contains(searchString)
                                       || s.ContactName.Contains(searchString)
                                       || s.City.Contains(searchString)
                                       || s.Country.Contains(searchString));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(s => s.CompanyName),
                "City" => query.OrderBy(s => s.City),
                "city_desc" => query.OrderByDescending(s => s.City),
                "Country" => query.OrderBy(s => s.Country),
                "country_desc" => query.OrderByDescending(s => s.Country),
                _ => query.OrderBy(s => s.CompanyName),
            };

            int pageSize = PageSize;
            var model = PaginatedList<CustomerDTO>.Create(query, pageNumber ?? 1, pageSize);
            return View(model);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerDTO customerDto)
        {
            if (ModelState.IsValid)
            {
                await _customerService.CreateCustomerAsync(customerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(customerDto);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerDTO customerDto)
        {
            if (id != customerDto.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(customerDto);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 