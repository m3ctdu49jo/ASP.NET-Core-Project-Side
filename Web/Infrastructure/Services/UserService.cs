using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShoppingMall.DTOs;
using ShoppingMall.Infrastructure.Repositories;
using ShoppingMall.Infrastructure.Services;
using ShoppingMall.Models;
using Microsoft.AspNetCore.Http;

namespace ShoppingMall;

public class UserService : IUserService
{
    private readonly IGenericService<User, UserDTO> _genericService;
    private readonly IRepository<User> _userRepository;
    IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public User? UserInfo { get; set; } = null;
    
    public UserService(IGenericService<User, UserDTO> genericService, IRepository<User> userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _genericService = genericService;
        _userRepository = userRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;    
    }
    public IGenericService<User, UserDTO> Generic => _genericService;

    public async Task<UserDTO?> GetByUserNameAndPasswordAsync(string username, string password)
    {
        var hasher = new PasswordHasher<User>();

        var users = await _userRepository.FindAsync(x => x.UserName == username);

        if(users.Any())
        {
            User user = users.FirstOrDefault();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
                _httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }));
            
                UserInfo = user;
                return _mapper.Map<UserDTO>(user);
        }
        
        return null;

    }

    public async Task<bool> IsLoginUserAsync(string userSessionId)
    {
        if (UserInfo == null)
            return false;

        var hasher = new PasswordHasher<User>();
        var userHash = hasher.HashPassword(UserInfo, UserInfo.UserName);
        
        return hasher.VerifyHashedPassword(UserInfo, userHash, userSessionId) == PasswordVerificationResult.Success;
    }

    public async Task<bool> IsExistUserNameAsync(string username)
    {
        await _userRepository.FindAsync(x => x.UserName == username).ContinueWith(task =>
        {
            if (task.Result.Any())
                return true;
            else
                return false;
        });
        return false;
    }
}
