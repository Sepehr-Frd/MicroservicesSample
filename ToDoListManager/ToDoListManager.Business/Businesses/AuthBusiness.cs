using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Common.Helpers;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Businesses;

public class AuthBusiness : IAuthBusiness
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthBusiness(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = unitOfWork.UserRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CustomResponse> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        if (IsSignedIn())
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, MessageConstants.AlreadySignedIn);
        }

        var user = await ValidateAndGetUserByCredentialsAsync(loginDto, cancellationToken);

        if (user is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, MessageConstants.InvalidCredentials);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Person!.FullName),
            new(ClaimTypes.UserData, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = loginDto.RememberMe
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        return CustomResponse.CreateSuccessfulResponse(MessageConstants.SuccessfulLogin);
    }

    public async Task<CustomResponse> LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync();

        return CustomResponse.CreateSuccessfulResponse(MessageConstants.SuccessfulLogout);
    }

    public async Task<User?> GetLoggedInUserAsync(CancellationToken cancellationToken = default)
    {
        if (!IsSignedIn())
        {
            return null;
        }

        var userIdString = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var userId = long.Parse(userIdString);

        return await _userRepository.GetByIdAsync(userId, null, cancellationToken);
    }

    private async Task<User?> ValidateAndGetUserByCredentialsAsync(LoginDto loginDto,
        CancellationToken cancellationToken = default)
    {
        var user = await LoadByUsernameAsync(loginDto.Username!, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var password = await loginDto.Password!.GetHashStringAsync();

        var isPasswordValid = password == user.PasswordHash;

        return !isPasswordValid ? null : user;
    }

    private bool IsSignedIn() =>
        _httpContextAccessor.HttpContext?.User.Identity is not null &&
        _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

    private async Task<User?> LoadByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(
            PaginationDto.DefaultPaginationDto,
            user => user.Username == username,
            users => users.Include(user => user.Person),
            cancellationToken);

        return users.Count == 0 ? null : users.Single();
    }
}