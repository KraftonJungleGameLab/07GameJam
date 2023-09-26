using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private float _dieTime;
    public float _dieFdt;
    [HideInInspector] public bool _isOnEnemy;
    public static Vector3 _playerFirstPos;
    public static Vector3 savePoint;
    public static bool _isSaved = false;
    public PlayerBehavior player;

    [SerializeField] private NavMeshSurface _redSurface;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        _playerFirstPos = player.gameObject.transform.position;
        if(_isSaved) player.gameObject.transform.position = savePoint;
    }

    void Update()
    {
        if (_dieFdt > _dieTime)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            _dieFdt = 0;
        }
    }

    public void PlusDieFdt()
    {
        _dieFdt += Time.deltaTime;
    }

    public void ResetDieFdt()
    {
        _dieFdt = 0;
    }


    public void SetSavePoint(Vector3 savePosition)
    {
        _isSaved = true;
        savePoint = savePosition;
        Debug.Log(savePoint);
    }

    public void ResetSavePoint()
    {
        _isSaved = false;
        savePoint = Vector2.zero;
    }

    public void ModifyMesh()
    {
        _redSurface.BuildNavMesh();
    }

}
