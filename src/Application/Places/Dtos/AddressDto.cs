
namespace Bridge.Application.Places.Dtos
{
    public class AddressDto
    {
        public static AddressDto Empty { get; } = new AddressDto();

        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string RoadAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}
