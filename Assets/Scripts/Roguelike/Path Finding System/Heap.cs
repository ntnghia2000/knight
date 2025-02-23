using UnityEngine;
using System.Collections;
using System;

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemIndex;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public int CurrentItemIndex
    {
        get { return currentItemIndex; }
        set { currentItemIndex = value; }
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemIndex;
        items[currentItemIndex] = item;
        SortUp(item);
        currentItemIndex++;
    }

    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        currentItemIndex--;
        items[0] = items[currentItemIndex];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void SortUp(T item)
    {
        int parentItemIndex = (item.HeapIndex - 1) / 2;
        while (true) {
            T parentItem = items[parentItemIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            } else {
                break;
            }
        }
    }

    private void SortDown(T item)
    {
        while(true) {
            int leftChildIndex = item.HeapIndex * 2 + 1;
            int rightChildIndex = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (leftChildIndex < currentItemIndex) {
                swapIndex = leftChildIndex;
                if (rightChildIndex < currentItemIndex) {
                    if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0) {
                        swapIndex = rightChildIndex;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        SwapItemIndex(itemA, itemB);
    }

    private void SwapItemIndex(T itemA, T itemB)
    {
        int tempIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempIndex;
    }
}
