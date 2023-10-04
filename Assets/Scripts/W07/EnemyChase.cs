using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Patrol,
        Return,
        Wait
    }
    [SerializeField] private bool _isInfChaseEnemy;

    [SerializeField] private GameObject patrolPosition1;
    [SerializeField] private GameObject patrolPosition2;
    [SerializeField] private bool isPatrol = false;
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
        _playerBehavior = GameManager.Instance.player; // FindAnyObjectByType ��� FindObjectOfType ���
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _enemyBasicSpeed;
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInfChaseEnemy)
        {
            InfChase();
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Light"))
            {
                if (currentState == EnemyState.Patrol || currentState == EnemyState.Wait || currentState == EnemyState.Return)
                {
                    _agent.speed = _enemyBasicSpeed - 1;
                }

                else if (currentState == EnemyState.Chase)
                {
                    _agent.speed = _enemyChaseSpeed - 1;
                }
            }
        }

        if (_isInfChaseEnemy)
        {
            return;
        }

        if (isPatrol)
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
        else
        {
            switch (currentState)
            {
                case EnemyState.Wait:
                    LookForTargets();
                    break;
                case EnemyState.Chase:
                    _agent.speed = _enemyChaseSpeed;
                    UpdateChase();
                    break;
                case EnemyState.Return:
                    _agent.speed = _enemyBasicSpeed;
                    UpdateReturn();
                    break;
                default:
                    break;
            }
        }
    }

    private void InfChase()
    {
        if (gameObject.activeSelf)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            targetPosition = _playerBehavior.transform.position;
            _agent.SetDestination(targetPosition);
        }
    }

    private void UpdatePatrol()
    {
        LookForTargets();
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
    private void LookForTargets()
    {
        if (Vector2.Distance(transform.position, _playerBehavior.transform.position) < _detectRange)
        {
            if (!Physics2D.Linecast(transform.position, _playerBehavior.gameObject.transform.position, LayerMask.GetMask("Obstacle")))
            {
                currentState = EnemyState.Chase;
                targetPosition = _playerBehavior.transform.position;
                Debug.Log($"TargetDetect : {targetPosition}");
                _isPlayerDetected = true;
                return;
            }
        }
        _isPlayerDetected = false;

    }
    private void UpdateChase()
    {
        
        if (Vector2.Distance(transform.position, _startPos) > _maxDistance)
        {
            _agent.SetDestination(_startPos);
            _isPlayerDetected = false;
            currentState = EnemyState.Return;
            return;
        }

        if (_isPlayerDetected)
        {
            LookForTargets();
            _agent.SetDestination(targetPosition);
            particle.transform.position = this.transform.position;
            if (!particle.isPlaying && !_isParticleOn)
            {
                particle.Play();
                _isParticleOn = true;
            }
            
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                LookForTargets();
                //_isPlayerDetected = false;
            }
        }

        else
        {
            _agent.SetDestination(targetPosition);
            if (Vector2.Distance(transform.position, targetPosition) < 1.0f || 
                Physics2D.Linecast(transform.position, targetPosition, LayerMask.GetMask("Obstacle")))
            {
                currentState = EnemyState.Return;
                Debug.Log("Return");
            }       
            //         
        }
    }

    private void UpdateReturn()
    {
        _isParticleOn = false;
        _agent.SetDestination(_startPos);
        LookForTargets();
        if (Vector3.Distance(transform.position, _startPos) < 0.1f)
            if (isPatrol)
            {
                currentState = EnemyState.Patrol;
            }
            else
            {
                currentState = EnemyState.Wait;
            }
        _agent.speed = _enemyBasicSpeed;
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
