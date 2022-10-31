namespace Bridge.WebApp.Services.GeoLocation
{
    /// <summary>
    /// Geolocation api 에러
    /// </summary>
    public class GeoError
    {
        public GeoError(int code, string message)
        {
            Code = code;
            Message = message;
            FriendlyMessage = code switch
            {
                1 => "위치 권한이 없습니다",
                2 => "내부 오류로 위치를 확인할 수 없습니다",
                3 => "위치 확인 중 타임아웃이 발생하였습니다",
                _ => "알수없는 응답코드입니다"
            };
        }

        public int Code { get; }
        public string Message { get; }
        public string FriendlyMessage { get; }
    }

}
