using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private PlayerBehavior _playerBehavior;
    private bool _isPlayerDetected = false;
    private NavMeshAgent _agent;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _bodyRange;

    private Vector3 _startPos;
    private bool _isPlayerDie;

    // Start is called before the first frame update
    void Start()
    {
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
        // 적과 플레이어 사이에 장애물이 있는지 검사
        if (Physics2D.Linecast(transform.position, _playerBehavior.gameObject.transform.position, LayerMask.GetMask("Obstacle")))
        {
            _isPlayerDetected = false;
        }
        else
        {
            _isPlayerDetected = true;
        }

        if (_isPlayerDetected)
        {
            _agent.SetDestination(_playerBehavior.gameObject.transform.position);
            particle.transform.position = this.transform.position;
            if (!particle.isPlaying)
            {
                particle.Play();
            }

            //Debug.Log("PlayerDetected bool" + _isPlayerDetected);
            //Debug.Log(particle.transform.position);
        }
        else
        {
            _detectRange = 20f;
            _agent.SetDestination(_startPos);
            _agent.speed = 5;
            if (particle.isPlaying)
                particle.Stop();
        }

        RaycastHit2D[] bodyhits = Physics2D.CircleCastAll(this.transform.position, _bodyRange, Vector2.zero);
        foreach (RaycastHit2D hit in bodyhits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                GameManager.Instance._isOnEnemy = true;
                return;
            }
        }
        GameManager.Instance._isOnEnemy = false;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, _detectRange, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _detectRange = 20f;
                _isPlayerDetected = true;
                _agent.speed = 8;
                return;
            }
        }
        _isPlayerDetected = false;
    }

    IEnumerator PlayerDie()
    {
        if (!_isPlayerDie)
        {
            Debug.Log("PlayerDie");
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
