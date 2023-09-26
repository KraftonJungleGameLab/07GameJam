using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBake : MonoBehaviour
{
    public static TestBake Instance;

    private NavMeshSurface surface;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        StartCoroutine(Bake());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //surface.BuildNavMesh();
    }

    IEnumerator Bake()
    {
        surface.BuildNavMesh();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Bake());
    }

}
