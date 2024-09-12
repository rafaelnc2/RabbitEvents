
using FluentValidation;
using RabbitEvents.Shared.Inputs.Authors;

namespace RabbitEvents.Application.Validators.Autor;

public class CreateAuthorValidator : AbstractValidator<CreateAuthorInput>, FluentValidation.IValidator
{
    public CreateAuthorValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Informe um Nome válido!");

        RuleFor(x => x.Sobre)
            .MaximumLength(500)
            .When(z => string.IsNullOrWhiteSpace(z.Sobre) is false)
            .WithMessage("Dados do Autor inválidos");

        RuleFor(x => x.Biografia)
            .MaximumLength(1000)
            .When(z => string.IsNullOrWhiteSpace(z.Biografia) is false)
            .WithMessage("Biografia do Autor inválida");
    }
}
