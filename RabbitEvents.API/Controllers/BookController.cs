using RabbitEvents.Application.Validators.Autor;
using RabbitEvents.Application.Validators.Books;
using RabbitEvents.Shared.Inputs.Authors;
using RabbitEvents.Shared.Inputs.Books;
using RabbitEvents.Shared.Responses.Books;

namespace RabbitEvents.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ApiBaseController
{
    private readonly IBookDomainService _bookService;

    public BookController(IBookDomainService bookDomainService)
    {
        _bookService = bookDomainService;
    }

    [HttpPost]
    [TypeFilter(typeof(ValidatorFilter<CreateBookInput, CreateBookValidator>))]
    public async Task<IActionResult> CriarAsync([FromForm] CreateBookInput createBookInput)
    {
        var response = await _bookService.CriarAsync(createBookInput);

        return ApiResult<BookResponse>(response);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidatorFilter<UpdateBookInput, UpdateBookValidator>))]
    public async Task<IActionResult> AtualizarAsync([FromRoute] string id, [FromForm] UpdateBookInput atualizarLivroInput)
    {
        var response = await _bookService.AtualizarAsync(atualizarLivroInput);

        return ApiResult<BookResponse>(response);
    }

    [HttpGet("{Id}")]
    [TypeFilter(typeof(ValidatorFilter<GetBookByIdInput, GetBookByIdValidator>))]
    public async Task<IActionResult> ObterPorIdAsync([FromRoute] GetBookByIdInput obterLivroPorIdInput)
    {
        var response = await _bookService.ObterPorIdAsync(obterLivroPorIdInput);

        return ApiResult<BookResponse>(response);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosAsync([FromQuery] GetBooksByFiltersInput? filtersInput)
    {
        var response = await _bookService.ObterTodosAsync(filtersInput);

        return ApiResult<IEnumerable<BookResponse>>(response);
    }
}
