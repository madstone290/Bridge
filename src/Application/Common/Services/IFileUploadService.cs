namespace Bridge.Application.Common.Services
{
    public interface IFileUploadService
    {
        /// <summary>
        /// 파일을 삭제한다
        /// </summary>
        /// <param name="filePath">파일경로</param>
        void DeleteFile(string filePath);

        /// <summary>
        ///  파입을 업로드하고 식별할 수 있는 경로를 반환한다.
        /// </summary>
        /// <param name="category">파일 카테고리. 디렉토리명으로 사용한다.</param>
        /// <param name="fileName">파일명</param>
        /// <param name="data">데이터</param>
        /// <returns>파일경로</returns>
        string? UploadFile(string category, string fileName, byte[] data);
    }
}
