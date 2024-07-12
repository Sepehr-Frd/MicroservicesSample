using System.Net;
using Humanizer;
using Mapster;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Businesses;

public class ToDoListBusiness : IToDoListBusiness
{
    private readonly IAuthBusiness _authBusiness;
    private readonly IBaseRepository<ToDoList> _toDoListRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToDoListBusiness(IUnitOfWork unitOfWork, IAuthBusiness authBusiness)
    {
        _unitOfWork = unitOfWork;
        _authBusiness = authBusiness;
        _toDoListRepository = unitOfWork.ToDoListRepository;
    }

    public async Task<CustomResponse<ToDoListDto?>> GetToDoListByGuidAsync(Guid toDoListGuid, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.GetByGuidAsync(toDoListGuid, null, cancellationToken);

        if (toDoList is null)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoList).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoList.UserId)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var toDoListDto = toDoList.Adapt<ToDoListDto>();

        return CustomResponse<ToDoListDto?>.CreateSuccessfulResponse(toDoListDto);
    }

    public async Task<CustomResponse<List<ToDoListDto>?>> GetLoggedInUserToDoListsAsync(PaginationDto? paginationDto = null, CancellationToken cancellationToken = default)
    {
        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        paginationDto ??= PaginationDto.DefaultPaginationDto;

        var toDoLists = await _toDoListRepository.GetAllAsync(
            paginationDto,
            toDoList => toDoList.UserId == loggedInUser!.Id,
            null,
            cancellationToken);

        var toDoListDtos = toDoLists.Adapt<List<ToDoListDto>>();

        return CustomResponse<List<ToDoListDto>?>.CreateSuccessfulResponse(toDoListDtos);
    }

    public async Task<CustomResponse<ToDoListDto?>> CreateToDoListAsync(ToDoListDto toDoListDto, CancellationToken cancellationToken = default)
    {
        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Guid != toDoListDto.UserGuid)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var toDoList = toDoListDto.Adapt<ToDoList>();

        toDoList.UserId = loggedInUser.Id;

        var createdToDoList = await _toDoListRepository.CreateAsync(toDoList, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var createdToDoListDto = createdToDoList.Adapt<ToDoListDto>();

        return CustomResponse<ToDoListDto?>.CreateSuccessfulResponse(
            createdToDoListDto,
            string.Format(MessageConstants.SuccessfullyCreated, nameof(ToDoList).Humanize(LetterCasing.Title)),
            HttpStatusCode.Created);
    }

    public async Task<CustomResponse<ToDoListDto?>> UpdateToDoListAsync(ToDoListDto toDoListDto, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.GetByGuidAsync(toDoListDto.Guid, null, cancellationToken);

        if (toDoList is null)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoList).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Guid != toDoListDto.UserGuid || loggedInUser.Id != toDoList.UserId)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        toDoListDto.Adapt(toDoList);

        var updatedToDoList = _toDoListRepository.Update(toDoList);

        await _unitOfWork.CommitAsync(cancellationToken);

        var updateToDoListDto = updatedToDoList.Adapt<ToDoListDto>();

        return CustomResponse<ToDoListDto?>.CreateSuccessfulResponse(
            updateToDoListDto,
            string.Format(
                MessageConstants.SuccessfullyUpdated,
                nameof(ToDoList).Humanize(LetterCasing.Title)));
    }

    public async Task<CustomResponse<ToDoListDto?>> DeleteToDoListByGuidAsync(Guid toDoListGuid, CancellationToken cancellationToken = default)
    {
        var toDoList = await _toDoListRepository.GetByGuidAsync(toDoListGuid, null, cancellationToken);

        if (toDoList is null)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoList).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoList.UserId)
        {
            return CustomResponse<ToDoListDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var deletedToDoList = _toDoListRepository.Delete(toDoList);

        await _unitOfWork.CommitAsync(cancellationToken);

        var deletedToDoListDto = deletedToDoList.Adapt<ToDoListDto>();

        return CustomResponse<ToDoListDto?>.CreateSuccessfulResponse(
            deletedToDoListDto,
            string.Format(
                MessageConstants.SuccessfullyDeleted,
                nameof(ToDoList).Humanize(LetterCasing.Title)));
    }
}