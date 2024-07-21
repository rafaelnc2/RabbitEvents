using RabbitEvents.Application.Extensions;
using RabbitEvents.Application.Mappings;
using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.Application.Services;

public sealed class AutorService(
    ILogger<AutorService> Logger,
    IAutorRedisRepository AutorRedisRepository) : IAutorDomainService
{
    //https://www.youtube.com/watch?v=Ft4SJgQETAk&t=81s
    // fazer upload de imagens blob storage local docker
    //https://hub.docker.com/r/microsoft/azure-storage-azurite
    //https://www.youtube.com/watch?v=Tt5zIKVMMbs = incluir cache para armazenar stream do arquivo

    public async Task<ApiResponse<CriarAutorResponse>> CriarAsync(CriarAutorInput criarInput)
    {
        Logger.LogInformation("Criando novo Autor");

        var response = new ApiResponse<CriarAutorResponse>();

        var autor = Autor.Create(criarInput.Nome, criarInput.Sobre, criarInput.Biografia, criarInput.Imagem?.GetFileExtension());

        var result = await AutorRedisRepository.CriarAsync(autor, criarInput.Imagem?.GetByteArray());

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

        var fileExtension = atualizarInput.Imagem?.GetFileExtension();

        autor.Update(atualizarInput.Nome, atualizarInput.Sobre, atualizarInput.Biografia, fileExtension);

        await AutorRedisRepository.AtualizarAsync(autor);

        var autorResponse = AutorMap.ToAutorResponse(autor);

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
