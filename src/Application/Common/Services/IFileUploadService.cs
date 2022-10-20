namespace Bridge.Application.Common.Services
{
    public interface IFileUploadService
    {
        /// <summary>
        ///  파입을 업로드하고 식별할 수 있는 경로를 반환한다.
        /// </summary>
        /// <param name="directoryName">디렉토리명</param>
        /// <param name="fileName">파일명</param>
        /// <param name="data">데이터</param>
        /// <returns>DB 파일경로</returns>
        string? UploadFile(string directoryName, string fileName, byte[] data);
    }
}
