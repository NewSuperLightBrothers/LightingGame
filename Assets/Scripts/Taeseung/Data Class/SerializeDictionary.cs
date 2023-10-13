using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeDictionary<T1, T2>
{
    [SerializeField] private List<T1> _keys;
    [SerializeField] private List<T2> _values;

    private Dictionary<T1, T2> _dict = new();

    public void InitializeList()
    {
        _dict = new();
        if (_keys == null)     _keys = new();
        if (_values == null)  _values = new();


        if (_keys.Count == _values.Count)
        {
            int length = _keys.Count;
            for (int i = 0; i < length; i++)
            {
                _dict.Add(_keys[i], _values[i]);
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

    public List<T2> Getvalues() => _values;

    public void AddValue(T1 key, T2 value)
    {
        if (!_dict.ContainsKey(key))
        {
            _keys.Add(key);
            _values.Add(value);
            _dict.Add(key, value);
        }
    }

    public void RemoveValue(T1 key)
    {
        if (!_dict.ContainsKey(key))
        {
            _dict.Remove(key);
            int removeidx = _keys.FindIndex(data => data.ToString() == key.ToString());
            _keys.RemoveAt(removeidx);
            _values.RemoveAt(removeidx);
        }
    }


}

