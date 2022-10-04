using System.Text.Json;

namespace Bridge.WebApp.Services
{
    public interface ILocalStorageService
    {
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask<TItem> GetItemAsync<TItem>(string key, CancellationToken? cancellationToken = null);

        ValueTask SetItemAsync<TItem>(string key, TItem item, CancellationToken? cancellationToken = null);

        ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null);

        ValueTask<TItem?> TryGetItemAsync<TItem>(string key, CancellationToken? cancellationToken = null);

        ValueTask TryRemoveItemAsync(string key, CancellationToken? cancellationToken = null);
    }

    public class EncryptionLocationStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _blazoredLocalStorageService;
        private readonly IEncryptionService _encryptionService;


        public EncryptionLocationStorageService(Blazored.LocalStorage.ILocalStorageService blazoredLocalStorageService, IEncryptionService encryptionService)
        {
            _blazoredLocalStorageService = blazoredLocalStorageService;
            _encryptionService = encryptionService;
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
        {
            return _blazoredLocalStorageService.ContainKeyAsync(key, cancellationToken);
        }

        public async ValueTask<TItem> GetItemAsync<TItem>(string key, CancellationToken? cancellationToken = null)
        {
            var chiper = await _blazoredLocalStorageService.GetItemAsync<string>(key, cancellationToken);
            var json = _encryptionService.Decrypt(chiper)!;
            return JsonSerializer.Deserialize<TItem>(json)!;
        }
     
        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            return _blazoredLocalStorageService.RemoveItemAsync(key, cancellationToken);
        }

        public async ValueTask SetItemAsync<TItem>(string key, TItem item, CancellationToken? cancellationToken = null)
        {
            var json = JsonSerializer.Serialize(item);
            var chiper = _encryptionService.Encrypt(json);
            await _blazoredLocalStorageService.SetItemAsync(key, chiper, cancellationToken);
        }

        public async ValueTask<TItem?> TryGetItemAsync<TItem>(string key, CancellationToken? cancellationToken = null)
        {
            if (await ContainKeyAsync(key))
                return await GetItemAsync<TItem>(key, cancellationToken);
            else
                return default;
        }

        public async ValueTask TryRemoveItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            if (await ContainKeyAsync(key))
                await RemoveItemAsync(key, cancellationToken);
        }
    }
}
