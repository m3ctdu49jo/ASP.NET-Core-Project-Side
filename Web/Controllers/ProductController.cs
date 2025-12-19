using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductCollectionService _productCollectionService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IOrderService orderService, IShoppingCartService shoppingCartService, IProductCollectionService productCollectionService, IMapper mapper)
        {
            _productService = productService;
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _productCollectionService = productCollectionService;
            _mapper = mapper;
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

            ProductViewModel viewModel = new ProductViewModel
            {
                Product = _mapper.Map<ProductDTO>(product),
                IsInCollection = await _productCollectionService.Generic.GetByIdAsync(id, User.Identity.Name) != null
            };

            return View(viewModel);
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

            var updatedProduct = await _productService.UpdateProductAsync(id, _mapper.Map<Product>(productDto));
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

        [HttpPost]
        public IActionResult AddToShoppingCar(string productId, int count)
        {
            return View();
        }

        
        
        [HttpPost]
        [ActionName("Collections")]
        [EnableRateLimiting("fixed-per-ip")]
        public async Task<IActionResult> AddCollections([FromBody]ProductCollection collection)
        {
            try
            {
                var item = await _productCollectionService.Generic.GetByIdAsync(collection.ProductID, User.Identity.Name);
                if (item != null)
                    return BadRequest("此商品已在收藏清單中");

                await _productCollectionService.Generic.AddAsync(new ProductCollection
                {
                    ProductID = collection.ProductID,
                    UserName = User.Identity.Name
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
        [HttpDelete]
        [ActionName("Collections")]
        [EnableRateLimiting("fixed-per-ip")]
        public async Task<IActionResult> RemoveCollections([FromBody]ProductCollection collection)
        {
            try
            {   
                var item = await _productCollectionService.Generic.GetByIdAsync(collection.ProductID, User.Identity.Name);
                if (item == null)
                    return BadRequest("取消失敗，此商品不在收藏清單中");

                await _productCollectionService.Generic.DeleteAsync(collection.ProductID, User.Identity.Name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
        
    }
} 