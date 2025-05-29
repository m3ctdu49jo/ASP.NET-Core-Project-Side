using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;

namespace ShoppingMall.Web.Controllers
{
    public class UsersController : Controller
    {

        private readonly IGenericService<ShoppingMall.Web.Models.User, UserDTO> _userService;

        public UsersController(IGenericService<ShoppingMall.Web.Models.User, UserDTO> userService)
        {
            _userService = userService;
        }

        // GET: UsersController
        public ActionResult<UserDTO> Index(int id)
        {
            var users = _userService.GetByIdAsync(id);

            return View(users);
        }

    }
}
