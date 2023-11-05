using System;
using System.Collections.Generic;

namespace ProjectDMG.Api.Notifications
{
    public class ObservableStack<T>
    {
        private Stack<T> stack = new();

        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;

        public int Count
        {
            get { return stack.Count; }
        }

        public void Push(T item)
        {
            stack.Push(item);

            // Raise event to notify that an item has been added
            OnItemAdded(new ItemAddedEventArgs<T>(item));
        }

        public T? Pop()
        {
            return stack.Pop();
        }

        public T? Peek()
        {
            return stack.Peek();
        }        

        protected virtual void OnItemAdded(ItemAddedEventArgs<T> e)
        {
            ItemAdded?.Invoke(this, e);
        }
    }

    public class ItemAddedEventArgs<T> : EventArgs
    {
        public T Item { get; private set; }

        public ItemAddedEventArgs(T item)
        {
            Item = item;
        }
    }
}
