using Bridge.WebApp.Api;
using MudBlazor;

namespace Bridge.WebApp.Extensions
{
    public static class SnackbarExtensions
    {
        /// <summary>
        /// Api 응답이 실패한 경우 메시지를 출력한다. 응답 성공여부 반환.
        /// </summary>
        public static bool CheckSuccess(this ISnackbar snackbar, params IApiResult[] results)
        {
            foreach (var result in results)
            {
                if (!result.Success)
                {
                    snackbar.Add(result.ErrorMessage, Severity.Error);
                    return false;
                }
            }
            return true;
        }
    }
}
