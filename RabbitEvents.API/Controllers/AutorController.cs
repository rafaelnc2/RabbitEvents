using RabbitEvents.Application.Validators.Autor;
using RabbitEvents.Shared.Inputs.Autor;
using RabbitEvents.Shared.Responses.Autor;

namespace RabbitEvents.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AutorController : ApiBaseController
{
    private readonly IAutorDomainService _autorService;
    public AutorController(IAutorDomainService autorService)
    {
        _autorService = autorService;
    }

    [HttpPost]
    [TypeFilter(typeof(ValidatorFilter<CriarAutorInput, CriarAutorValidator>))]
    public async Task<IActionResult> CriarAsync([FromForm] CriarAutorInput criarAutorInput)
    {
        var response = await _autorService.CriarAsync(criarAutorInput);

        return ApiResult<CriarAutorResponse>(response);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(ValidatorFilter<AtualizarAutorInput, AtualizarAutorValidator>))]
    public async Task<IActionResult> AtualizarAsync([FromRoute] string id, [FromBody] AtualizarAutorInput atualizarAutorInput)
    {
        var response = await _autorService.AtualizarAsync(atualizarAutorInput);

        return ApiResult<AutorResponse>(response);
    }

    [HttpGet("{Id}")]
    [TypeFilter(typeof(ValidatorFilter<ObterAutorPorIdInput, ObterAutorPorIdValidator>))]
    public async Task<IActionResult> ObterPorIdAsync([FromRoute] ObterAutorPorIdInput obterAutorPorIdInput)
    {
        var response = await _autorService.ObterPorIdAsync(obterAutorPorIdInput);

        return ApiResult<AutorResponse>(response);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosAsync()
    {
        var response = await _autorService.ObterTodosAsync();

        return ApiResult<IEnumerable<AutorResponse>>(response);
    }
}
