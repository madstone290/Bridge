namespace Bridge.Shared
{
    /// <summary>
    /// 실행 결과
    /// </summary>
    public class Result
    {
        protected Result() { }

        public static Result SuccessResult() => new() { Success = true };

        public static Result FailResult(string? error) => new() { Success = false, Error = error };

        /// <summary>
        /// 실행 성공 여부
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// 실행 중 발생한 에러
        /// </summary>
        public string? Error { get; init; }
    }

    /// <summary>
    /// 데이터를 포함하는 실행 결과
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class Result<TData> : Result
    {
        public static Result<TData> SuccessResult(TData data) => new() { Success = true, Data = data };

        public static new Result<TData> FailResult(string? error) => new() { Success = false, Error = error };

        /// <summary>
        /// 데이터
        /// </summary>
        public TData? Data { get; init; }
    }
}
