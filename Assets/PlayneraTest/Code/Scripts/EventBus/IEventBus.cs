
using System;

namespace PlayneraTest.Code.Scripts.EventBus
{
    public interface IEventBus
    {
        public void Subscribe<T> (Action<T> hendler);
        public void Unsubscribe<T> (Action<T> hendler);
        public void RiseEvent<T> (T @event);
    }
}