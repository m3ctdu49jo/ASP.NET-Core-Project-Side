using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
            return View("Login");
        }

        [HttpPost]
        [ActionName("Index")]
        // GET: LoginController
        public async Task<ActionResult> Login([Bind("UserName, Password")] UserDTO userDTO)
        {
            bool identify = true;
            var username = userDTO.UserName;
            var password = userDTO.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMsg = "帳號與密碼不得為空";
                identify = false;
            }
            if (ModelState.IsValid && identify)
            {
                try
                {
                    identify = await IdentityCheckAndSetUserInfoAsync(username, password);                    
                    if(!identify)
                        ViewBag.ErrorMsg = string.Concat(userDTO.UserName, " ", "登入失敗，請稍後再試");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                if (!identify)
                    ViewBag.ErrorMsg = "帳號或密碼錯誤";
            }
            
                if (!identify)
                    return View("Login", userDTO);

            return RedirectToAction(nameof(LoginSuccess), "Login");
        }

        private async Task<bool> IdentityCheckAndSetUserInfoAsync(string username, string password)
        {
            bool success = false;
            var user = await _userService.GetByUserNameAndPasswordAsync(username, password);

            if (user != null)
            {
                await SetAuthenticationUserInfoAsync(user);
                success = true;
            }
            return success;
        }
        
        private async Task SetAuthenticationUserInfoAsync(UserDTO user)
        {
            
                HttpContext.Session.Clear();
                // 登入成功，將使用者資訊存入Session
                HttpContext.Session.SetString("UserSession", user.UserName);

                // 設定 Cookie 驗證
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // 記住登入
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };
                // _httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity());
                // AddIdentity 只暫時加身份，不會讓使用者真正「登入」；要讓登入狀態持久，必須用 SignInAsync（Cookie 驗證）。
                // AddIdentity 只是在記憶體中暫時加一個身份，不會持久化，下次請求就消失。
                // SignInAsync 會把 ClaimsIdentity 寫入 Cookie，讓瀏覽器帶著 Cookie，之後每個請求都能自動還原 User.Identity。
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );
        }

        public IActionResult LoginSuccess()
        {
            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.Identity?.Name))
                ViewBag.LoginMsg = "登入成功!!" + User.Identity?.Name;
            else
                ViewBag.LoginMsg = "登入失敗!!";
            return View();
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
                    await _userService.AddUserAsync(userDTO).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            ViewBag.ErrorMsg = string.Concat(userDTO.UserName, " ", "註冊失敗，請稍後再試");
                            success = false;
                            // throw new Exception(task.Exception?.Message);
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
            ViewBag.SuccessMsg = "註冊成功，請登入";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // 清除Session
            HttpContext.Session.Remove("UserSession");
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
