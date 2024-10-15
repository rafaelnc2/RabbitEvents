namespace RabbitEvents.Shared.Constants;

public static class CacheKeysConstants
{
    public static char KEY_SEPARATOR = ':';
    public static int DEFAULT_EXPIRES = 30;

    public static string AUTHOR_IMAGE_KEY = "author-image";
    public static string BOOK_IMAGE_KEY = "book-image";

    public static string LISTS_PRFIX = "Lists";

    public static string LITERARY_GENRES_PRFIX = "LiteraryGenres";
    public static int LITERARY_GENRE_MAX_LENGTH = 30;
    public static char LITERARY_GENRE_SEPARATOR = '|';
    public static string LITERARY_GENRE_LIST_NAME = $"{LISTS_PRFIX}{KEY_SEPARATOR}{LITERARY_GENRES_PRFIX}";
}
