using System;
using Prism.Events;

namespace Metars.Services.Interfaces
{
    public interface IEventSubscriber
    {
        void Subscribe<TEvent, TPayload>(Action<TPayload> action, ThreadOption? option = null) where TEvent : PubSubEvent<TPayload>, new();
        void Subscribe<TEvent>(Action action, ThreadOption? option = null) where TEvent : PubSubEvent, new();
    }
}
