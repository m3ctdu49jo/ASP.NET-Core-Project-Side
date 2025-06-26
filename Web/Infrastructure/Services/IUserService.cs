using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Infrastructure.Services;

public interface IUserService : IService<User>
{
    public Task<User?> GetByUserNameAndPasswordAsync(string username, string password);
    public Task<User?> GetByIdAndUserNameAsync(Guid id, string username);
    public Task<User?> GetUserForForgotPasswordAsync(string username, string name, string email);
    public Task<bool> IsExistUserNameAsync(string username);
    // public Task<bool> IsLoginUserAsync(string userSessionId);
    public Task AddUserAsync(User user);
    public Task UpdateUserPasswordAsync(User user, string newPassword);
}
