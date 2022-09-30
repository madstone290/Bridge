using Bridge.Domain.Common.ValueObjects;

namespace Bridge.Application.Common.Services
{
    /// <summary>
    /// 주소 및 위치에 관련된 기능을 제공한다
    /// </summary>
    public interface IAddressLocationService
    {
        /// <summary>
        /// 주소 및 위치를 생성한다
        /// </summary>
        /// <param name="baseAddress">기본 주소</param>
        /// <param name="details">상세 주소</param>
        /// <returns></returns>
        Task<Tuple<Address, PlaceLocation>> CreateAddressLocationAsync(string baseAddress, string details);

    }
}
