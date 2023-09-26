using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public Vector3 savePosition;
    public BoxCollider stageBlockCollider;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (savePosition == Vector3.zero)
                GameManager.Instance.SetSavePoint(other.transform.position);
            else
                GameManager.Instance.SetSavePoint(savePosition);

            stageBlockCollider.enabled = true;
        }
    }
}