﻿namespace NotificationService.Application.Abstractions
{
    public interface IEventHandler<TEvent>
    {
        public Task HandleAsync(TEvent @event);
    }
}
