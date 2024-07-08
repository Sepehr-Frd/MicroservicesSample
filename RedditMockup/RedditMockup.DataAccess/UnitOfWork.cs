using RedditMockup.DataAccess.Context;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private IBaseRepository<Answer>? _answerRepository;

    private IBaseRepository<Person>? _personRepository;

    private IBaseRepository<Question>? _questionRepository;

    private IBaseRepository<Role>? _roleRepository;

    private IBaseRepository<User>? _userRepository;

    private readonly RedditMockupContext _context;

    private readonly ISieveProcessor _sieveProcessor;

    public UnitOfWork(RedditMockupContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public IBaseRepository<Answer> AnswerRepository =>
        _answerRepository ??= new AnswerRepository(_context, _sieveProcessor);

    public IBaseRepository<Person> PersonRepository =>
        _personRepository ??= new PersonRepository(_context, _sieveProcessor);

    public IBaseRepository<Question> QuestionRepository =>
        _questionRepository ??= new QuestionRepository(_context, _sieveProcessor);

    public IBaseRepository<Role> RoleRepository =>
        _roleRepository ??= new RoleRepository(_context, _sieveProcessor);

    public IBaseRepository<User> UserRepository =>
        _userRepository ??= new UserRepository(_context, _sieveProcessor);

    public async Task<int> CommitAsync(CancellationToken cancellationToken) =>
        await _context.SaveChangesAsync(cancellationToken);
}