using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using RabbitEvents.Shared.Responses;

namespace RabbitEvents.API.Filters.Validators;

public class ValidatorFilter<TInput, TValidator> : Attribute, IAsyncActionFilter
    where TInput : class
    where TValidator : IValidator<TInput>, new()
{

    private readonly ILogger<ValidatorFilter<TInput, TValidator>> _logger;

    public ValidatorFilter(ILogger<ValidatorFilter<TInput, TValidator>> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("Validação do input");

        var inputModel = context.ActionArguments.Where(x => x.Value?.GetType() == typeof(TInput)).FirstOrDefault();

        if (inputModel.Value is null)
            throw new BadHttpRequestException("Input inválido");

        TInput input = inputModel.Value as TInput;

        SetIdFromRouteParameter(context, input);

        ValidationResult result = await ExecuteValidatorAsync(input);

        if (result.IsValid is false)
        {
            var errorList = new List<string>();

            foreach (var failure in result.Errors)
                errorList.Add("Erro na propriedade " + failure.PropertyName + " - " + failure.ErrorMessage);

            var response = new ApiResponse<TInput>().BadRequestResponse(errorList);

            context.Result = new BadRequestObjectResult(response);

            return;
        }

        await next();
    }

    private void SetIdFromRouteParameter(ActionExecutingContext context, TInput input)
    {
        var inputId = context.ActionArguments.Where(x => x.Key.Equals("id", StringComparison.OrdinalIgnoreCase));

        if (inputId is not null && inputId.FirstOrDefault().Value is not null)
        {
            var idFromRequestRoute = inputId.FirstOrDefault().Value!.ToString();

            var property = typeof(TInput).GetProperty("Id");

            if (property is not null && property.CanWrite)
                property.SetValue(input, idFromRequestRoute!.ToString());
        }
    }

    private async ValueTask<ValidationResult> ExecuteValidatorAsync(TInput input)
    {
        var validator = new TValidator();

        var isAsyncValidator = typeof(IValidator<TInput>).GetMethods().Any(m => m.Name == "ValidateAsync");

        ValidationResult result = isAsyncValidator ?
            await validator.ValidateAsync(input) :
            validator.Validate(input);

        return result;
    }
}
