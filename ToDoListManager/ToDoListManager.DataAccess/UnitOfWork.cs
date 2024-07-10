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


    private readonly ToDoListManagerDbContext _dbDbContext;

    public UnitOfWork(ToDoListManagerDbContext dbDbContext)
    {
        _dbDbContext = dbDbContext;
    }
    
    public IBaseRepository<Category> CategoryRepository =>
        _categoryRepository ??= new CategoryRepository();
    
    public IBaseRepository<Person> PersonRepository =>
        _personRepository ??= new PersonRepository(_dbDbContext);

    public IBaseRepository<User> UserRepository =>
        _userRepository ??= new UserRepository(_dbDbContext, _sieveProcessor);

    public IBaseRepository<Person> PersonRepository =>
        _personRepository ??= new PersonRepository(_dbDbContext);
    
    public IBaseRepository<Person> PersonRepository =>
        _personRepository ??= new PersonRepository(_dbDbContext);
    
    public async Task<int> CommitAsync(CancellationToken cancellationToken) =>
        await _dbDbContext.SaveChangesAsync(cancellationToken);
}