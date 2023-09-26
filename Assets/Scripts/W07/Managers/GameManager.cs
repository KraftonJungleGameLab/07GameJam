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
    [HideInInspector] private float _dieFdt;
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
        if(_isOnEnemy)
        {
            _dieFdt += Time.deltaTime;

            if (_dieFdt > _dieTime)
            {
                Debug.Log("PlayerDie!!");
            }
        }
        else
        {
            _dieFdt = 0;
        }
        
    }

    public void SetSavePoint(Vector3 savePosition)
    {
        savePoint = savePosition;
    }
}
