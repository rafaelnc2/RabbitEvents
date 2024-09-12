using FluentValidation;
using RabbitEvents.Shared.Inputs.Books;

namespace RabbitEvents.Application.Validators.Books;

public class CreateBookValidator : AbstractValidator<CreateBookInput>, FluentValidation.IValidator
{
    public CreateBookValidator()
    {
        RuleFor(b => b.AuthorId)
            .NotEmpty()
            .Must(GuidValidator.Validate)
            .WithMessage("Informe um Autor ID válido");

        RuleFor(b => b.Titulo)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(b => b.Prefacio)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(1000);

        RuleFor(b => b.Edicao)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(b => b.Edicao is not null);

        RuleFor(b => b.AnoPublicacao)
            .NotNull()
            .GreaterThan(0);

        RuleFor(b => b.Editora)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(b => b.GeneroLiterario)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(b => b.Preco)
            .NotEmpty()
            .GreaterThan(0);
    }
}
