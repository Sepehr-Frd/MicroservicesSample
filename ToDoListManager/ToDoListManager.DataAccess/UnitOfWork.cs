using ToDoListManager.DataAccess.Context;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.DataAccess.Repositories;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private IBaseRepository<Category>? _categoryRepository;

    private IBaseRepository<Person>? _personRepository;

    private IBaseRepository<User>? _userRepository;

    private IBaseRepository<ToDoItem>? _toDoItemRepository;

    private IBaseRepository<ToDoList>? _toDoListRepository;

    private readonly ToDoListManagerDbContext _toDoListManagerDbContext;

    public UnitOfWork(ToDoListManagerDbContext toDoListManagerDbContext)
    {
        _toDoListManagerDbContext = toDoListManagerDbContext;
    }

    public IBaseRepository<Category> CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_toDoListManagerDbContext);

    public IBaseRepository<Person> PersonRepository =>
        _personRepository ??= new PersonRepository(_toDoListManagerDbContext);

    public IBaseRepository<ToDoItem> ToDoItemRepository =>
        _toDoItemRepository ??= new ToDoItemRepository(_toDoListManagerDbContext);

    public IBaseRepository<ToDoList> ToDoListRepository =>
        _toDoListRepository ??= new ToDoListRepository(_toDoListManagerDbContext);

    public IBaseRepository<User> UserRepository =>
        _userRepository ??= new UserRepository(_toDoListManagerDbContext);

    public async Task<int> CommitAsync(CancellationToken cancellationToken) =>
        await _toDoListManagerDbContext.SaveChangesAsync(cancellationToken);
}