using Bridge.Application.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bridge.Infrastructure.Services
{
    /// <summary>
    /// 서버 내부에 파일을 저장한다
    /// </summary>
    public class InternalFileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public InternalFileUploadService(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }

        private static string GetNextFilePath(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath)!;
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            
            int patternNumber = 1;
            var pattern = string.Format("({0})", patternNumber);
            var nextFilePath = Path.Combine(directory, baseName + pattern + extension);

            while (File.Exists(nextFilePath))
            {
                patternNumber++;
                pattern = string.Format("({0})", patternNumber);
                nextFilePath = Path.Combine(directory, baseName + pattern + extension);
            }
            return nextFilePath;
        }

        public string? UploadFile(string directoryName, string fileName, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(fileName) || data == null || data.Length == 0)
                return null;

            var rootPath = _hostEnvironment.ContentRootPath;
            var baseDirectory = "Files";

            var directoryPath = Path.Combine(rootPath, baseDirectory, directoryName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fullPath = Path.Combine(directoryPath, fileName);
            if (File.Exists(fullPath))
            {
                fullPath = GetNextFilePath(fullPath);
            }
            using (var outputStream = new FileStream(fullPath, FileMode.Create))
            {
                using (var inputStream = new MemoryStream(data))
                {
                    inputStream.CopyTo(outputStream);
                }
            }

            return Path.Combine(baseDirectory, directoryName, Path.GetFileName(fullPath));
        }

        
    }
}
