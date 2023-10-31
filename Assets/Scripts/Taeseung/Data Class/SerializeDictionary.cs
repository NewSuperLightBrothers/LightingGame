using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeDictionary<T1, T2> 
{
    [SerializeField] private List<T1> l_key;
    [SerializeField] private List<T2> l_value;

    private Dictionary<T1, T2> _dict = new();



    public void InitializeList()
    {
        _dict = new();
        if (l_key == null)     l_key = new();
        if (l_value == null)  l_value = new();


        if (l_key.Count == l_value.Count)
        {
            int length = l_key.Count;
            for (int i = 0; i < length; i++)
            {
                _dict.Add(l_key[i], l_value[i]);
            }
        }
        else
            throw new System.NullReferenceException();
    }



    public void SetValue
        (T1 key, T2 value){
        _dict[key] = value;
    }

    public T2 GetValue(T1 key) => _dict[key];

    public bool TryGetValues(T1 key, T2 value) => _dict.TryGetValue(key, out value);

    public List<T2> Getvalues() => l_value;

    public void AddValue(T1 key, T2 value)
    {
        if (!_dict.ContainsKey(key))
        {
            l_key.Add(key);
            l_value.Add(value);
            _dict.Add(key, value);
        }
    }

    public void RemoveValue(T1 key)
    {
        if (!_dict.ContainsKey(key))
        {
            _dict.Remove(key);
            int removeidx = l_key.FindIndex(data => data.ToString() == key.ToString());
            l_key.RemoveAt(removeidx);
            l_value.RemoveAt(removeidx);
        }
    }


}

