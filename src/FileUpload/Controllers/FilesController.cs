using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace FileUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private const string DIRECTORY = "UploadedFiles";
        private static string DirPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, DIRECTORY));

        public FilesController()
        {
            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);
        }

        static string NewFileName(string fileName)
        {
            return Guid.NewGuid().ToString() + "_" + fileName;
        }

        [HttpDelete("delete/{fileId}")]
        public IActionResult Delete(string fileId)
        {
            string path = Path.Combine(DirPath, fileId);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            return Ok();
        }

        /// <summary>
        /// 파일을 다운로드한다. 파일명으로 접근한다.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> Download(string fileId)
        {
            string path = Path.Combine(DirPath, fileId);

            if (System.IO.File.Exists(path))
            {
                // Get all bytes of the file and return the file with the specified file contents 
                byte[] b = await System.IO.File.ReadAllBytesAsync(path);
                return File(b, "application/octet-stream");
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            return await BufferUpload();
        }

        /// <summary>
        /// stream방식으로 파일을 업로드한다. 대용량 파일에 적합하다.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/stream")]
        public async Task<IActionResult> StreamUpload()
        {
            List<string> fileNames = new();
            var rawBoundary = MediaTypeHeaderValue.Parse(Request.ContentType).Boundary;
            var boundary = HeaderUtilities.RemoveQuotes(rawBoundary).Value;
            var reader = new MultipartReader(boundary, Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (contentDisposition!.DispositionType.Equals("form-data") &&
                        (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                        !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                    {
                        byte[] fileArray;
                        using (var memoryStream = new MemoryStream())
                        {
                            await section.Body.CopyToAsync(memoryStream);
                            fileArray = memoryStream.ToArray();
                        }
                        var fileName = NewFileName(contentDisposition.FileName.Value);
                        using (var fileStream = System.IO.File.Create(Path.Combine(DirPath, fileName)))
                        {
                            await fileStream.WriteAsync(fileArray);
                        }
                        fileNames.Add(fileName);
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }

            return Ok(fileNames);
        }

        /// <summary>
        /// buffer방식으로 파일을 업로드한다. 작은 용량의 파일에 적합하다.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/buffer")]
        public async Task<IActionResult> BufferUpload()
        {
            List<string> fileNames = new();
            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length == 0)
                    continue;

                try
                {
                    var fileName = NewFileName(formFile.FileName);
                    using (var fileStream = new FileStream(Path.Combine(DirPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                    fileNames.Add(fileName);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(fileNames);
        }
    }
}
