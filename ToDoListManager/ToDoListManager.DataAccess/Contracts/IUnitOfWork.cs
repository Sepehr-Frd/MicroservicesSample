﻿using ToDoListManager.Model.Entities;

namespace ToDoListManager.DataAccess.Contracts;

public interface IUnitOfWork
{
    // [Properties]

    IBaseRepository<Answer>? AnswerRepository { get; }

    IBaseRepository<Person>? PersonRepository { get; }

    IBaseRepository<Question>? QuestionRepository { get; }

    IBaseRepository<Role>? RoleRepository { get; }

    IBaseRepository<User>? UserRepository { get; }

    // [Methods]

    Task<int> CommitAsync(CancellationToken cancellationToken);
}