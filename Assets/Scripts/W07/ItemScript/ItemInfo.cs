using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour, IObjectItem
{
    [SerializeField]private Item _item;

    private SpriteRenderer _spriteRenderer;

    public Item GetItem()
    {
        return this._item;
    }

    public void SetItem(Item item)
    {
        this._item = item;
        _spriteRenderer.sprite = _item._itemImage;
        _spriteRenderer.color = _item._itemColor;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetItem(this._item);
    }
}
