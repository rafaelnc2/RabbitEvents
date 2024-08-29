using RabbitEvents.Domain.IntegrationEvents.AutorEvents;
using RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;
using RabbitEvents.Shared.Inputs.Authors;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Services;

public sealed class AuthorService : IAuthorDomainService
{
    private readonly ILogger<AuthorService> _logger;
    private readonly IAutorRedisRepository _autorRedisRepository;
    private readonly ImageService _imageService;

    public AuthorService(ILogger<AuthorService> logger, IAutorRedisRepository autorRedisRepository, ImageService imageService)
    {
        _logger = logger;
        _autorRedisRepository = autorRedisRepository;
        _imageService = imageService;
    }

    public async Task<ApiResponse<CreateAuthorResponse>> CriarAsync(CreateAuthorInput criarInput)
    {
        _logger.LogInformation("Criando novo Autor");

        var response = new ApiResponse<CreateAuthorResponse>();

        var author = Author.Create(criarInput.Nome, criarInput.Sobre, criarInput.Biografia);

        var result = await _autorRedisRepository.CriarAsync(author);

        if (criarInput.Imagem is not null)
        {
            var authorWithImageCreatedEvent = new AuthorWithImageCreatedEvent(author.Id, criarInput.Imagem.GetFileExtension(), criarInput.Imagem.ContentType);
            var authorWithoutImageCreatedEvent = new AuthorWithoutImageCreatedEvent(author.Id, author.Nome);

            await _imageService.SaveImageService(criarInput.Imagem, CacheKeysConstants.AUTHOR_IMAGE_KEY, author.Id, author.Nome,
                authorWithImageCreatedEvent, authorWithoutImageCreatedEvent);
        }

        var autorResponse = AuthorMap.ToCriarAutorResponse(result);

        _logger.LogInformation("Autor criado com sucesso");

        return response.CreatedResponse(autorResponse);
    }

    public async Task<ApiResponse<AuthorResponse>> AtualizarAsync(UpdateAuthorInput atualizarInput)
    {
        _logger.LogInformation($"Atualizando Autor ID: {atualizarInput.Id}");

        var response = new ApiResponse<AuthorResponse>();

        var author = await _autorRedisRepository.ObterPorIdAsync(atualizarInput.Id!);

        if (author is null)
            return response.NotFoundResponse();

        author.Update(atualizarInput.Nome, atualizarInput.Sobre, atualizarInput.Biografia);

        await _autorRedisRepository.AtualizarAsync(author);

        if (atualizarInput.Imagem is not null)
        {
            var authorWithImageUpdatedEvent = new AuthorWithImageCreatedEvent(author.Id, atualizarInput.Imagem.GetFileExtension(), atualizarInput.Imagem.ContentType);
            await _imageService.SaveImageService(atualizarInput.Imagem, CacheKeysConstants.AUTHOR_IMAGE_KEY, author.Id, author.Nome, authorWithImageUpdatedEvent);
        }

        var autorResponse = AuthorMap.ToAutorResponse(author);

        _logger.LogInformation("Autor atualizado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<AuthorResponse>> ObterPorIdAsync(GetAuthorByIdInput obterAutorPorIdInput)
    {
        _logger.LogInformation($"Obter Autor por ID: {obterAutorPorIdInput.Id}");

        var response = new ApiResponse<AuthorResponse>();

        var autor = await _autorRedisRepository.ObterPorIdAsync(obterAutorPorIdInput.Id);

        if (autor is null)
            return response.NotFoundResponse();

        var autorResponse = AuthorMap.ToAutorResponse(autor);

        _logger.LogInformation($"Autor ID: {obterAutorPorIdInput.Id} encontrado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<IEnumerable<AuthorResponse>>> ObterTodosAsync(string? nameFilter)
    {
        _logger.LogInformation("Obter todos os Autores");

        var response = new ApiResponse<IEnumerable<AuthorResponse>>();

        var autores = await _autorRedisRepository.ObterTodosAsync(nameFilter);

        var result = autores.Select(aut => AuthorMap.ToAutorResponse(aut));

        return response.OkResponse(result);
    }
}
