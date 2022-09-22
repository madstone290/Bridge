using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Services
{
    /// <summary>
    /// 주소의 지리정보를 제공한다.
    /// 외부 API를 사용한다.
    /// </summary>
    public interface IAddressMapService
    {
        /// <summary>
        /// 해당주소의 위도 및 경도를 가져온다.
        /// </summary>
        /// <param name="address">주소</param>
        /// <returns>위도, 경도 튜플</returns>
        Task<Tuple<double, double>> GetLatitudeAndLongitudeAsync(string address);

        /// <summary>
        /// 해당주소의 대한 UTM-K 동향, 북향 좌표를 가져온다.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>동향 북향 튜플</returns>
        Task<Tuple<double, double>> GetUTM_K_EastingAndNorthingAsync(string address);
    }
}
