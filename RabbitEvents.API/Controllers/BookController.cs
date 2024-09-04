using RabbitEvents.Application.Validators.Books;
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

        return ApiResult<CreateBookResponse>(response);
    }
}
