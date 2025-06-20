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
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.Utils;
using System.Text.Json;

namespace ShoppingMall.Web.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        public LoginController(IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration config)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _config = config;
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
            bool identified = true;
            var username = userDTO.UserName;
            var password = userDTO.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMsg = "帳號與密碼不得為空";
                identified = false;
            }
            if (ModelState.IsValid && identified)
            {
                try
                {
                    identified = await IdentityCheckAndSetUserInfoAsync(username, password);
                    if (!identified)
                        ViewBag.ErrorMsg = string.Concat(userDTO.UserName, " ", "登入失敗，請稍後再試");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                if (!identified)
                    ViewBag.ErrorMsg = "帳號或密碼錯誤";
            }

            if (!identified)
                return View("Login", userDTO);

            TempData["LoginSuccessfully"] = identified;

            return RedirectToAction(nameof(LoginSuccess), "Login");
        }

        private async Task<bool> IdentityCheckAndSetUserInfoAsync(string username, string password)
        {
            bool success = false;
            var user = await _userService.GetByUserNameAndPasswordAsync(username, password);

            if (user != null)
            {
                await SetAuthenticationUserInfoAsync(user);
                await UpdateUserLastLoginDateAsync(user);
                success = true;
            }
            return success;
        }

        private async Task SetAuthenticationUserInfoAsync(User user)
        {

            HttpContext.Session.Clear();
            // 登入成功，將使用者資訊存入Session
            HttpContext.Session.SetString("UserSession", user.UserName);

            // 設定 Cookie 驗證
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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

        private async Task UpdateUserLastLoginDateAsync(User user)
        {
            // 更新使用者的登入時間
            user.LastLoginDate = user.NewLoginDate;
            user.NewLoginDate = DateTime.Now;
            await _userService.Generic.UpdateAsync(user);
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
            RegisterUserViewModel model = new RegisterUserViewModel
            {
                Cities = CitiesSelectItemList()
            };

            return View(model);
        }
        private List<SelectListItem> CitiesSelectItemList()
        {
            var cities = ConfigUtil.GetTaiwanCitisSection(_config);
            var selectListItems = cities.Select(city => new SelectListItem { Text = city.CityName, Value = city.CityName }).ToList();
            selectListItems.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "請選擇縣市"
            });
            
            return selectListItems;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel RegisterVM)
        {
            bool success = false;
            RegisterVM.Cities = CitiesSelectItemList();
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "請正確填寫欄位";
                return View(RegisterVM);
            }
            if (!RegisterInfoCheck(RegisterVM.UserInfo, RegisterVM.ConfirmPassword))
            {
                return View(RegisterVM);
            }

            try
            {
                bool isExist = await _userService.IsExistUserNameAsync(RegisterVM.UserInfo.UserName);
                if (isExist)
                {
                    ViewBag.ErrorMsg = "帳號已存在，請重新輸入";
                    return View(RegisterVM);
                }
                else
                    success = true;

                if (success)
                {
                    RegisterVM.UserInfo.CreatDate = DateTime.Now;
                    await _userService.AddUserAsync(_mapper.Map<User>(RegisterVM.UserInfo));
                }
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMsg = string.Concat(RegisterVM.UserInfo.UserName, " ", "註冊失敗，請稍後再試");
                success = false;                
                return View(RegisterVM);
                // throw new Exception(ex.Message);
            }

            if (success)
            {
                TempData["RegiserSuccessfully"] = success;
                return RedirectToAction(nameof(RegisterSuccess), "Login");
            }

            return View(RegisterVM);
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

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.ResultMsg = "請正確填寫欄位";
                return View(model);
            }

            User user = await _userService.GetUserForForgotPasswordAsync(model.UserName, model.Name, model.Email);

            if(user == null)
            {
                model.ResultMsg = "找不到符合條件的使用者，請確認輸入的帳號、姓名和電子郵件是否正確";
                return View(model);
            }
            TempData["ForgotPasswordUser"] = JsonSerializer.Serialize(user);

            return RedirectToAction(nameof(ResetPassword));
        }
        public async Task<ActionResult> ResetPassword()
        {
            string? forgotPasswordUser = TempData["ForgotPasswordUser"] as string;
            if (string.IsNullOrEmpty(forgotPasswordUser))
            {
                return RedirectToAction("ForgotPassword");
            }

            User user = JsonSerializer.Deserialize<User>(forgotPasswordUser);

            string tempPassword = CommUtil.RandomString(10);

            await _userService.UpdateUserPasswordAsync(user, tempPassword);

            ViewBag.UserName = user.UserName;
            ViewBag.TempPassword = tempPassword;

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
