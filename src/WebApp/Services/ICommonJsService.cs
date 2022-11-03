using Bridge.WebApp.Api;
using Microsoft.JSInterop;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// 공용 자바스크립트 함수를 제공한다
    /// </summary>
    public interface ICommonJsService
    {
        /// <summary>
        /// JS모듈을 초기화한다.
        /// </summary>
        /// <returns></returns>
        Task Initialzie();

        /// <summary>
        /// 아이템 위치까지 스크롤한다.
        /// </summary>
        /// <param name="parentId">아이템이 속한 부모 엘리멘트의 아이디</param>
        /// <param name="itemId">아이템 엘리멘트의 아이디</param>
        /// <returns></returns>
        Task ScrollAsync(string parentId, string itemId);
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

        protected IJSObjectReference Module => _module!;

        public async Task Initialzie()
        {
            _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);
        }

        public async Task ScrollAsync(string parentId, string itemId)
        {
            await Module.InvokeVoidAsync("scroll", parentId, itemId);
        }

    }
}
