using RabbitEvents.Infrastructure.IntegrationEvents.Events.AutorEvents;
using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Services;

public sealed class AutorService(
    ILogger<AutorService> Logger,
    IAutorRedisRepository AutorRedisRepository,
    ICacheService CacheService,
    IBus Bus) : IAutorDomainService
{
    public async Task<ApiResponse<CriarAutorResponse>> CriarAsync(CriarAutorInput criarInput)
    {
        Logger.LogInformation("Criando novo Autor");

        var response = new ApiResponse<CriarAutorResponse>();

        var autor = Autor.Create(criarInput.Nome, criarInput.Sobre, criarInput.Biografia);

        var result = await AutorRedisRepository.CriarAsync(autor);

        if (criarInput.Imagem is not null)
        {
            var fileExtension = criarInput.Imagem.GetFileExtension();

            autor.SetImage($"{autor.Id}.{fileExtension}");

            await CacheService.SetValueAsync(
                $"{CacheKeysConstants.AUTOR_IMAGE_KEY}:{autor.Id}",
                criarInput.Imagem.GetByteArray()!,
                CacheKeysConstants.DEFAULT_EXPIRES
            )!;

            var autorComImagemCriadoEvent = new AutorComImagemCriadoEvent(autor.Id, fileExtension, criarInput.Imagem.ContentType);

            await Bus.Publish((object)autorComImagemCriadoEvent);
        }

        var autorResponse = AutorMap.ToCriarAutorResponse(result);

        Logger.LogInformation("Autor criado com sucesso");

        return response.CreatedResponse(autorResponse);
    }

    public async Task<ApiResponse<AutorResponse>> AtualizarAsync(AtualizarAutorInput atualizarInput)
    {
        Logger.LogInformation($"Atualizando Autor ID: {atualizarInput.Id}");

        var response = new ApiResponse<AutorResponse>();

        var autor = await AutorRedisRepository.ObterPorIdAsync(atualizarInput.Id!);

        if (autor is null)
            return response.NotFoundResponse();

        autor.Update(atualizarInput.Nome, atualizarInput.Sobre, atualizarInput.Biografia, atualizarInput.Imagem?.GetFileExtension());

        await AutorRedisRepository.AtualizarAsync(autor, atualizarInput.Imagem?.GetByteArray());

        var autorResponse = AutorMap.ToAutorResponse(autor);

        Logger.LogInformation("Autor atualizado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<AutorResponse>> ObterPorIdAsync(ObterAutorPorIdInput obterAutorPorIdInput)
    {
        Logger.LogInformation($"Obter Autor por ID: {obterAutorPorIdInput.Id}");

        var response = new ApiResponse<AutorResponse>();

        var autor = await AutorRedisRepository.ObterPorIdAsync(obterAutorPorIdInput.Id);

        if (autor is null)
            return response.NotFoundResponse();

        var autorResponse = AutorMap.ToAutorResponse(autor);

        Logger.LogInformation($"Autor ID: {obterAutorPorIdInput.Id} encontrado com sucesso");

        return response.OkResponse(autorResponse);
    }

    public async Task<ApiResponse<IEnumerable<AutorResponse>>> ObterTodosAsync()
    {
        Logger.LogInformation("Obter todos os Autores");

        var response = new ApiResponse<IEnumerable<AutorResponse>>();

        var autores = await AutorRedisRepository.ObterTodosAsync();

        var result = autores.Select(aut => AutorMap.ToAutorResponse(aut));

        return response.OkResponse(result);
    }
}
