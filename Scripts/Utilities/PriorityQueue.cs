using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();
    private Dictionary<T, int> indices = new Dictionary<T, int>();

    public void Push(T data)
    {
        heap.Add(data);
        int index = heap.Count - 1;
        indices[data] = index;
        HeapifyUp(index);
    }

    public T Pop()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        T top = heap[0];
        T last = heap[heap.Count - 1];
        heap[0] = last;
        indices[last] = 0;
        heap.RemoveAt(heap.Count - 1);
        indices.Remove(top);

        if (heap.Count > 0)
            HeapifyDown(0);

        return top;
    }

    // 큐에 있는 요소의 값이 낮아졌을 때 사용
    public void DecreaseKey(T data)
    {
        if (!indices.TryGetValue(data, out int index))
            return;
        HeapifyUp(index);
    }

    // 큐에 해당 요소가 있는지 확인
    public bool Contains(T data)
    {
        return indices.ContainsKey(data);
    }

    // 큐에 있는 해당 요소를 가져옴
    public T Get(T data)
    {
        if (indices.TryGetValue(data, out int index))
            return heap[index];
        throw new KeyNotFoundException("Item not found in priority queue.");
    }

    public int Count()
    {
        return heap.Count;
    }

    public void Clear()
    {
        heap.Clear();
        indices.Clear();
    }

    private void HeapifyUp(int index)
    {
        int parent = (index - 1) / 2;
        while (index > 0 && heap[index].CompareTo(heap[parent]) > 0)
        {
            Swap(index, parent);
            index = parent;
            parent = (index - 1) / 2;
        }
    }

    private void HeapifyDown(int index)
    {
        int n = heap.Count;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int largest = index;

            if (left < n && heap[left].CompareTo(heap[largest]) > 0)
                largest = left;
            if (right < n && heap[right].CompareTo(heap[largest]) > 0)
                largest = right;
            if (largest == index)
                break;

            Swap(index, largest);
            index = largest;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;

        indices[heap[i]] = i;
        indices[heap[j]] = j;
    }
}
