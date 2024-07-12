namespace ToDoListManager.Common.Dtos;

public record SignupDto(
    string Username,
    string Password,
    string Email,
    string FirstName,
    string? LastName = null);