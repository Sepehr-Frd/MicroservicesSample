using Microsoft.AspNetCore.Components;
using ToDoListManager.Api.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Api.Controllers;

[Route("users")]
public class UserController : BaseController<User, UserDto>
{
    public UserController(IBaseBusiness<User, UserDto> business) : base(business)
    {
    }
}