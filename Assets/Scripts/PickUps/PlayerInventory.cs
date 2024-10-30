using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<EItems, bool> _items=new Dictionary<EItems, bool>();


    private void Awake()
    {
         
        _items.Add(EItems.Lever, false);
    }
    public void AddItem(EItems item) 
    {
        if (_items.ContainsKey(item))
            _items[item] = true;
    }
    public void DeleteItem(EItems item) 
    {
        if(_items.ContainsKey(item))
            _items[item] = false;
    }
    public bool CheckItem(EItems item) 
    {
        return _items.ContainsKey(item) && _items[item];
    }
}
public enum EItems 
{
    Lever
}
