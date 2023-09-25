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

}
