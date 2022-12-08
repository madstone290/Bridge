namespace Bridge.Domain.Common
{
    /// <summary>
    /// 애그리게이트 루트
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        /// <summary>
        /// 소유자 아이디
        /// </summary>
        public string OwnerId { get; private set; } = string.Empty;

        protected AggregateRoot() { }
        public AggregateRoot(string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
                throw new DomainException("소유자 아이디가 유효하지 않습니다");
            OwnerId = ownerId;
        }
    }
}
