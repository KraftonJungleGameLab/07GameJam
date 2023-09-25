using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject _trash;

    public void ActiveTrashIcon()
    {
        _trash.SetActive(!_trash.activeSelf);
        Debug.Log("TrashIconActive");
    }
}
