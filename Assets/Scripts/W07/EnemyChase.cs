using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Patrol,
        Return
    }
    [SerializeField] private GameObject patrolPosition1;
    [SerializeField] private GameObject patrolPosition2;
    private bool isPatroling1;
    private Vector3 targetPosition;

    [SerializeField] EnemyState currentState = EnemyState.Patrol;


    [SerializeField] private bool _isPlayerDetected = false;
    private PlayerBehavior _playerBehavior;
    private NavMeshAgent _agent;

    [SerializeField] private float _enemyBasicSpeed;
    [SerializeField] private float _enemyChaseSpeed;

    [SerializeField] private ParticleSystem particle;
    private float _basicRange;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _bodyRange;

    [SerializeField] private bool _isParticleOn;

    private Vector3 _startPos;
    private bool _isPlayerDie;

    private float _maxDistance = 17.5f;

    // Start is called before the first frame update
    void Start()
    {
        _basicRange = _detectRange;
        _isParticleOn = false;
        _startPos = transform.position;
        _isPlayerDie = false;
        _playerBehavior = FindObjectOfType<PlayerBehavior>(); // FindAnyObjectByType 대신 FindObjectOfType 사용
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
            case EnemyState.Chase:
                UpdateChase();
                break;
            case EnemyState.Return:
                UpdateReturn();
                break;
            default:
                break;
        }
    }

    private void UpdatePatrol()
    {
        LookForTargets();

        if (Vector2.Distance(transform.position, _startPos) > _maxDistance)
        {
            _agent.SetDestination(_startPos);
            _isPlayerDetected = false;
            currentState = EnemyState.Return;
        }

        if(Vector2.Distance(transform.position, _playerBehavior.transform.position) < _detectRange)
        {
            currentState = EnemyState.Chase;
            _isPlayerDetected = true;
        }
    }
    private void LookForTargets()
    {
        if (isPatroling1)
        {
            _agent.SetDestination(patrolPosition2.transform.position);
            float distance = Vector3.Distance(transform.position, patrolPosition2.transform.position);
            if (distance < 0.1f)
            {
                isPatroling1 = false;
            }
        }
        else
        {
            _agent.SetDestination(patrolPosition1.transform.position);
            float distance = Vector3.Distance(transform.position, patrolPosition1.transform.position);
            if (distance < 0.1f)
            {
                isPatroling1 = true;
            }
        }
    }
    private void UpdateChase()
    {
        if (Physics2D.Linecast(transform.position, _playerBehavior.gameObject.transform.position, LayerMask.GetMask("Obstacle")) || (Vector3.Distance(transform.position, targetPosition) < 0.1f))
        {
            currentState = EnemyState.Return;
        }
        else
        {
            targetPosition = _playerBehavior.gameObject.transform.position;
        }

        if (_isPlayerDetected)
        {
            _agent.SetDestination(targetPosition);
            particle.transform.position = this.transform.position;
            if (!particle.isPlaying && !_isParticleOn)
            {
                particle.Play();
                _isParticleOn = true;
            }
        }
        else
        {
            currentState = EnemyState.Return;
        }
    }

    private void UpdateReturn()
    {
        _isParticleOn = false;
        _agent.SetDestination(_startPos);
        if (Vector3.Distance(transform.position, _startPos) < 0.1f)
            currentState = EnemyState.Patrol;
        _agent.speed = 5;
        _detectRange = _basicRange;
        if (particle.isPlaying)
            particle.Stop();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _bodyRange);
    }
}
