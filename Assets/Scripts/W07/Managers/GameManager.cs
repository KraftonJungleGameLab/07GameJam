using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private float _dieTime;
    private float _dieFdt;
    [HideInInspector] public bool _isOnEnemy;
    [HideInInspector] public Vector3 savePoint;
    public PlayerBehavior player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        savePoint = player.transform.position;
    }

    void Update()
    {
        if (_dieFdt > _dieTime)
        {
            Debug.Log("PlayerDie!!");
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
        savePoint = savePosition;
    }
}
