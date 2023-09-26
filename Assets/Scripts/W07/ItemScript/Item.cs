using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum Key
    {
        red,
        green
    }

    [Header("Item spec")]
    public bool _isKey;
    public Key _keyValue;

    public string _itemName;
    public Sprite _itemImage;
    public Color _itemColor;

    public Item(Item item)
    {
        this._isKey = item._isKey;
        this._keyValue = item._keyValue;
        this._itemName = item._itemName;
        this._itemImage = item._itemImage;
        this._itemColor = item._itemColor;
    }
}
