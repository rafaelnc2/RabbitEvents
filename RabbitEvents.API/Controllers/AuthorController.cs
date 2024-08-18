using RabbitEvents.Application.Validators.Autor;
using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ApiBaseController
{
    private readonly IAuthorDomainService _autorService;
    public AuthorController(IAuthorDomainService autorService)
    {
        _autorService = autorService;
    }

    [HttpPost]
    [TypeFilter(typeof(ValidatorFilter<CreateAuthorInput, CriarAutorValidator>))]
    public async Task<IActionResult> CriarAsync([FromForm] CreateAuthorInput criarAutorInput)
    {
        var response = await _autorService.CriarAsync(criarAutorInput);

        return ApiResult<CreateAuthorResponse>(response);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidatorFilter<UpdateAuthorInput, AtualizarAutorValidator>))]
    public async Task<IActionResult> AtualizarAsync([FromRoute] string id, [FromForm] UpdateAuthorInput atualizarAutorInput)
    {
        var response = await _autorService.AtualizarAsync(atualizarAutorInput);

        return ApiResult<AuthorResponse>(response);
    }

    [HttpGet("{Id}")]
    [TypeFilter(typeof(ValidatorFilter<GetAuthorByIdInput, ObterAutorPorIdValidator>))]
    public async Task<IActionResult> ObterPorIdAsync([FromRoute] GetAuthorByIdInput obterAutorPorIdInput)
    {
        var response = await _autorService.ObterPorIdAsync(obterAutorPorIdInput);

        return ApiResult<AuthorResponse>(response);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosAsync()
    {
        var response = await _autorService.ObterTodosAsync();

        return ApiResult<IEnumerable<AuthorResponse>>(response);
    }
}
