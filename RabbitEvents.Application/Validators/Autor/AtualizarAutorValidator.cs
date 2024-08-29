using FluentValidation;
using RabbitEvents.Shared.Inputs.Authors;

namespace RabbitEvents.Application.Validators.Autor;

public class AtualizarAutorValidator : AbstractValidator<UpdateAuthorInput>
{
    public AtualizarAutorValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .Must(GuidValidator.Validate)
            .WithMessage("ID informado é inválido");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255)
            .WithMessage("Informe um Nome válido!");

        RuleFor(x => x.Sobre)
            .MaximumLength(1000)
            .When(z => string.IsNullOrWhiteSpace(z.Sobre) is false)
            .WithMessage("Dados do Autor inválidos");

        RuleFor(x => x.Biografia)
            .MaximumLength(1000)
            .When(z => string.IsNullOrWhiteSpace(z.Sobre) is false)
            .WithMessage("Dados do Autor inválidos");
    }
}
