using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColorSwitch : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Color _orginColor;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _orginColor = _sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Light"))
        {
            _sprite.color = Color.blue;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            _sprite.color = _orginColor;
        }
    }
}
