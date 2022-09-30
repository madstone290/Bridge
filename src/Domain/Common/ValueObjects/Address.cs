namespace Bridge.Domain.Common.ValueObjects
{
    /// <summary>
    /// 대한민국 주소
    /// <para>시/도 + 시/구/군 + 읍/면 + (동/리 + 번지 or 도로명 + 건물번호) + 상세주소</para>
    /// </summary>
    public class Address : ValueObject
    {
        private Address() { }

        public static Address Empty => new();

        public static Address Create(string roadAddress, string jibunAddress, string details, string siDo, string siGuGun, string eupMyeonDong, string roadName, string postalCode)
        {
            return new Address()
            {
                RoadAddress = roadAddress,
                JibunAddress = jibunAddress,
                Details = details,
                SiDo = siDo,
                SiGuGun = siGuGun,
                EupMyeonDong = eupMyeonDong,
                RoadName = roadName,
                PostalCode = postalCode,
            };
        }

        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string RoadAddress { get; set; } = string.Empty;

        /// <summary>
        /// 지번주소
        /// </summary>
        public string JibunAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// 시/도
        /// </summary>
        public string SiDo { get; set; } = string.Empty;

        /// <summary>
        /// 시/구/군
        /// </summary>
        public string SiGuGun { get; set; } = string.Empty;

        /// <summary>
        /// 읍/면/동
        /// </summary>
        public string EupMyeonDong { get; set; } = string.Empty;

        /// <summary>
        /// 도로명주소 도로명
        /// </summary>
        public string RoadName { get; set; } = string.Empty;

        /// <summary>
        /// 우편번호
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        protected override IEnumerable<object?> GetEqualityPropertyValues()
        {
            yield return RoadAddress;
            yield return JibunAddress;
        }

    }
}
