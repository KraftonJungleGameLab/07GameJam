using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private GameObject _moveObject;
    [SerializeField] private Transform _point;
    private bool _isMoveOn = false;

    private void Update()
    {
        if(_isMoveOn) 
        {
            _moveObject.gameObject.transform.position = Vector3.MoveTowards(_moveObject.gameObject.transform.position, _point.position, 0.02f);
            if (Vector2.Distance(_moveObject.gameObject.transform.position, _point.position) < 0.1f)
            {
                _moveObject.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _isMoveOn = true;
        }
    }
}
