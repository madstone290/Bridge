using System.Linq.Expressions;

namespace Bridge.Application.Common
{
    /// <summary>
    /// 읽기모델 저장소.
    /// 엔티티 타입대신 읽기모델을 반환한다.
    /// </summary>
    public interface IReadRepository<TEntity, TReadModel>
    {
        Task<List<TReadModel>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
