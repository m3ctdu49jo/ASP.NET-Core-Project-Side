using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.Filters;
using ShoppingMall.Web.ViewModels;

namespace ShoppingMall.Web.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
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
        [ServiceFilter(typeof(LoginAuthenticatedRedirectFilter))]
        public IActionResult Login()
        {
            // 不要快取頁面，每次都要重新向伺服器請求最新內容。
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View("Login");
        }

        [HttpPost]
        [ActionName("Index")]
        [ServiceFilter(typeof(LoginAuthenticatedRedirectFilter))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginCheck([Bind("UserName, Password")] UserDTO userDTO)
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
                    if (!identify)
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

            TempData["LoginSuccessfully"] = identify;

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
            if (TempData["LoginSuccessfully"] is null)
            {
                // 如果沒有登入成功的 TempData，則重定向到 Login 頁面
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel userModel)
        {
            bool success = false;
            if (!RegisterInfoCheck(userModel.UserInfo, userModel.ConfirmPassword))
            {
                return View(userModel);
            }

            try
            {
                bool isExist = await _userService.IsExistUserNameAsync(userModel.UserInfo.UserName);
                if (isExist)
                    ViewBag.ErrorMsg = "帳號已存在，請重新輸入";
                else
                    success = true;

                if (ModelState.IsValid && success)
                {
                    userModel.UserInfo.CreatDate = DateTime.Now;
                    await _userService.AddUserAsync(userModel.UserInfo).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            ViewBag.ErrorMsg = string.Concat(userModel.UserInfo.UserName, " ", "註冊失敗，請稍後再試");
                            success = false;
                            // throw new Exception(task.Exception?.Message);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (success)
            {
                TempData["RegiserSuccessfully"] = success;
                return RedirectToAction(nameof(RegisterSuccess), "Login");
            }

            return View(userModel);
        }
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                // 如果沒有登入成功的資訊，則重定向到 Login 頁面
                return RedirectToAction("Index", "Login");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // 清除Session
            HttpContext.Session.Remove("UserSession");

            // 驗證使用者資訊已被清除
            TempData["LogoutSuccessfully"] = HttpContext.Session.GetString("UserSession") == null;

            // 不直接 return View()，否則會導致頁面仍保留使用者資訊
            return RedirectToAction(nameof(LogoutMsg));
        }

        public IActionResult LogoutMsg()
        {
            return View();
        }

        private bool RegisterInfoCheck(UserDTO userDTO, string? confirmPassword = null)
        {
            bool isValid = true;            
            if (string.IsNullOrEmpty(userDTO.UserName) || string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.Name))
            {
                ViewBag.ErrorMsg = "請填寫*必填欄位";
                isValid = false;
            }
            if (userDTO.Password != confirmPassword)
            {
                ViewBag.ErrorMsg = string.Concat(ViewBag.ErrorMsg ?? "", "密碼與確認密碼不一致");
                isValid = false;
            }
            return isValid;
        }
    }
}
