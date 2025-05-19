using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Services;
using ShoppingMall.Models;

namespace ShoppingMall;

public interface IUserService : IService<User, UserDTO>
{
    public Task<UserDTO?> GetByUserNameAndPasswordAsync(string username, string password);
    public Task<bool> IsExistUserNameAsync(string username);
    public Task<bool> IsLoginUserAsync(string userSessionId);
}
