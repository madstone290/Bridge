using Bridge.WebApp.Api;
using MudBlazor;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// API 처리결과 유효성을 검증하고 결과를 출력한다
    /// </summary>
    public interface IApiResultValidationService
    {
        /// <summary>
        /// api 처리결과가 성공적이어야 한다
        /// </summary>
        /// <param name="result"></param>
        /// <param name="allowNullData">데이터 널 허용여부</param>
        /// <returns></returns>
        bool Validate<TData>(ApiResult<TData> result, bool allowNullData = false);
    }

    /// <summary>
    /// API 처리결과 유효성을 검증하고 결과를 Snackbar로 출력한다.
    /// </summary>
    public class SnackbarApiResultValidationService : IApiResultValidationService
    {
        private readonly ISnackbar _snackbar;
        
        public SnackbarApiResultValidationService(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        public bool Validate<TData>(ApiResult<TData> result, bool nullableData = false)
        {
            if (!result.Success)
            {
                _snackbar.Add(result.Error, Severity.Error);
                return false;
            }

            if (nullableData)
                return true;

            if (result.Data == null)
            {
                _snackbar.Add("데이터가 없습니다", Severity.Error);
                return false;
            }

            return true;
        }
    }
}
