using FluentValidation;
using RabbitEvents.Shared.Inputs.Autor;

namespace RabbitEvents.Application.Validators.Autor;

public class ObterAutorPorIdValidator : AbstractValidator<ObterAutorPorIdInput>
{
    public ObterAutorPorIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(GuidValidator.Validate)
            .WithMessage("Informe um ID válido");
    }
}
