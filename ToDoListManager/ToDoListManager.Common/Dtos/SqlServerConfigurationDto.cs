namespace ToDoListManager.Common.Dtos;

public class SqlServerConfigurationDto
{
    public required string ConnectionString { get; set; }

    public required string UserId { get; set; }

    public required string Password { get; set; }
}