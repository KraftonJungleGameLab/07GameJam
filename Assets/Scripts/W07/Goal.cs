using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);
        
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider !=null && hit.collider.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.NextScene();
            }
        }
    }
}
