using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(EdgeCollider2D))]
public class LineManager : MonoBehaviour
{
    public static LineManager instance;
    [SerializeField] private GameObject _player;
    private LineRenderer _lr;
    private NavMeshAgent _playerNav;

    private EdgeCollider2D edgeCollider;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {       
        _lr = this.GetComponent<LineRenderer>();
        _lr.startWidth = _lr.endWidth = 0.3f;
        _lr.material.color = Color.green;
        _lr.enabled = false;

        edgeCollider = this.GetComponent<EdgeCollider2D>();
        _playerNav = _player.GetComponent<NavMeshAgent>();
    }

    void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
    }

    public void makePath(GameObject target)
    {
        _lr.enabled = true;
        StartCoroutine(makePathCoroutine(target));
    }
    
    IEnumerator makePathCoroutine(GameObject target)
    {
        _playerNav.SetDestination(target.transform.position);
        _lr.SetPosition(0, _player.transform.position);
        //this.transform.position
        yield return new WaitForSeconds(0.1f);

        drawPath();
    }
    void drawPath()
    {
        int length = _playerNav.path.corners.Length;

        _lr.positionCount = length;
        for (int i = 1; i < length; i++)
            _lr.SetPosition(i, _playerNav.path.corners[i]);

        SetEdgeCollider(_lr);

        if(GameManager.instance._isFirstTurn) 
        {

            GameManager.instance.TimeStop();
            GameManager.instance._isFirstTurn = false;
            
        }
        
    }
    /*
    public void GenerateMeshCollider()
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();
        _lr.BakeMesh(mesh, true);

        // if you need collisions on both sides of the line, simply duplicate & flip facing the other direction!
        // This can be optimized to improve performance ;)
        int[] meshIndices = mesh.GetIndices(0);
        int[] newIndices = new int[meshIndices.Length * 2];

        int j = meshIndices.Length - 1;
        for (int i = 0; i < meshIndices.Length; i++)
        {
            newIndices[i] = meshIndices[i];
            newIndices[meshIndices.Length + i] = meshIndices[j];
        }
        mesh.SetIndices(newIndices, MeshTopology.Triangles, 0);

        collider.sharedMesh = mesh;
    }
    */

    /*
    public void makePath(GameObject target1, GameObject target2)
    {
        StartCoroutine(makePathCoroutine(target1,target2));
    }
    
    
    IEnumerator makePathCoroutine(GameObject target1, GameObject target2)
    {
        NavMeshAgent target1Nav = target1.GetComponent<NavMeshAgent>();
        target1Nav.SetDestination(target2.transform.position);
        _lr.SetPosition(0, target1Nav.transform.position);
        //this.transform.position
        yield return new WaitForSeconds(0.1f);

        drawPath();
    }
    
    
    void drawPath(NavMeshAgent targetNav)
    {
        int length = targetNav.path.corners.Length;

        _lr.positionCount = length;
        for (int i = 1; i < length; i++)
            _lr.SetPosition(i, _playerNav.path.corners[i]);
    }*/
}
