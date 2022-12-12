using Bridge.Application.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bridge.Infrastructure.Services
{
    /// <summary>
    /// 서버 내부에 파일을 저장한다
    /// </summary>
    public class InternalFileUploadService : IFileUploadService
    {
        /// <summary>
        /// 업로드용 디렉토리
        /// </summary>
        private string UploadDirectory { get; }

        public InternalFileUploadService(IConfiguration configuration)
        {
            UploadDirectory = configuration.GetValue<string>("UploadDirectory");
        }

        private static string GetNextFilePath(string rootDirectory, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath)!;
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);

            int patternNumber = 1;
            var pattern = string.Format("({0})", patternNumber);
            var nextFilePath = Path.Combine(directory, baseName + pattern + extension);

            while (File.Exists(Path.Combine(rootDirectory, nextFilePath)))
            {
                patternNumber++;
                pattern = string.Format("({0})", patternNumber);
                nextFilePath = Path.Combine(directory, baseName + pattern + extension);
            }
            return nextFilePath;
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(UploadDirectory, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public string? UploadFile(string directoryName, string fileName, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(fileName) || data == null || data.Length == 0)
                return null;

            var filePath = Path.Combine(directoryName, fileName);
            var fullPath = Path.Combine(UploadDirectory, filePath);

            var directoryFullName = Path.GetDirectoryName(fullPath)!;
            if (!Directory.Exists(directoryFullName))
                Directory.CreateDirectory(directoryFullName);

            if (File.Exists(fullPath))
            {
                filePath = GetNextFilePath(UploadDirectory, filePath);
                fullPath = Path.Combine(UploadDirectory, filePath);
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
