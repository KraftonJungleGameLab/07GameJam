using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _lightObject;
    [SerializeField] private GameObject _keyEquipUI;

    private NavMeshAgent _agent;
    private bool _isLightDirModify;
    [SerializeField] private float _rotationModifier;
    [SerializeField] private float _speedModifier;

    [SerializeField] private float _rotateSpeed;

    private Quaternion _arrowRotation;

    private bool _isArrowTrigger;
    private bool _isKeyEquip;
    private bool _isWalkSoundPlay = false;

    [SerializeField] private float _fdt = 0;
    [SerializeField] private float _maxFdt;
    private Vector3 _lastTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        _isKeyEquip = false;
        _keyEquipUI.SetActive(false);
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, Vector2.up, 0.15f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null & hit.collider.CompareTag("Arrow"))
            {
                Debug.Log("Arrow");
                _fdt = 0;
                _isArrowTrigger = true;
                _arrowRotation = hit.collider.transform.rotation;
                //_lightObject.transform.rotation = hit.collider.transform.rotation;
                GameManager.instance.RemoveArrow(hit.collider.gameObject);
            }

            if (hit.collider != null && hit.collider.CompareTag("Key"))
            {
                hit.collider.gameObject.SetActive(false);
                _keyEquipUI.SetActive(true);
                _isKeyEquip = true;
                SoundManager.instance.PlaySE("Key");
            }

            if (hit.collider != null && hit.collider.CompareTag("OpenZone") && _isKeyEquip)
            {
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                _isKeyEquip = false;
                _keyEquipUI.SetActive(_isKeyEquip);
                SoundManager.instance.PlaySE("OpenDoor");
            }

            if (hit.collider != null && hit.collider.CompareTag("Goal") && !GameManager.instance._isGoalActive)
            {
                StartCoroutine(NextScene());
            }

        }


        if (_isArrowTrigger)
        {
            _fdt += Time.deltaTime;
            _lightObject.transform.rotation = Quaternion.Slerp(_lightObject.transform.rotation, _arrowRotation, Time.deltaTime * _speedModifier);
        }

        if (_fdt >= _maxFdt)
        {
            RotateLightBasic(_lastTargetPos);
            _isArrowTrigger = false;
        }

        if (TargetManager.instance._playerCanGo)
        {
            GameObject target = TargetManager.instance._targets[0];
            _lastTargetPos = target.transform.position;
            //_agent.SetDestination(target.transform.position);
            if (!_isArrowTrigger) RotateLightBasic(target.transform.position);

            LineManager.instance.makePath(target);
            if (!_isWalkSoundPlay)
            {
                StartCoroutine(StartWalk());
            }
            if (Vector2.Distance(transform.position, target.transform.position) < 0.5f)
            {
                TargetManager.instance.RemoveTarget(target);
                GameManager.instance.RemoveAllArrow();
            }
        }

        else if (!TargetManager.instance._playerCanGo)
        {
            if (_isWalkSoundPlay)
            {
                StopCoroutine(StartWalk());
                SoundManager.instance.StopSE("Walk");
                _isWalkSoundPlay = false;
            }
            _agent.SetDestination(transform.position);
        }

    }

    IEnumerator NextScene()
    {
        GameManager.instance._isGoalActive = true;
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE("NextStage");
        UIManager.Instance.NextSceneOn();
        yield return new WaitForSeconds(2f);
        LevelManager.instance.NextStage();
    }

    IEnumerator StartWalk()
    {
        if (!GameManager.instance._isFirstTurn)
        {
            _isWalkSoundPlay = true;
            SoundManager.instance.PlaySE("Walk");
            
        }
        yield return new WaitForSeconds(10f);
        if (_isWalkSoundPlay)
        {
            SoundManager.instance.StopSE("Walk");
            _isWalkSoundPlay = false;
        }
    }
    /*
    void RotateLightBasic(GameObject target)
    {
        Vector3 vectorToTarget = _lightObject.transform.position - target.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _lightObject.transform.rotation = Quaternion.Slerp(_lightObject.transform.rotation, q, Time.deltaTime * _speedModifier);
    }
    */
    void RotateLightBasic(Vector3 target)
    {
        Vector3 vectorToTarget = _lightObject.transform.position - target;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _lightObject.transform.rotation = Quaternion.Slerp(_lightObject.transform.rotation, q, Time.deltaTime * _speedModifier);
    }

    public void RotateLightPos(Vector2 mousePos)
    {
        _lightObject.transform.rotation = Quaternion.LookRotation(mousePos);
    }

}
