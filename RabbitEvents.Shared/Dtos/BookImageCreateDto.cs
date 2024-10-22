namespace RabbitEvents.Shared.Dtos;

public record BookImageCreateDto(Guid BookId, string BookTitle, string LiteraryGenre, string DescriptionForCreatingImage);
