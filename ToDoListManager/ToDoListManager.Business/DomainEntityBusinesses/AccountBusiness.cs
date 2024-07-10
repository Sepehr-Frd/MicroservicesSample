using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.Common.Helpers;
using Sieve.Models;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.DataAccess.Repositories;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.DomainEntityBusinesses;

public class AccountBusiness
{
    // [Fields]

    private readonly UserRepository _userRepository;

    // [Constructor]

    public AccountBusiness(IUnitOfWork unitOfWork) =>
        _userRepository = (UserRepository)unitOfWork.UserRepository!;

    // [Private Methods]

    private async Task<User?> ValidateAndGetUserByCredentialsAsync(LoginDto loginDto,
        CancellationToken cancellationToken = default)
    {
        var user = await LoadByUsernameAsync(loginDto.Username!, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var password = await loginDto.Password!.GetHashStringAsync();

        var isPasswordValid = password == user.Password;

        return !isPasswordValid ? null : user;
    }

    private static bool IsSignedIn(HttpContext httpContext) =>
        httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated;

    private async Task<User?> LoadByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={username}"
        };

        var users = await _userRepository.GetAllAsync(sieveModel,
            users => users
                .Include(x => x.UserRoles)!
                .ThenInclude(x => x.Role), cancellationToken);

        return users.Count == 0 ? null : users.Single();
    }

    // [Public Methods]

    public async Task<CustomResponse> LoginAsync(LoginDto login, HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        if (IsSignedIn(httpContext))
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, "You are already signed in.");
        }

        var user = await ValidateAndGetUserByCredentialsAsync(login, cancellationToken);

        if (user is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, "Username and/or password not correct.");
        }

        var roles = user.UserRoles!.Select(userRole => userRole.Role).ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(role?.Title!, role?.Title!)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = login.RememberMe
        };

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        return CustomResponse.CreateSuccessfulResponse("Successfully logged in.");
    }

    public async static Task<CustomResponse> LogoutAsync(HttpContext httpContext)
    {
        if (!IsSignedIn(httpContext))
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, "You are not signed in.");
        }

        await httpContext.SignOutAsync();

        return CustomResponse.CreateSuccessfulResponse("Successfully logged out.");
    }

}