using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Infrastructure.Repositories;
using ShoppingMall.Web.Infrastructure.Services;
using ShoppingMall.Web.Models;
using Microsoft.AspNetCore.Http;

namespace ShoppingMall.Web.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IGenericService<User> _genericService;
    private readonly IRepository<User> _userRepository;
    IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    // public User? UserInfo { get; set; } = null;

    public UserService(IGenericService<User> genericService, IRepository<User> userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _genericService = genericService;
        _userRepository = userRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    public IGenericService<User> Generic => _genericService;

    public async Task<User?> GetByUserNameAndPasswordAsync(string username, string password)
    {
        var hasher = new PasswordHasher<User>();

        var users = await _userRepository.FindAsync(x => x.UserName == username);

        if (users.Any())
        {
            User user = users.FirstOrDefault();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
        }

        return null;
    }
    public async Task<User?> GetByIdAndUserNameAsync(int id, string username)
    {
        var user = await _userRepository.FindAsync(x => x.Id == id && x.UserName == username.Trim());
        if (!user.Any())
            return null;

        return user.FirstOrDefault();
    }

    // public async Task<bool> IsLoginUserAsync(string userSessionId)
    // {
    //     if (UserInfo == null)
    //         return false;

    //     var hasher = new PasswordHasher<User>();
    //     var userHash = hasher.HashPassword(UserInfo, UserInfo.UserName);

    //     return hasher.VerifyHashedPassword(UserInfo, userHash, userSessionId) == PasswordVerificationResult.Success;
    // }

    public async Task<bool> IsExistUserNameAsync(string username)
    {
        var users = await _userRepository.FindAsync(x => x.UserName == username.Trim());
        return users.Any();
    }

    public async Task AddUserAsync(User user)
    {
        user.UpdateDate = DateTime.Now;
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, user.Password);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }
    
    public async Task UpdateUserPasswordAsync(User user, string newPassword)
    {
        user.UpdateDate = DateTime.Now;
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, newPassword);
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<User?> GetUserForForgotPasswordAsync(string username, string name, string email)
    {
        var user = await _userRepository.FindAsync(x =>
            x.UserName == username.Trim() &&
            x.Name == name.Trim() &&
            x.Email == email.Trim());
        return user.FirstOrDefault();
    }
}
