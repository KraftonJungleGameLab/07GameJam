using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject _switchDoor;

    public void OpenDoor()
    {
        _switchDoor.SetActive(false);
    }
}
