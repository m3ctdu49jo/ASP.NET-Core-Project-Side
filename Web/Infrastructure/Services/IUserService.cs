using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public interface IUserService : IService<User, UserDTO>
{
    public Task<UserDTO?> GetByUserNameAndPasswordAsync(string username, string password);
    public Task<bool> IsExistUserNameAsync(string username);
    public Task<bool> IsLoginUserAsync(string userSessionId);
    public Task AddUserAsync(UserDTO userDTO);
}
