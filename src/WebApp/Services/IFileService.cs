using Microsoft.JSInterop;

namespace Bridge.WebApp.Services
{
    public interface IFileService
    {
        Task DownloadFileAsync(string fileName, Stream stream);
    }

    public class FileService : IFileService
    {
        private const string JsFile = "/js/file.js";
        private readonly IJSRuntime _jsRuntime;

        public FileService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadFileAsync(string fileName, Stream stream)
        {
            var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);
            
            using var streamRef = new DotNetStreamReference(stream);
            await module.InvokeVoidAsync("downloadFile", fileName, streamRef);
        }
    }
}
