using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour, IObjectItem
{
    [SerializeField]private Item _item;

    private Sprite _sprite;

    public Item GetItem()
    {
        return this._item;
    }

    public void SetItem(Item item)
    {
        this._item = item;
        _sprite = _item._itemImage;
    }

    private void Start()
    {
        SetItem(this._item);
    }
}
