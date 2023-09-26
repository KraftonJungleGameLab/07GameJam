using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private PlayerBehavior _playerBehavior;
    private bool _isPlayerDetected = false;
    private NavMeshAgent _agent;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _bodyRange;

    private Vector3 _startPos;

    private bool _isPlayerDie;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _isPlayerDie = false;
        _playerBehavior = FindAnyObjectByType<PlayerBehavior>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPlayerDetected) 
        {
            _agent.SetDestination(_playerBehavior.gameObject.transform.position);
        }

        else
        {
            _detectRange = 5f;
            _agent.SetDestination(_startPos);
            _agent.speed = 5;
        }


        RaycastHit2D[] bodyhits = Physics2D.CircleCastAll(this.transform.position, _bodyRange, Vector2.zero);
        foreach (RaycastHit2D hit in bodyhits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player") && !GameManager.instance._isGoalActive)
            {
                //StartCoroutine(PlayerDie());
                //return;
            }
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, _detectRange, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null & hit.collider.CompareTag("Player"))
            {
                _detectRange = 10f;
                _isPlayerDetected = true;
                _agent.speed = 8;
                return;
            }
        }
        _isPlayerDetected = false;
    }

    IEnumerator PlayerDie()
    {
        if(!_isPlayerDie )
        {
            _isPlayerDie = true;
            //SoundManager.instance.StopBGM();
            //SoundManager.instance.PlaySE("Die");
            //UIManager.Instance.NextSceneOn();
        }      
        yield return new WaitForSeconds(2f);
        //LevelManager.instance.ReloadScene();
    }


    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _bodyRange);
    }
}
