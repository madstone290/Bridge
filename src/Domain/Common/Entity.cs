namespace Bridge.Domain.Common
{
    /// <summary>
    /// 도메인 엔티티
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 테이블 PK
        /// </summary>
        public int Pk { get; private set; }

        /// <summary>
        /// 엔티티 아이디
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        
    }
}
