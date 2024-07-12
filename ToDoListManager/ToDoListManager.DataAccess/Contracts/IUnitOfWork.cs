using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Contracts;

public interface IUnitOfWork
{
    // [Properties]

    public IBaseRepository<Category> CategoryRepository { get; }

    public IBaseRepository<Person> PersonRepository { get; }

    public IBaseRepository<ToDoItem> ToDoItemRepository { get; }

    public IBaseRepository<ToDoList> ToDoListRepository { get; }

    public IBaseRepository<User> UserRepository { get; }

    // [Methods]

    Task<int> CommitAsync(CancellationToken cancellationToken);
}