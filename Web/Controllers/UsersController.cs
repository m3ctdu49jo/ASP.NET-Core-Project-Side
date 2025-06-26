using System.Security.Claims;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Filters;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using ShoppingMall.Web.Utils;
using ShoppingMall.Web.ViewModels;

namespace ShoppingMall.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticatedFilter))]
    public class UsersController : Controller
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IUserService userService, IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _userService = userService;
            _config = config;
        }

        // GET: UsersController
        public async Task<ActionResult<UserDTO>> UserCenter()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);
            var user = await _userService.GetByIdAndUserNameAsync(userId, User.Identity?.Name ?? string.Empty);
            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<UserDTO>(user));
        }

        public async Task<ActionResult<EditUserInfoViewModel>> Edit()
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return NotFound();

            var user = await _userService.GetByIdAndUserNameAsync(userId, User.Identity?.Name ?? "");
            if (user == null)
                return NotFound();
                
            try
            {
                var cities = ConfigUtil.GetTaiwanCitisSection(_config);
                EditUserInfoViewModel EditUserVM = new EditUserInfoViewModel()
                { 
                    UserInfo = _mapper.Map<UserDTO>(user),
                    Cities = CitiesSelectItemList()
                };

                var selectedItem = EditUserVM.Cities.Find(x => x.Text == user.City);
                if (selectedItem != null)
                    selectedItem.Selected = true;            

                return View(EditUserVM);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserInfoViewModel EditUserInfoVM)
        {            
            // 資料重新填充
            EditUserInfoVM.Cities = CitiesSelectItemList();
            var selectedItem = EditUserInfoVM.Cities.Find(x => x.Text == EditUserInfoVM.UserInfo.City);
            if(selectedItem != null)
                selectedItem.Selected = true;
                
            if (!ModelState.IsValid)
            {
                return View(EditUserInfoVM);
            }
            var userInfo = EditUserInfoVM.UserInfo;
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return NotFound();
            try
            {
                var user = await _userService.GetByIdAndUserNameAsync(userId, userInfo.UserName ?? string.Empty);
                if (user == null || user.UserId != userId)
                    return NotFound();       
                
                if (user.UpdateDate.HasValue && DateTime.Compare(user.UpdateDate.GetValueOrDefault().AddSeconds(10), DateTime.Now) > 0)
                {
                    EditUserInfoVM.ResultMsg = "資料更新過於頻繁，請稍後再試";
                    return View(EditUserInfoVM);
                }

                _mapper.Map(EditUserInfoVM.UserInfo, user);
                await _userService.Generic.UpdateAsync(_mapper.Map(EditUserInfoVM.UserInfo, user));


                EditUserInfoVM.ResultMsg = $"資料更新成功!";
                return View(EditUserInfoVM);
            }
            catch (Exception ex)
            {
                EditUserInfoVM.ResultMsg = $"資料更新失敗: {ex.Message}";
                return View(EditUserInfoVM);
            }

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

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.GetByUserNameAndPasswordAsync(User.Identity?.Name ?? string.Empty, model.OldPassword);
            if (user == null)
            {
                model.ResultMessage = "舊密碼不正確";
                return View(model);
            }
            else if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                model.ResultMessage = "新密碼與確認新密碼不一致，請重新輸入";
                return View(model);
            } 
            
            try
            {
                await _userService.UpdateUserPasswordAsync(user, model.NewPassword);
                model.ResultMessage = $"密碼更新成功，請重新登入";
                model.IsSuccess = true;
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // 清除Session
                HttpContext.Session.Remove("UserSession");
            }
            catch (Exception ex)
            {
                model.ResultMessage = $"密碼更新失敗" + ex.Message;
                model.IsSuccess = false;
                return View(model);
            }

            return View(model);

        }
    }
}
