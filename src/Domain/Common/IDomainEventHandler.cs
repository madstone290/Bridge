﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
       Task HandleAsync(TEvent @event);
    }
}
