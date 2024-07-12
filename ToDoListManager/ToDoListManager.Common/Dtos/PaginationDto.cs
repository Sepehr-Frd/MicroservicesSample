namespace ToDoListManager.Common.Dtos;

public record PaginationDto
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public PaginationDto() : this(1, 10)
    {
    }

    public PaginationDto(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber >= 1 ? pageNumber : 1;
        PageSize = pageSize >= 10 && pageSize <= 100 ? pageSize : 10;
    }

    public static PaginationDto DefaultPaginationDto { get; } = new();
}