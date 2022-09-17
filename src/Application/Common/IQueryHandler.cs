using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common
{
    public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
       where TQuery : IQuery<TResult>
    {
        Task<TResult> IRequestHandler<TQuery, TResult>.Handle(TQuery request, CancellationToken cancellationToken)
        {
            return HandleQuery(request, cancellationToken);
        }

        public abstract Task<TResult> HandleQuery(TQuery query, CancellationToken cancellationToken);
    }
}
