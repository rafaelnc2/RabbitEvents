namespace RabbitEvents.Application.Interfaces;

public interface ICacheService
{
    Task? SetValueAsync(string key, string value, int minutesToExpire = 15);
    Task? SetValueAsync(string key, byte[] value, int minutesToExpire = 15);
    Task GetValueAsync(string key);
    Task DeleteValueAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}
