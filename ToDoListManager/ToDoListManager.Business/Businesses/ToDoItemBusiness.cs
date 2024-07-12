using System.Net;
using Humanizer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.Businesses;

public class ToDoItemBusiness : IToDoItemBusiness
{
    private readonly IAuthBusiness _authBusiness;
    private readonly IBaseRepository<ToDoItem> _toDoItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToDoItemBusiness(IUnitOfWork unitOfWork, IAuthBusiness authBusiness)
    {
        _unitOfWork = unitOfWork;
        _authBusiness = authBusiness;
        _toDoItemRepository = unitOfWork.ToDoItemRepository;
    }

    public async Task<CustomResponse<ToDoItemDto?>> GetToDoItemByGuidAsync(Guid toDoItemGuid, CancellationToken cancellationToken = default)
    {
        var toDoItem = await _toDoItemRepository.GetByGuidAsync(
            toDoItemGuid,
            toDoItems => toDoItems.Include(item => item.ToDoList),
            cancellationToken);

        if (toDoItem is null)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoItem).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoItem.ToDoList?.UserId)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var toDoItemDto = toDoItem.Adapt<ToDoItemDto>();

        return CustomResponse<ToDoItemDto?>.CreateSuccessfulResponse(toDoItemDto);
    }

    public async Task<CustomResponse<List<ToDoItemDto>?>> GetToDoItemsByListGuidAsync(Guid toDoListGuid, PaginationDto? paginationDto = null, CancellationToken cancellationToken = default)
    {
        var toDoList = await _unitOfWork
            .ToDoListRepository
            .GetByGuidAsync(toDoListGuid, null, cancellationToken);

        if (toDoList is null)
        {
            return CustomResponse<List<ToDoItemDto>?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoItem).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoList.UserId)
        {
            return CustomResponse<List<ToDoItemDto>?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        paginationDto ??= PaginationDto.DefaultPaginationDto;

        var toDoItems = await _toDoItemRepository.GetAllAsync(
            paginationDto,
            toDoItem => toDoItem.ToDoListId == toDoList.Id,
            null,
            cancellationToken);

        var toDoItemDtos = toDoItems.Adapt<List<ToDoItemDto>>();

        return CustomResponse<List<ToDoItemDto>?>.CreateSuccessfulResponse(toDoItemDtos);
    }

    public async Task<CustomResponse<ToDoItemDto?>> CreateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken = default)
    {
        var toDoList = await _unitOfWork
            .ToDoListRepository
            .GetByGuidAsync(toDoItemDto.ToDoListGuid, null, cancellationToken);

        if (toDoList is null)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoList).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoList.UserId)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var toDoItem = toDoItemDto.Adapt<ToDoItem>();

        toDoItem.ToDoListId = toDoList.Id;

        if (toDoItemDto.CategoryGuid.HasValue)
        {
            var category = await _unitOfWork
                .CategoryRepository
                .GetByGuidAsync(toDoItemDto.CategoryGuid.Value, null, cancellationToken);

            if (category is null)
            {
                return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                    HttpStatusCode.BadRequest,
                    string.Format(
                        MessageConstants.EntityNotFound,
                        nameof(Category).Humanize(LetterCasing.Title)));
            }

            if (category.UserId != loggedInUser.Id)
            {
                return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
            }

            toDoItem.CategoryId = category.Id;
        }

        var createdToDoItem = await _toDoItemRepository.CreateAsync(toDoItem, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var createdToDoItemDto = createdToDoItem.Adapt<ToDoItemDto>();

        return CustomResponse<ToDoItemDto?>.CreateSuccessfulResponse(
            createdToDoItemDto,
            string.Format(MessageConstants.SuccessfullyCreated, nameof(ToDoItem).Humanize(LetterCasing.Title)),
            HttpStatusCode.Created);
    }

    public async Task<CustomResponse<ToDoItemDto?>> UpdateToDoItemAsync(ToDoItemDto toDoItemDto, CancellationToken cancellationToken = default)
    {
        var toDoItem = await _toDoItemRepository.GetByGuidAsync(
            toDoItemDto.Guid,
            toDoItems =>
                toDoItems
                    .Include(item => item.ToDoList)
                    .Include(item => item.Category),
            cancellationToken);

        if (toDoItem is null || toDoItem.ToDoList?.Guid != toDoItemDto.ToDoListGuid)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoItem).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoItem.ToDoList.UserId)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        toDoItemDto.Adapt(toDoItem);

        if (toDoItemDto.CategoryGuid.HasValue && toDoItemDto.CategoryGuid.Value != toDoItem.Category!.Guid)
        {
            var newCategory = await _unitOfWork
                .CategoryRepository
                .GetByGuidAsync(toDoItemDto.CategoryGuid.Value, null, cancellationToken);

            if (newCategory is null)
            {
                return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                    HttpStatusCode.BadRequest,
                    string.Format(
                        MessageConstants.EntityNotFound,
                        nameof(Category).Humanize(LetterCasing.Title)));
            }

            toDoItem.CategoryId = newCategory.Id;
        }

        var updatedToDoItem = _toDoItemRepository.Update(toDoItem);

        await _unitOfWork.CommitAsync(cancellationToken);

        var updateToDoItemDto = updatedToDoItem.Adapt<ToDoItemDto>();

        return CustomResponse<ToDoItemDto?>.CreateSuccessfulResponse(
            updateToDoItemDto,
            string.Format(
                MessageConstants.SuccessfullyUpdated,
                nameof(ToDoItem).Humanize(LetterCasing.Title)));
    }

    public async Task<CustomResponse<ToDoItemDto?>> DeleteToDoItemByGuidAsync(Guid toDoItemGuid, CancellationToken cancellationToken = default)
    {
        var toDoItem = await _toDoItemRepository.GetByGuidAsync(
            toDoItemGuid,
            toDoItems => toDoItems.Include(item => item.ToDoList),
            cancellationToken);

        if (toDoItem is null)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(
                HttpStatusCode.BadRequest,
                string.Format(
                    MessageConstants.EntityNotFound,
                    nameof(ToDoItem).Humanize(LetterCasing.Title)));
        }

        var loggedInUser = await _authBusiness.GetLoggedInUserAsync(cancellationToken);

        if (loggedInUser!.Id != toDoItem.ToDoList!.UserId)
        {
            return CustomResponse<ToDoItemDto?>.CreateUnsuccessfulResponse(HttpStatusCode.Forbidden);
        }

        var deletedToDoItem = _toDoItemRepository.Delete(toDoItem);

        await _unitOfWork.CommitAsync(cancellationToken);

        var deletedToDoItemDto = deletedToDoItem.Adapt<ToDoItemDto>();

        return CustomResponse<ToDoItemDto?>.CreateSuccessfulResponse(
            deletedToDoItemDto,
            string.Format(
                MessageConstants.SuccessfullyDeleted,
                nameof(ToDoItem).Humanize(LetterCasing.Title)));
    }
}