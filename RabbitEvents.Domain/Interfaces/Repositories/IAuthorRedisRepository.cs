﻿namespace RabbitEvents.Domain.Interfaces.Repositories;

public interface IAuthorRedisRepository
{
    Task<Author> CriarAsync(Author autor);

    Task<Author> AtualizarAsync(Author autor);

    Task<Author?> ObterPorIdAsync(string autorId);

    Task<IEnumerable<Author>> ObterTodosAsync(string? filterName);
}
