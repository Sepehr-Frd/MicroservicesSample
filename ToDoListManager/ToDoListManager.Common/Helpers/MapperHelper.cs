using Mapster;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Helpers;

public class MapperHelper
{
    public static void RegisterMapperConfigurations()
    {
        TypeAdapterConfig<User, UserDto>
            .NewConfig()
            .Map(userDto => userDto.FirstName, user => user.Person!.FullName);

        TypeAdapterConfig<ToDoItem, ToDoItemDto>
            .NewConfig()
            .Map(toDoItemDto => toDoItemDto.ToDoListGuid, toDoItem => toDoItem.ToDoList!.Guid)
            .Map(toDoItemDto => toDoItemDto.CategoryGuid, toDoItem => toDoItem.Category!.Guid)
            .IgnoreIf((toDoItem, _) => toDoItem.Category == null, nameof(ToDoItemDto.CategoryGuid));
    }
}