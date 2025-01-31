﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerMod
{
    public class PriorityEvent<T> where T : Delegate
    {
        public PriorityEvent(out IPriorityEventOwner p)
        {
            p = new PriorityEventOwner(this);
        }

        private readonly SortedDictionary<float, T> _events = new();

        public void Subscribe(float key, T subscriber)
        {
            T current = _events.TryGetValue(key, out current) ? current : null;
            _events[key] = (T)Delegate.Combine(current, subscriber);
        }

        public void Unsubscribe(float key, T subscriber)
        {
            if (_events.TryGetValue(key, out T current))
            {
                _events[key] = (T)Delegate.Remove(current, subscriber);
            }
        }

        public interface IPriorityEventOwner
        {
            T[] GetSubscriberList();
        }

        private class PriorityEventOwner : IPriorityEventOwner
        {
            public PriorityEventOwner(PriorityEvent<T> _parent) => this._parent = _parent;
            private readonly PriorityEvent<T> _parent;
            public T[] GetSubscriberList() => _parent._events.Values.ToArray();
        }
    }
}
