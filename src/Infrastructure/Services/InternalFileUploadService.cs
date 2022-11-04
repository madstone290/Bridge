using Bridge.Application.Common.Services;
using Microsoft.AspNetCore.Hosting;

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

        private static string GetNextFilePath(string rootPath, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath)!;
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);

            int patternNumber = 1;
            var pattern = string.Format("({0})", patternNumber);
            var nextFilePath = Path.Combine(directory, baseName + pattern + extension);

            while (File.Exists(Path.Combine(rootPath, nextFilePath)))
            {
                patternNumber++;
                pattern = string.Format("({0})", patternNumber);
                nextFilePath = Path.Combine(directory, baseName + pattern + extension);
            }
            return nextFilePath;
        }

        public void DeleteFile(string filePath)
        {
            var rootPath = _hostEnvironment.ContentRootPath;
            var fullPath = Path.Combine(rootPath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public string? UploadFile(string directoryName, string fileName, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(fileName) || data == null || data.Length == 0)
                return null;

            var rootPath = _hostEnvironment.ContentRootPath;
            var baseDirectory = "Files";
            var filePath = Path.Combine(baseDirectory, directoryName, fileName);
            var fullPath = Path.Combine(rootPath, filePath);

            var directoryFullName = Path.GetDirectoryName(fullPath)!;
            if (!Directory.Exists(directoryFullName))
                Directory.CreateDirectory(directoryFullName);

            if (File.Exists(fullPath))
            {
                filePath = GetNextFilePath(rootPath, filePath);
                fullPath = Path.Combine(rootPath, filePath);
            }
            using (var outputStream = new FileStream(fullPath, FileMode.Create))
            {
                using (var inputStream = new MemoryStream(data))
                {
                    inputStream.CopyTo(outputStream);
                }
            }

            return filePath;
        }

        
    }
}
