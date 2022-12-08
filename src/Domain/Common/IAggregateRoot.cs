namespace Bridge.Domain.Common
{
    /// <summary>
    /// 애그리게이트 루트 식별용 마커 인터페이스
    /// </summary>
    public interface IAggregateRoot
    {
        string OwnerId { get; }
    }
}
