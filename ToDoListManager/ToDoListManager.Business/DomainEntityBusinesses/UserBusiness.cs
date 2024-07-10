using Mapster;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using ToDoListManager.Business.Base;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.DataAccess.Repositories;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.DomainEntityBusinesses;

public class UserBusiness : BaseBusiness<User, UserDto>
{
    // [Fields]

    private readonly UserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    // [Constructor]

    public UserBusiness(IUnitOfWork unitOfWork) :
        base(unitOfWork, unitOfWork.UserRepository!)
    {
        _userRepository = (UserRepository)unitOfWork.UserRepository!;

        _unitOfWork = unitOfWork;
    }

    // [Methods]

    public async override Task<User?> CreateAsync(UserDto userDto, CancellationToken cancellationToken = default)
    {
        var person = new Person
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };

        var createdPerson = await _unitOfWork.PersonRepository!.CreateAsync(person, cancellationToken);

        var user = userDto.Adapt<User>();

        user.PersonId = createdPerson.Id;

        return await CreateAsync(user, cancellationToken);
    }

    public async override Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _userRepository.GetByIdAsync(id,
            users => users.Include(user => user.Person)
                .Include(user => user.Profile),
            cancellationToken);

    public async override Task<User?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _userRepository.GetByGuidAsync(guid,
            users => users.Include(user => user.Person)
                .Include(user => user.Profile),
            cancellationToken);

    public async override Task<List<User>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default) =>
        await _userRepository.GetAllAsync(sieveModel,
            users => users.Include(user => user.Person)
                .Include(user => user.Profile),
            cancellationToken);

}