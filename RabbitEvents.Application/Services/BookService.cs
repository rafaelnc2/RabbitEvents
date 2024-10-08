using RabbitEvents.Application.IntegrationEvents.Books;
using RabbitEvents.Shared.Inputs.Authors;
using RabbitEvents.Shared.Inputs.Books;
using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.Application.Services;

public sealed class BookService : IBookDomainService
{
    private readonly ILogger<BookService> _logger;
    private readonly IAuthorRedisRepository _autorRedisRepository;
    private readonly IBookRedisRepository _bookRedisRepository;
    private readonly ImageService _imageService;

    public BookService(ILogger<BookService> logger, IAuthorRedisRepository autorRedisRepository, IBookRedisRepository bookRedisRepository, ImageService imageService)
    {
        _logger = logger;
        _autorRedisRepository = autorRedisRepository;
        _bookRedisRepository = bookRedisRepository;
        _imageService = imageService;
    }

    public async Task<ApiResponse<BookResponse>> CriarAsync(CreateBookInput criarInput)
    {
        _logger.LogInformation("Criando novo Livro");

        var response = new ApiResponse<BookResponse>();

        var author = await _autorRedisRepository.ObterPorIdAsync(criarInput.AuthorId);

        if (author is not Author)
            return response.BadRequestResponse(new List<string>() { "Autor informado é inválido" });

        var book = Book.Create(author.Id.ToString(), author.Nome, criarInput.Titulo, criarInput.Prefacio, criarInput.Edicao, criarInput.AnoPublicacao,
            criarInput.Editora, criarInput.GeneroLiterario, criarInput.Preco);

        var result = await _bookRedisRepository.CriarAsync(book);

        if (criarInput.Imagem is not null)
        {
            _logger.LogInformation("Criar livro com imagem");

            var bookWithImageCreatedEvent = new BookWithImageCreatedEvent(book.Id, criarInput.Imagem.GetFileExtension(), criarInput.Imagem.ContentType);
            var bookWithoutImageCreatedEvent = new BookWithoutImageCreatedEvent(book.Id, book.Titulo);

            await _imageService.SaveImageService(criarInput.Imagem, CacheKeysConstants.BOOK_IMAGE_KEY, book.Id, book.Titulo,
                bookWithImageCreatedEvent, bookWithoutImageCreatedEvent).ConfigureAwait(false); ;
        }

        var bookResponse = BookMap.ToCreateBookRespponse(book);

        _logger.LogInformation("Livro criado com sucesso");

        return response.CreatedResponse(bookResponse);
    }


    public async Task<ApiResponse<BookResponse>> AtualizarAsync(UpdateBookInput atualizarInput)
    {
        _logger.LogInformation($"Atualizando novo Livro Id: {atualizarInput.Id}");

        var response = new ApiResponse<BookResponse>();

        Book? book = await _bookRedisRepository.ObterPorIdAsync(atualizarInput.Id!);

        if (book is not Book)
            return response.NotFoundResponse();

        book.Update(atualizarInput.Titulo, atualizarInput.Prefacio, atualizarInput.Edicao, atualizarInput.AnoPublicacao,
            atualizarInput.Editora, atualizarInput.GeneroLiterario, atualizarInput.Preco);

        var result = await _bookRedisRepository.AtualizarAsync(book);

        if (atualizarInput.Imagem is not null)
        {
            var bookWithImageCreatedEvent = new BookWithImageCreatedEvent(book.Id, atualizarInput.Imagem.GetFileExtension(), atualizarInput.Imagem.ContentType);

            await _imageService.SaveImageService(atualizarInput.Imagem, CacheKeysConstants.BOOK_IMAGE_KEY, book.Id, book.Titulo,
                bookWithImageCreatedEvent).ConfigureAwait(false);
        }

        var bookResponse = BookMap.ToCreateBookRespponse(book);

        _logger.LogInformation("Livro atualizado com sucesso");

        return response.CreatedResponse(bookResponse);
    }


    public async Task<ApiResponse<BookResponse>> ObterPorIdAsync(GetBookByIdInput obterLivroPorIdInput)
    {
        _logger.LogInformation($"Obter Livro por ID: {obterLivroPorIdInput.Id}");

        var response = new ApiResponse<BookResponse>();

        var book = await _bookRedisRepository.ObterPorIdAsync(obterLivroPorIdInput.Id);

        if (book is null)
            return response.NotFoundResponse();

        var bookResponse = BookMap.ToCreateBookRespponse(book);

        _logger.LogInformation($"Livro ID: {obterLivroPorIdInput.Id} encontrado com sucesso");

        return response.OkResponse(bookResponse);
    }

    public async Task<ApiResponse<IEnumerable<BookResponse>>> ObterTodosAsync(GetBooksByFiltersInput? filtersInput)
    {
        _logger.LogInformation("Obter todos os Livros");

        var response = new ApiResponse<IEnumerable<BookResponse>>();

        var books = _bookRedisRepository.ObterTodos(filtersInput);

        var result = books.Select(book => BookMap.ToCreateBookRespponse(book));

        return response.OkResponse(result);
    }
}
