using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Business.Contracts;

public interface IUserBusiness
{
    Task<CustomResponse<UserDto?>> CreateUserAsync(SignupDto signupDto, CancellationToken cancellationToken = default);

    Task<CustomResponse<UserDto?>> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}