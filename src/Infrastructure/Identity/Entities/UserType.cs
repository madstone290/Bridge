namespace Bridge.Infrastructure.Identity.Entities
{
    /// <summary>
    /// 사용자 타입
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 관리자
        /// </summary>
        Admin,

        /// <summary>
        /// 공급자. 장소 및 제품을 등록한다.
        /// </summary>
        Provider,

        /// <summary>
        /// 소비자
        /// </summary>
        Consumer

    }
}
