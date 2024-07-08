using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Common.Dtos;

namespace RedditMockup.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AccountController : ControllerBase
{
    private readonly AccountBusiness _accountBusiness;

    public AccountController(AccountBusiness accountBusiness) =>
        _accountBusiness = accountBusiness;

    [Authorize]
    [HttpPost]
    [Route("logout")]
    public async Task<CustomResponse> Logout() =>
        await AccountBusiness.LogoutAsync(HttpContext);

    [HttpPost]
    [Route("login")]
    public async Task<CustomResponse> LoginAsync(LoginDto login, CancellationToken cancellationToken) =>
        await _accountBusiness.LoginAsync(login, HttpContext, cancellationToken);

}