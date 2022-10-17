using Bridge.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
