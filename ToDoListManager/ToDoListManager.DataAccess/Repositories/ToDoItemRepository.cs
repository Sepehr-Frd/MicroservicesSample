using Microsoft.EntityFrameworkCore;
using ToDoListManager.DataAccess.Base;
using ToDoListManager.DataAccess.Context;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class ToDoItemRepository : BaseRepository<ToDoItem>
{
    private readonly ToDoListManagerDbContext _dbContext;

    public ToDoItemRepository(ToDoListManagerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ToDoItem>> GetAllToDoItemsWithoutPaginationAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.ToDoItems!.ToListAsync(cancellationToken);
}