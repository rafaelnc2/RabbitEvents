using RabbitEvents.Shared.Inputs.Books;
using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Application.Services;

public sealed class BookService : IBookDomainService
{
    private readonly ILogger<BookService> _logger;
    private readonly IAuthorRedisRepository _autorRedisRepository;
    private readonly IBookRedisRepository _bookRedisRepository;

    public BookService(ILogger<BookService> logger, IAuthorRedisRepository autorRedisRepository, IBookRedisRepository bookRedisRepository)
    {
        _logger = logger;
        _autorRedisRepository = autorRedisRepository;
        _bookRedisRepository = bookRedisRepository;
    }

    public async Task<ApiResponse<CreateBookResponse>> CriarAsync(CreateBookInput criarInput)
    {
        _logger.LogInformation("Criando novo Livro");

        var response = new ApiResponse<CreateBookResponse>();

        var author = await _autorRedisRepository.ObterPorIdAsync(criarInput.AuthorId);

        if (author is not Author)
            return response.BadRequestResponse(new List<string>() { "Autor informado é inválido" });

        var book = Book.Create(author, criarInput.Titulo, criarInput.Prefacio, criarInput.Edicao, criarInput.AnoPublicacao,
            criarInput.Editora, criarInput.GeneroLiterario, criarInput.Preco);

        var result = await _bookRedisRepository.CriarAsync(book);

        if (criarInput.Imagem is not null)
        {
            _logger.LogInformation("Criar livro com imagem");

            //var authorWithImageCreatedEvent = new AuthorWithImageCreatedEvent(author.Id, criarInput.Imagem.GetFileExtension(), criarInput.Imagem.ContentType);
            //var authorWithoutImageCreatedEvent = new AuthorWithoutImageCreatedEvent(author.Id, author.Nome);

            //await _imageService.SaveImageService(criarInput.Imagem, CacheKeysConstants.AUTHOR_IMAGE_KEY, author.Id, author.Nome,
            //    authorWithImageCreatedEvent, authorWithoutImageCreatedEvent);
        }

        var bookResponse = BookMap.ToCreateBookRespponse(book);

        _logger.LogInformation("Livro criado com sucesso");

        return response.CreatedResponse(bookResponse);
    }


    public Task<Book> AtualizarAsync(UpdateBookInput atualizarInput)
    {
        throw new NotImplementedException();
    }


    public Task<Book> ObterPorIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> ObterTodosAsync()
    {
        throw new NotImplementedException();
    }
}
