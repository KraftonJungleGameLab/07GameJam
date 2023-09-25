using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Item _item;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _item = null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckItem(ItemInfo item)
    {
        if (_item != null) 
        {
                   
        }

        else _item = item.GetItem();
    }

}
