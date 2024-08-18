using RabbitEvents.Domain.IntegrationEvents.AutorEvents;
using RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;
using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Services;

public sealed class AuthorService(
    ILogger<AuthorService> Logger,
    IAutorRedisRepository AutorRedisRepository,
    ICacheService CacheService,
    IBus Bus) : IAuthorDomainService
{
    public async Task<ApiResponse<CreateAuthorResponse>> CriarAsync(CreateAuthorInput criarInput)
    {
        Logger.LogInformation("Criando novo Autor");

        var response = new ApiResponse<CreateAuthorResponse>();

        var autor = Author.Create(criarInput.Nome, criarInput.Sobre, criarInput.Biografia, criarInput.Genero);

        var result = await AutorRedisRepository.CriarAsync(autor);

        //Criar service para imagens
        if (criarInput.Imagem is not null)
        {
            var fileExtension = criarInput.Imagem.GetFileExtension();

            await CacheService.SetValueAsync(
                $"{CacheKeysConstants.AUTHOR_IMAGE_KEY}{CacheKeysConstants.KEY_SEPARATOR}{autor.Id}",
                criarInput.Imagem.GetByteArray()!,
                CacheKeysConstants.DEFAULT_EXPIRES
            )!;

            var autorComImagemCriadoEvent = new AuthorWithImageCreatedEvent(autor.Id, fileExtension, criarInput.Imagem.ContentType);

            await Bus.Publish((object)autorComImagemCriadoEvent);
        }
        else
        {
            var authorWithoutImageCreatedEvent = new AuthorWithoutImageCreatedEvent(autor.Id, autor.Nome);

            await Bus.Publish((object)authorWithoutImageCreatedEvent);
        }

        var autorResponse = AutorMap.ToCriarAutorResponse(result);

        Logger.LogInformation("Autor criado com sucesso");

        return response.CreatedResponse(autorResponse);
    }

    public async Task<ApiResponse<AuthorResponse>> AtualizarAsync(UpdateAuthorInput atualizarInput)
    {
        Logger.LogInformation($"Atualizando Autor ID: {atualizarInput.Id}");

        var response = new ApiResponse<AuthorResponse>();

        var autor = await AutorRedisRepository.ObterPorIdAsync(atualizarInput.Id!);

        if (autor is null)
            return response.NotFoundResponse();

        autor.Update(atualizarInput.Nome, atualizarInput.Sobre, atualizarInput.Biografia, atualizarInput.Imagem?.GetFileExtension());

        await AutorRedisRepository.AtualizarAsync(autor, atualizarInput.Imagem?.GetByteArray());

        var autorResponse = AutorMap.ToAutorResponse(autor);

        Logger.LogInformation("Autor atualizado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<AuthorResponse>> ObterPorIdAsync(GetAuthorByIdInput obterAutorPorIdInput)
    {
        Logger.LogInformation($"Obter Autor por ID: {obterAutorPorIdInput.Id}");

        var response = new ApiResponse<AuthorResponse>();

        var autor = await AutorRedisRepository.ObterPorIdAsync(obterAutorPorIdInput.Id);

        if (autor is null)
            return response.NotFoundResponse();

        var autorResponse = AutorMap.ToAutorResponse(autor);

        Logger.LogInformation($"Autor ID: {obterAutorPorIdInput.Id} encontrado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<IEnumerable<AuthorResponse>>> ObterTodosAsync()
    {
        Logger.LogInformation("Obter todos os Autores");

        var response = new ApiResponse<IEnumerable<AuthorResponse>>();

        var autores = await AutorRedisRepository.ObterTodosAsync();

        var result = autores.Select(aut => AutorMap.ToAutorResponse(aut));

        return response.OkResponse(result);
    }
}
