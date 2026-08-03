using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Filters;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.Utils;
using ShoppingMall.Web.ViewModels;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingMall.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticatedFilter))]
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductCollectionService _productCollectionService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ShoppingCartController(
            IProductService productService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IShoppingCartService shoppingCartService,
            IProductCollectionService productCollectionService,
            IUserService userService,
            IMapper mapper,
            IConfiguration config
        )
        {
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _shoppingCartService = shoppingCartService;
            _productCollectionService = productCollectionService;
            _userService = userService;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart([FromBody] ShoppingCartDTO req)
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

        public async Task<IActionResult> CheckBill()
        {
            var cartItems = await _shoppingCartService.GetAllIncludeProductByUserNameAsync(User.Identity.Name);
            return View(cartItems);
        }

        public async Task<IActionResult> BillShipInfo()
        {
            ShipInfoViewModel model = new ShipInfoViewModel();
            GetBaseShipInfoViewModel(ref model);

            var cartItems = await _shoppingCartService.GetAllIncludeProductByUserNameAsync(User.Identity.Name);
            model.CartItems = cartItems.ToList();

            return View(model);
        }
        [HttpPost]
        [ActionName("BillShipInfo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillShipInfoCheck(ShipInfoViewModel model)
        {
            ShoppingCarOrderViewModel m = new ShoppingCarOrderViewModel();

            GetBaseShipInfoViewModel(ref model);

            try
            {
                var cartItems = await _shoppingCartService.GetAllIncludeProductByUserNameAsync(User.Identity.Name);
                model.CartItems = cartItems.ToList();
                if (model.Ship_equal_user)
                {
                    var userInfo = await _userService.GetByIdAndUserNameAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value), User.Identity.Name);
                    if (userInfo == null || userInfo.City == null || userInfo.Address == null || userInfo.Phone == null)
                    {
                        model.ResultMsg = "訂購人會員地址資料不完整，請手動填寫寄送資訊";
                        ModelState.Clear();
                        return View(nameof(BillShipInfo), model);
                    }
                    else
                    {
                        m.Shipping.City = userInfo.City;
                        m.Shipping.Address = userInfo.Address;
                        m.Shipping.ContactName = userInfo.UserName;
                        m.Shipping.Phone = userInfo.Phone;
                    }
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View(nameof(BillShipInfo), model);
                    }
                    m.Shipping = model.Shipping;
                }

                string orderNum = string.Concat(CommUtil.RandomStringInA_Z(2), CommUtil.RandomStringIn0_9(8));

                var user = await _userService.GetByIdAndUserNameAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value), User.Identity.Name);
                await _shoppingCartService.Checkout(user, orderNum, m.Shipping);
                TempData["orderNum"] = orderNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(Successful));
        }

        private ShipInfoViewModel GetBaseShipInfoViewModel(ref ShipInfoViewModel m)
        {
            var cities = ConfigUtil.GetTaiwanCitisSection(_config);
            var selectListItems = cities.Select(city => new SelectListItem { Text = city.CityName, Value = city.CityName }).ToList();
            m.Cities = selectListItems;

            return m;
        }
        public async Task<IActionResult> Successful()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartDTO req)
        {
            try
            {
                var shoppingItem = await _shoppingCartService.GetByIdAndUserNameAsync(req.Product.ProductID, User.Identity.Name);
                var product = await _productService.GetProductByIdAsync(req.Product.ProductID);

                var diffNum = req.PurchCount - shoppingItem.PurchCount;
                req.PurchCount = diffNum;
                var (isValid, errorMessage) = await VaildProductStock(req, product, shoppingItem);
                if (!isValid)
                    return BadRequest(errorMessage);

                await UpdateShoppingCart(shoppingItem, product, req.PurchCount);
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
            var productStock = product?.UnitsInStock ?? 0;

            if (product == null)
                errorMessage = "Not exist product";
            else if (productStock <= 0)
                errorMessage = "商品目前缺貨";
            else if (req.PurchCount + (shoppingItem != null ? shoppingItem.PurchCount : 0) <= 0)
                errorMessage = "最小購買數量為1";
            else if (req.PurchCount >= productStock || req.PurchCount + (shoppingItem != null ? shoppingItem.PurchCount : 0) > productStock)
                errorMessage = "購買數量超過上限";
            else if (req.PurchCount == 0 && shoppingItem.PurchCount == 1)
                errorMessage = "已達到最小購買數量上限，請確認購買數量";
            else if (req.PurchCount == 0 && shoppingItem.PurchCount == product.UnitsInStock)
                errorMessage = "已達到最大購買數量上限，請確認購買數量";
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