using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common
{
    public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> IRequestHandler<TCommand, TResult>.Handle(TCommand request, CancellationToken cancellationToken)
        {
            return HandleCommand(request, cancellationToken);
        }

        public abstract Task<TResult> HandleCommand(TCommand command, CancellationToken cancellationToken);
    }
}
