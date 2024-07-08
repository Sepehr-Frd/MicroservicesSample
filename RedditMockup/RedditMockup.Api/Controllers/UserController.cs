using Microsoft.AspNetCore.Components;
using RedditMockup.Api.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

[Route("users")]
public class UserController : BaseController<User, UserDto>
{
    public UserController(IBaseBusiness<User, UserDto> business) : base(business)
    {
    }
}