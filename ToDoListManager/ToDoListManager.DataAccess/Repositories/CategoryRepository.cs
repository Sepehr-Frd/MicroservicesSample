using Microsoft.EntityFrameworkCore;
using ToDoListManager.DataAccess.Base;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class CategoryRepository : BaseRepository<Category>
{
    public CategoryRepository(DbContext dbContext) : base(dbContext)
    {
    }
}