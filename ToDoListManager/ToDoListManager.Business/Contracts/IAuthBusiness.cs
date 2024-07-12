using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Contracts;

public interface IAuthBusiness
{
    Task<CustomResponse> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    Task<CustomResponse> LogoutAsync();

    Task<User?> GetLoggedInUserAsync(CancellationToken cancellationToken = default);
}