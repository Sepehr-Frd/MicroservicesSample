using System.Net;
using Humanizer;
using Mapster;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Businesses;

public class UserBusiness : IUserBusiness
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthBusiness _authBusiness;

    public UserBusiness(IUnitOfWork unitOfWork, IAuthBusiness authBusiness)
    {
        _userRepository = unitOfWork.UserRepository;
        _unitOfWork = unitOfWork;
        _authBusiness = authBusiness;
    }

    public async Task<CustomResponse<UserDto?>> CreateUserAsync(SignupDto signupDto, CancellationToken cancellationToken = default)
    {
        var person = new Person
        {
            FirstName = signupDto.FirstName,
            LastName = signupDto.LastName
        };

        var createdPerson = await _unitOfWork.PersonRepository.CreateAsync(person, cancellationToken);

        var user = signupDto.Adapt<User>();

        user.PersonId = createdPerson.Id;

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

        var createdUserDto = createdUser.Adapt<UserDto>();

        return CustomResponse<UserDto?>.CreateSuccessfulResponse(
            createdUserDto,
            string.Format(MessageConstants.SuccessfullyCreated, nameof(User).Humanize(LetterCasing.Title)),
            HttpStatusCode.Created);
    }

    public async Task<CustomResponse<UserDto?>> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var user = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            return CustomResponse<UserDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Unauthorized);
        }

        var userDto = user.Adapt<UserDto>();

        return CustomResponse<UserDto?>.CreateSuccessfulResponse(userDto);
    }
}