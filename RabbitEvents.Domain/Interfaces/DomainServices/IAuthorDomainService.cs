﻿using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Domain.Interfaces.DomainServices;

public interface IAuthorDomainService
{
    Task<ApiResponse<CreateAuthorResponse>> CriarAsync(CreateAuthorInput criarInput);

    Task<ApiResponse<AuthorResponse>> AtualizarAsync(UpdateAuthorInput atualizarInput);

    Task<ApiResponse<AuthorResponse>> ObterPorIdAsync(GetAuthorByIdInput obterAutorPorIdInput);

    Task<ApiResponse<IEnumerable<AuthorResponse>>> ObterTodosAsync();
}