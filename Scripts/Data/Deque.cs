using System;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.Port;

namespace MyThings.Data
{
    [Serializable]
    public class Deque<T> : IEnumerable<T>

    {
        private T[] buffer;
        private int head;
        private int tail;
        private int count;

        public Deque(int capacity = 16)
        {
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }

        public int Count => count;

        public void AddToFront(T item)
        {
            EnsureCapacity();
            head = (head - 1 + buffer.Length) % buffer.Length;
            buffer[head] = item;
            count++;
        }

        public void AddToBack(T item)
        {
            EnsureCapacity();
            buffer[tail] = item;
            tail = (tail + 1) % buffer.Length;
            count++;
        }

        public T RemoveFromFront()
        {
            if (count == 0) throw new InvalidOperationException("Deque is empty");
            T value = buffer[head];
            buffer[head] = default;
            head = (head + 1) % buffer.Length;
            count--;
            return value;
        }

        public T RemoveFromBack()
        {
            if (count == 0) throw new InvalidOperationException("Deque is empty");
            tail = (tail - 1 + buffer.Length) % buffer.Length;
            T value = buffer[tail];
            buffer[tail] = default;
            count--;
            return value;
        }

        public T PeekFront()
        {
            if (count == 0) throw new InvalidOperationException("Deque is empty");
            return buffer[head];
        }

        public T PeekBack()
        {
            if (count == 0) throw new InvalidOperationException("Deque is empty");
            int index = (tail - 1 + buffer.Length) % buffer.Length;
            return buffer[index];
        }

        private void EnsureCapacity()
        {
            if (count == buffer.Length)
            {
                int newCapacity = buffer.Length * 2;
                T[] newBuffer = new T[newCapacity];
                for (int i = 0; i < count; i++)
                {
                    newBuffer[i] = buffer[(head + i) % buffer.Length];
                }
                buffer = newBuffer;
                head = 0;
                tail = count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return buffer[(head + i) % buffer.Length];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            buffer = new T[16];
            head = 0;
            tail = 0;
            count = 0;
        }
    }
}