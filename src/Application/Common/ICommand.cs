using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }

    public interface ICommand : ICommand<Unit>
    {
    }
}
