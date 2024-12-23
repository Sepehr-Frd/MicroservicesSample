namespace ToDoListManager.Common.Dtos;

public record ToDoListDto
{
    public ToDoListDto(Guid guid, string name, Guid userGuid, string? description = null)
    {
        Guid = guid;
        Name = name;
        UserGuid = userGuid;
        Description = description;
    }

    public Guid Guid { get; set; }

    public string Name { get; set; }

    public Guid UserGuid { get; set; }

    public string? Description { get; set; }
}