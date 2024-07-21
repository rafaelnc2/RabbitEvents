namespace RabbitEvents.Application.Validators;

public static class GuidValidator
{
    public static bool Validate(string input) =>
        Guid.TryParse(input, out Guid result);
}
