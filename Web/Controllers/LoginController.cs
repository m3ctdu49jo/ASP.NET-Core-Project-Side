using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Services;
using ShoppingMall.Models;

namespace ShoppingMall.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;    
        }

        [ActionName("Index")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        // GET: LoginController
        public async Task<ActionResult> Login([Bind("username, password")] UserDTO userDTO)
        {
            var username = userDTO.UserName;
            var password = userDTO.Password;
            if(ModelState.IsValid == false)
            {
                ViewBag.ErrorMsg = "帳號或密碼錯誤";
                return View(userDTO);
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMsg = "帳號與密碼不得為空";
                return View();
            }

            await _userService.GetByUserNameAndPasswordAsync(username, password).ContinueWith(task =>
            {
                var user = task.Result;
                if (task.Result != null)
                {
                    _httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }));
                    HttpContext.Session.SetString("UserSession", task.Result.UserName);
                }
                else
                {
                    ViewBag.ErrorMsg = "帳號或密碼錯誤";
                }
            });
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {         
            bool success = false;       
            if(!RegisterCheck(userDTO))
            {
                ViewBag.ErrorMsg = "請填寫*必填欄位";
                return View(userDTO);
            }
            
            try
            {
                bool isExist = await _userService.IsExistUserNameAsync(userDTO.UserName);
                if (isExist)
                    ViewBag.ErrorMsg = "帳號已存在，請重新輸入";
                else
                    success = true;

                if (ModelState.IsValid && success)
                {
                    userDTO.CreatDate = DateTime.Now;
                    await _userService.Generic.AddAsync(userDTO).ContinueWith(task =>
                    {
                        if (!task.IsCompleted)
                        {
                            ViewBag.ErrorMsg = string.Concat(userDTO.UserName, " ", "註冊失敗，請稍後再試");
                            success = false;
                        }
                    });
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            if (success)
            {
                return RedirectToAction(nameof(RegisterSuccess), "Login");
            }
            
            return View(userDTO);
        }

        public IActionResult RegisterSuccess()
        {
            ViewBag["SuccessMsg"] = "註冊成功，請登入";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserSession");
            _httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity());
            return RedirectToAction("Index", "Home");
        }

        public bool RegisterCheck(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.UserName) || string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.Name))
            {
                return false;
            }
            return true;
        }
    }
}
