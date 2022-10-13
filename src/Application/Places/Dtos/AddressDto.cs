
namespace Bridge.Application.Places.Dtos
{
    public class AddressDto
    {
        public static AddressDto Empty { get; } = new AddressDto();

        /// <summary>
        /// 기본주소
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string Details { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.Join(" ", BaseAddress, Details);
        }
    }
}
