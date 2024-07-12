using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthBusiness _authBusiness;

    public AuthController(IAuthBusiness authBusiness)
    {
        _authBusiness = authBusiness;
    }

    [Authorize]
    [HttpPost]
    [Route("logout")]
    public async Task<ActionResult<CustomResponse>> Logout()
    {
        var result = await _authBusiness.LogoutAsync();

        return StatusCode((int)result.HttpStatusCode, result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<CustomResponse>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var result = await _authBusiness.LoginAsync(loginDto, cancellationToken);

        return StatusCode((int)result.HttpStatusCode, result);
    }
}