using Microsoft.EntityFrameworkCore;
using ToDoListManager.DataAccess.Base;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class ToDoListRepository : BaseRepository<ToDoList>
{
    public ToDoListRepository(DbContext dbContext) : base(dbContext)
    {
    }
}