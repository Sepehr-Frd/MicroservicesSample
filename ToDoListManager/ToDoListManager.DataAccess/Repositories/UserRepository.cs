using ToDoListManager.DataAccess.Base;
using ToDoListManager.DataAccess.Context;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class UserRepository : BaseRepository<User>
{
    // [Constructor]

    public UserRepository(ToDoListManagerDbContext dbContext) :
        base(dbContext)
    {
    }
}