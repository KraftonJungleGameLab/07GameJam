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
    [HideInInspector] public static Vector3 savePoint;
    public PlayerBehavior player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        player.gameObject.transform.position = savePoint;
        savePoint = player.transform.position;
    }

    void Update()
    {
        if (_dieFdt > _dieTime)
        {
            player.transform.position = savePoint;
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
        savePoint = savePosition;
        Debug.Log(savePoint);
    }
}
