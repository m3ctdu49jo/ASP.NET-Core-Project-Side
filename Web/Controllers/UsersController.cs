using Microsoft.AspNetCore.Mvc;
using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Services;

namespace ShoppingMall.Controllers
{
    public class UsersController : Controller
    {

        private readonly IGenericService<ShoppingMall.Models.User, UserDTO> _userService;

        public UsersController(IGenericService<ShoppingMall.Models.User, UserDTO> userService)
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
