using Microsoft.JSInterop;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// 공용 자바스크립트 함수를 제공한다
    /// </summary>
    public interface ICommonJsService
    {
        /// <summary>
        /// 아이템 위치까지 스크롤한다.
        /// </summary>
        /// <param name="parentId">아이템이 속한 부모 엘리멘트의 아이디</param>
        /// <param name="itemId">아이템 엘리멘트의 아이디</param>
        /// <returns></returns>
        Task ScrollAsync(string parentId, string itemId);

        /// <summary>
        /// 모바일 브라우저인지 확인한다.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsMobileBrowser();
    }

    public class CommonJsService : ICommonJsService
    {
        private const string JsFile = "/js/common.js";
        private readonly IJSRuntime _jsRuntime;

        private IJSObjectReference? _module;

        public CommonJsService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task<IJSObjectReference> GetModule()
        {
            _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);
            return _module;
        }

        public async Task ScrollAsync(string parentId, string itemId)
        {
            var module = await GetModule();
            await module.InvokeVoidAsync("scroll", parentId, itemId);
        }

        public async Task<bool> IsMobileBrowser()
        {
            var module = await GetModule();
            return await module.InvokeAsync<bool>("isMobileBrowser");
        }

    }
}
