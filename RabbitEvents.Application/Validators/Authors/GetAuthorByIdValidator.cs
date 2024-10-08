﻿using FluentValidation;
using RabbitEvents.Shared.Inputs.Authors;

namespace RabbitEvents.Application.Validators.Autor;

public class GetBookByIdValidator : AbstractValidator<GetBookByIdInput>
{
    public GetBookByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(GuidValidator.Validate)
            .WithMessage("Informe um ID válido");
    }
}
