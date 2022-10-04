using Microsoft.JSInterop;
using System.Text.Json;

namespace Bridge.WebApp.Services
{
    public interface ICookieService
    {
        /// <summary>
        /// 쿠키를 저장한다
        /// </summary>
        /// <param name="name">쿠키명</param>
        /// <param name="value">쿠키값</param>
        /// <param name="maxAgeMin">분단위 쿠키수명</param>
        /// <returns></returns>
        Task SetCookieAsync(string name, string value, int? maxAgeMin = null);

        /// <summary>
        /// 쿠키를 저장한다
        /// </summary>
        /// <param name="name">쿠키명</param>
        /// <param name="item">쿠키아이템</param>
        /// <param name="maxAgeMin">분단위 쿠키수명</param>
        /// <returns></returns>
        Task SetCookieAsync<TItem>(string name, TItem item, int? maxAgeMin = null);

        /// <summary>
        /// 쿠키를 불러온다. 존재하지 않는 경우 default반환.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string?> GetCookieAsync(string name);


        /// <summary>
        /// 쿠키를 불러온다. 존재하지 않는 경우 default반환.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TItem?> GetCookieAsync<TItem>(string name);
    }

    public class CookieService : ICookieService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IEncryptionService _encryptionService;

        public CookieService(IJSRuntime jsRuntime, IEncryptionService encryptionService)
        {
            _jsRuntime = jsRuntime;
            _encryptionService = encryptionService;
        }

        public async Task<string?> GetCookieAsync(string name)
        {
            var value = await _jsRuntime.InvokeAsync<string?>("getCookie", name);
            if (value == null)
                return null;
            return _encryptionService.Decrypt(value);
        }

        public async Task<TItem?> GetCookieAsync<TItem>(string name)
        {
            var value = await GetCookieAsync(name);
            TItem? item = default;
            try 
            {
                if(value != null)
                    item = JsonSerializer.Deserialize<TItem>(value);
            }
            catch { }
            return item;
        }

        public async Task SetCookieAsync(string name, string value, int? maxAgeMin = null)
        {
            var chiper = _encryptionService.Encrypt(value);
            await _jsRuntime.InvokeVoidAsync("setCookie", name, chiper, maxAgeMin * 60);
        }

        public async Task SetCookieAsync<TItem>(string name, TItem item, int? maxAgeMin = null)
        {
            var value = JsonSerializer.Serialize(item);
            await SetCookieAsync(name, value, maxAgeMin);
        }
    }
}
