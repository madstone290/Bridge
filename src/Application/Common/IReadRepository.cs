using System.Linq.Expressions;

namespace Bridge.Application.Common
{
    /// <summary>
    /// 읽기모델 저장소.
    /// </summary>
    public interface IReadRepository<TEntity>
    {
    }

    /// <summary>
    /// 읽기모델 저장소. 
    /// 지정한 읽기모델을 반환한다.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TReadModel"></typeparam>
    public interface IReadRepository<TEntity, TReadModel> : IReadRepository<TEntity>
    {
        /// <summary>
        /// 조건에 맞는 첫번째 읽기모델을 반환한다.
        /// </summary>
        /// <param name="predicate">조건</param>
        /// <returns></returns>
        Task<TReadModel?> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 조건에 맞는 읽기모델을 반환한다.
        /// </summary>
        /// <param name="predicate">조건</param>
        /// <returns></returns>
        Task<List<TReadModel>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
