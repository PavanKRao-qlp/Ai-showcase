using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeapDT<T> where T : System.IComparable<T>
{
    private List<T> heapTreeData = new List<T>();
    public HeapDT()
    {
    }
    public HeapDT(uint size)
    {
        heapTreeData.Capacity = (int)size;
    }
    public int Count => heapTreeData?.Count ?? 0;

    private uint GetLchildIndex(uint parentIndex)
    {
        return (2 * parentIndex) + 1;
    }
    private uint GetRchildIndex(uint parentIndex)
    {
        return (2 * parentIndex) + 2;
    }
    private uint GetParentIndex(uint childIndex)
    {
        return (childIndex - 1) / 2;
    }
    public bool IsValidIndex(uint index)
    {
        if (heapTreeData == null) throw new System.NullReferenceException("heapTreeData NULL ");
        return index >= 0 && index < heapTreeData.Count;
    }

    public T GetLchild(uint index)
    {
        uint ix = GetLchildIndex(index);
        if (IsValidIndex(ix)) return heapTreeData[(int)ix];
        return default(T);
    }
    public T GetRchild(uint index)
    {
        uint ix = GetRchildIndex(index);
        if (IsValidIndex(ix)) return heapTreeData[(int)ix];
        return default(T);
    }
    public T GetParent(uint index)
    {
        uint ix = GetParentIndex(index);
        if (IsValidIndex(ix)) return heapTreeData[(int)ix];
        return default(T);
    }

    public void Swap(uint iX_1, uint ix_2)
    {
        if (!IsValidIndex(iX_1) || !IsValidIndex(ix_2)) throw new System.ArgumentOutOfRangeException();
        T temp = heapTreeData[(int)ix_2];
        heapTreeData[(int)ix_2] = heapTreeData[(int)iX_1];
        heapTreeData[(int)iX_1] = temp;
    }

    public void HeapifyUp()
    {
        uint ix = (uint)heapTreeData.Count - 1;
        while (ix > 0 && IsValidIndex(ix) && IsLBeforeR(heapTreeData[(int)ix], GetParent(ix)))
        {
            Swap(GetParentIndex(ix), ix);
            ix = GetParentIndex(ix);
        }
    }

    public void HeapifyDown()
    {
        uint ix = 0;
        while (IsValidIndex(GetLchildIndex(ix)))
        {
            uint smallestChildIx = GetLchildIndex(ix);
            uint rIx = GetRchildIndex(ix);
            if (IsValidIndex(rIx) && IsLBeforeR(GetRchild(ix), GetLchild(ix)))
            {
                smallestChildIx = rIx;
            }
            if (IsLBeforeR(heapTreeData[(int)ix], heapTreeData[(int)smallestChildIx]))
            {
                break;
            }
            else
                Swap(ix, smallestChildIx);
            ix = smallestChildIx;

        }
    }

    public abstract bool IsLBeforeR(T T_1, T T_2);

    public T At(int ix)
    {
        return heapTreeData[ix];
    }

    public void Add(T item)
    {
        heapTreeData.Add(item);
        HeapifyUp();
    }

    public T Peek()
    {
        if (heapTreeData == null) throw new System.NullReferenceException("heapTreeData NULL ");
        return heapTreeData[0];
    }
    public T Poll()
    {
        if (heapTreeData == null) throw new System.NullReferenceException("heapTreeData NULL ");
        if (heapTreeData.Count == 0) throw new System.ArgumentOutOfRangeException("heapTreeData empty");
        T val = heapTreeData[0];
        heapTreeData[0] = heapTreeData[heapTreeData.Count - 1];
        heapTreeData.RemoveAt(heapTreeData.Count - 1);
        HeapifyDown();
        return val;
    }

    public bool Contains(T item)
    {
        return heapTreeData.Contains(item);
    }

    public int TryGetIndexOf(T item)
    {
        return heapTreeData.IndexOf(item);
    }
    public T GetItemAt(int ix)
    {
        if (ix < 0 || ix >= heapTreeData.Count) throw new System.ArgumentOutOfRangeException();
        return heapTreeData[ix];
    }
}
public class MinHeap<T> : HeapDT<T> where T:System.IComparable<T>
{

    public MinHeap(uint size) : base(size)
    {
    }

    public override bool IsLBeforeR(T T_1, T T_2)
    {
        return T_1.CompareTo(T_2) < 0;
    }

}
public class MaxHeap<T> : HeapDT<T> where T : System.IComparable<T>
{
    public MaxHeap(uint size) : base(size)
    {
    }
    public override bool IsLBeforeR(T T_1, T T_2)
    {
        return T_1.CompareTo(T_2) > 0;
    }
}
public interface Iheapable : System.IComparable<Iheapable> 
{
    float GetValue();
    void SetValue(float value);
}

public class ComparableHeapNode<T> : Iheapable , System.IEquatable<ComparableHeapNode<T>>
{
    public T data_;
    private float priority_;
    public ComparableHeapNode(T data, float priority)
    {
        data_ = data;
        priority_ = priority;
    }

    public virtual int CompareTo(Iheapable other)
    {
        if (other == null) return 1; // 1 greater -1  smaller 0 equals 
        return this.GetValue().CompareTo(other.GetValue());
    }

    public bool Equals(ComparableHeapNode<T> other)
    {
        Debug.Log("compared");
        return data_.Equals(other.data_);
    }



    public virtual float GetValue()
    {
        return priority_;
    }

    public virtual void SetValue(float value)
    {
        priority_ = value;
    }
}