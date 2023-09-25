using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private Camera _mainCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void NextStage(Vector3 pos)
    {
        StartCoroutine(CameraMove(pos));
    }

    IEnumerator CameraMove(Vector3 pos)
    {
        _mainCamera.transform.position = Vector3.Lerp(transform.position ,pos + new Vector3(0, 0, -10), Time.deltaTime * 5f);
        yield return new WaitForSeconds(2f);
        GameManager.instance.StageStart();
    }
}
