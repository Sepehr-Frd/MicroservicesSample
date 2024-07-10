using Sieve.Services;
using ToDoListManager.DataAccess.Base;
using ToDoListManager.DataAccess.Context;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Repositories;

public class PersonRepository : BaseRepository<Person>
{
    // [Constructor]

    public PersonRepository(ToDoListManagerDbContext dbContext, ISieveProcessor sieveProcessor) :
        base(dbContext, sieveProcessor)
    {
    }

}