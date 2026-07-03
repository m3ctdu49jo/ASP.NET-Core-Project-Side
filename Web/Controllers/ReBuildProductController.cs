using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReBuildProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;

        public ReBuildProductController(IProductService productService, IOrderService orderService, IShoppingCartService shoppingCartService, IMapper mapper)
        {
            _productService = productService;
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDTO>(product));
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return Ok(await _productService.GetAllProductsAsync());

            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetFeaturedProducts([FromQuery] int count = 8)
        {
            var products = await _productService.GetFeaturedProductsAsync(count);
            return Ok(products);
        }

        [HttpGet("new-arrivals")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetNewArrivals([FromQuery] int count = 8)
        {
            var products = await _productService.GetNewArrivalsAsync(count);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductID }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedProduct = await _productService.UpdateProductAsync(id, _mapper.Map<Product>(productDto));
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("shopping-cart")]
        public async Task<ActionResult> AddToShoppingCart(int productId, int count)
        {
            // TODO: Implement shopping cart logic
            return Ok(new { message = "Item added to cart", productId, count });
        }
    }
}
