using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Filters;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticatedFilter))]
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IProductService productService, IOrderService orderService, IShoppingCartService shoppingCartService, IMapper mapper)
        {
            _productService = productService;
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart([FromBody]ShoppingCartDTO req)
        {
            try
            {
                var shoppingItem = await _shoppingCartService.GetByIdAndUserNameAsync(req.Product.ProductID, User.Identity.Name);
                var product = await _productService.GetProductByIdAsync(req.Product.ProductID);

                var (isValid, errorMessage) = await VaildProductStock(req, product, shoppingItem);
                if (!isValid)
                    return BadRequest(errorMessage);

                await UpdateShoppingCart(shoppingItem, product, req.PurchCount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        public async Task<IActionResult> Checkout()
        {
            var cartItems = await _shoppingCartService.GetAllIncludeProductByUserNameAsync(User.Identity.Name);

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                await _shoppingCartService.DeleteAsync(productId, User.Identity.Name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
                
            var cartItems = await _shoppingCartService.GetAllIncludeProductByUserNameAsync(User.Identity.Name);
            return PartialView("_CartItemsPartial", cartItems);
        }
        private async Task<(bool, string)> VaildProductStock(ShoppingCartDTO req, Product product, ShoppingCart shoppingItem)
        {
            string errorMessage = string.Empty;
            if (product == null)
                errorMessage = "Not exist product";
            var productStock = product?.UnitsInStock ?? 0;
            if (req.PurchCount <= 0)
                errorMessage = "商品目前缺貨";
            if (req.PurchCount >= productStock || req.PurchCount + (shoppingItem != null ? shoppingItem.PurchCount : 0) > productStock)
                errorMessage = "購買數量超過上限";
            return (string.IsNullOrEmpty(errorMessage), errorMessage);
        }
        private async Task UpdateShoppingCart(ShoppingCart shoppingItem, Product product, int purchCount)
        {
            if (shoppingItem != null)
            {
                shoppingItem.PurchCount += purchCount;
                await _shoppingCartService.Generic.UpdateAsync(shoppingItem);
            }
            else
            {
                shoppingItem = new ShoppingCart()
                {
                    UserName = User.Identity.Name,
                    ProductID = product.ProductID,
                    Product = product,
                    PurchCount = purchCount
                };
                await _shoppingCartService.Generic.AddAsync(shoppingItem);
            }
        }
    }
} 