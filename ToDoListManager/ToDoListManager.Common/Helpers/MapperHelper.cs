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
    }
}