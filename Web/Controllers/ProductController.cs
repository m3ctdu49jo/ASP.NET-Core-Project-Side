using Microsoft.AspNetCore.Mvc;
using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Category(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return View("Index", products);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return RedirectToAction(nameof(Index));

            var products = await _productService.SearchProductsAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View("Index", products);
        }

        public async Task<IActionResult> Featured(int count = 8)
        {
            var products = await _productService.GetFeaturedProductsAsync(count);
            ViewBag.Title = "精選商品";
            return View("Index", products);
        }

        public async Task<IActionResult> NewArrivals(int count = 8)
        {
            var products = await _productService.GetNewArrivalsAsync(count);
            ViewBag.Title = "新品上架";
            return View("Index", products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);

            var createdProduct = await _productService.CreateProductAsync(productDto);
            return RedirectToAction(nameof(Details), new { id = createdProduct.ProductID });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);

            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
            if (updatedProduct == null)
                return NotFound();

            return RedirectToAction(nameof(Details), new { id = updatedProduct.ProductID });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
} 