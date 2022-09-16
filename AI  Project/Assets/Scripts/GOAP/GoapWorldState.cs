using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hacky implementation of HashSet with ability to get element via key
using StateKey = System.String;
using StateValue = System.Object;

public class GoapWorldState : ISet<(StateKey, StateValue)>
{
    private HashSet<(StateKey, StateValue)> StateSet;
    private Dictionary<StateKey, StateValue> RefMap;
    public ICollection Keys => RefMap.Keys;
    //public ICollection Values => RefMap.Values;
    public GoapWorldState()
    {
        StateSet = new HashSet<(StateKey, StateValue)>();
        RefMap = new Dictionary<StateKey, StateValue>();
    }
    public GoapWorldState(GoapWorldState refState)
    {
        if (refState == null) return;
        StateSet = refState.StateSet;
        RefMap = refState.RefMap;
    }

    public GoapWorldState(IEnumerable<(StateKey,StateValue)> collection)
    {
        StateSet = new HashSet<(StateKey, StateValue)>();
        RefMap = new Dictionary<StateKey, StateValue>();
        foreach (var data in collection)
        {
            this.Add(data);
        }
    }
    public int Count => StateSet.Count;
    public bool IsReadOnly => true;
    public bool SetValue(StateKey key, StateValue value)
    {
        var success =  StateSet.Add((key, value));
        if (success) RefMap.Add(key, value);
        return success;
    }

    public bool Add(StateKey key, StateValue value)
    {
        var success = StateSet.Add((key, value));
        if(success) RefMap.Add(key, value);
        return success;
    }

    public bool Add((StateKey, StateValue) data)
    {
        return this.Add(data.Item1, data.Item2);
    }

    public bool Remove((StateKey, StateValue) item)
    {
        var success = StateSet.Remove(item);
        if(success) RefMap.Remove(item.Item1);
        return success;
    }

    public void Clear() {
        StateSet.Clear();
        RefMap.Clear();
    }

    public bool TryFind(StateKey key,out StateValue value)
    {
        if (RefMap.ContainsKey(key))
        {
            value = RefMap[key];
            return true;
        }
        value = null;
        return false;
    }

    public void UpdateValue(StateKey key , StateValue value , bool addIfNew =true)
    {
        object prevVal = null;
        if (TryFind(key, out prevVal))
        {
            this.Remove((key, prevVal));
            this.Add((key, value));
        }
        else if (addIfNew)
        {
            this.Add((key, value));
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    void ICollection<(StateKey, StateValue)>.Add((StateKey, StateValue) item)
    {
        throw new NotImplementedException();
    }



    bool ISet<(StateKey, StateValue)>.Add((StateKey, StateValue) item) => this.Add(item);
    public bool Contains((StateKey, StateValue) item) => StateSet.Contains(item);
    public void CopyTo((StateKey, StateValue)[] array, int arrayIndex) => StateSet.CopyTo(array, arrayIndex);
    public void ExceptWith(IEnumerable<(StateKey, StateValue)> other) => StateSet.ExceptWith(other);
    public void IntersectWith(IEnumerable<(StateKey, StateValue)> other) => StateSet.ExceptWith(other);
    public bool IsProperSubsetOf(IEnumerable<(StateKey, StateValue)> other) => StateSet.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<(StateKey, StateValue)> other) => StateSet.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<(StateKey, StateValue)> other) => StateSet.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<(StateKey, StateValue)> other) => StateSet.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<(StateKey, StateValue)> other) => StateSet.Overlaps(other);
    public bool SetEquals(IEnumerable<(StateKey, StateValue)> other) => StateSet.SetEquals(other);
    public void SymmetricExceptWith(IEnumerable<(StateKey, StateValue)> other) => StateSet.SymmetricExceptWith(other);
    public void UnionWith(IEnumerable<(StateKey, StateValue)> other) => StateSet.UnionWith(other);
    IEnumerator<(StateKey, StateValue)> IEnumerable<(StateKey, StateValue)>.GetEnumerator() => StateSet.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => StateSet.GetEnumerator();
}
