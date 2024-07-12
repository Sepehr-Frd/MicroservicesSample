using ToDoListManager.DataAccess.Base;
using ToDoListManager.DataAccess.Context;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class PersonRepository : BaseRepository<Person>
{
    public PersonRepository(ToDoListManagerDbContext dbContext) :
        base(dbContext)
    {
    }
}