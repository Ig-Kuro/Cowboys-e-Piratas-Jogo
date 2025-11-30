using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple binary min-heap Priority Queue compatible with Unity / older runtimes.
/// Usage: var pq = new PriorityQueue<Personagem>(Comparer<float>.Default);
/// Enqueue(item, priority); Dequeue() returns item with smallest priority.
/// </summary>
public class PriorityQueue<T>
{
    private class Node
    {
        public T Item;
        public float Priority;
        public Node(T item, float priority) { Item = item; Priority = priority; }
    }

    private List<Node> heap;

    public PriorityQueue(int capacity = 16)
    {
        heap = new List<Node>(capacity);
    }

    public int Count => heap.Count;

    public void Enqueue(T item, float priority)
    {
        var node = new Node(item, priority);
        heap.Add(node);
        SiftUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (heap.Count == 0) throw new InvalidOperationException("PriorityQueue is empty");
        var root = heap[0].Item;
        if (heap.Count == 1)
        {
            heap.Clear();
            return root;
        }
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        SiftDown(0);
        return root;
    }

    public T Peek()
    {
        if (heap.Count == 0) throw new InvalidOperationException("PriorityQueue is empty");
        return heap[0].Item;
    }

    public void Clear() => heap.Clear();

    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].Priority < heap[parent].Priority)
            {
                Swap(i, parent);
                i = parent;
            }
            else break;
        }
    }

    private void SiftDown(int i)
    {
        int last = heap.Count - 1;
        while (true)
        {
            int left = i * 2 + 1;
            int right = i * 2 + 2;
            int smallest = i;

            if (left <= last && heap[left].Priority < heap[smallest].Priority)
                smallest = left;
            if (right <= last && heap[right].Priority < heap[smallest].Priority)
                smallest = right;

            if (smallest != i)
            {
                Swap(i, smallest);
                i = smallest;
            }
            else break;
        }
    }

    private void Swap(int a, int b)
    {
        var tmp = heap[a];
        heap[a] = heap[b];
        heap[b] = tmp;
    }
}
