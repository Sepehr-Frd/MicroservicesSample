﻿using ChangeDataCaptureHub.DataAccess;
using ChangeDataCaptureHub.Model.Models;

namespace ChangeDataCaptureHub.Business.Businesses;

public class BaseBusiness<T>
    where T : BaseDocument
{
    private readonly IBaseRepository<T> _repository;

    protected BaseBusiness(IBaseRepository<T> repository) =>
        _repository = repository;

    public async Task CreateOneAsync(T t, CancellationToken cancellationToken = default) =>
        await _repository.CreateOneAsync(t, cancellationToken);

    public async Task CreateManyAsync(IEnumerable<T> values, CancellationToken cancellationToken = default) =>
        await _repository.CreateManyAsync(values, cancellationToken);

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await _repository.GetByIdAsync(id, cancellationToken);

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _repository.GetAllAsync(cancellationToken);

    public async Task<bool> UpdateOneAsync(T t, CancellationToken cancellationToken = default) =>
        await _repository.UpdateOneAsync(t, cancellationToken);

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await _repository.DeleteByIdAsync(id, cancellationToken);
}