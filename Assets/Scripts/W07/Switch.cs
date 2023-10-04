using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private List <GameObject> _switchDoor;

    public void OpenDoor()
    {
        //if (_switchDoor.activeSelf)
        //{
        //    _switchDoor.SetActive(false);
        //    GameManager.Instance.ModifyMesh();
        //}
        //else
        //{
        //    _switchDoor.SetActive(true);
        //    GameManager.Instance.ModifyMesh();
        //}

        foreach(GameObject door in _switchDoor) 
        {
            door.SetActive(!door.activeSelf);
        }
        GameManager.Instance.ModifyMesh();
    }
}
