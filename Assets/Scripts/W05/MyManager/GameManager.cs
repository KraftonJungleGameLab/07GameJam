using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool _isGoalActive = false;

    public bool _isFirstTurn = true;

    private bool isSpaceEnter = true;
    private bool _isRightClickOn = false;
    [SerializeField] private GameObject _arrowObject;

    private GameObject _currentArrow;
    [SerializeField] private Transform _arrowParent;

    private Vector3 _initialMousePosition;

    public List<GameObject> _arrows = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!UIManager.Instance._menu.activeSelf) 
            { 
                UIManager.Instance.MenuOn(); 
            }

            else UIManager.Instance.MenuOff();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _initialMousePosition = mousePos;

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    return;
                }
            }
            TargetManager.instance.GenerateTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10)));
        }
        if (Input.GetKeyDown(KeyCode.R))
        { //Test Remove All
            TargetManager.instance.RemoveAllTarget();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !_isRightClickOn)
        {
            if (Time.timeScale > 0)
            {
                isSpaceEnter = true;
                TimeStop();
            }

            else
            {
                isSpaceEnter = false;
                TimeGo();
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            InputRightClick();
        }

        if (_isRightClickOn)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 현재 마우스 위치와 초기 마우스 위치를 비교하여 회전값을 계산합니다.
            Vector3 mouseDelta = (mousePos + new Vector3(0, 0, 10)) - _initialMousePosition;
            float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg - 90;

            // Arrow의 회전을 설정합니다.
            _currentArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (Input.GetMouseButtonUp(1)) // 마우스 왼쪽 버튼을 놓았을 때
            {
                if (!isSpaceEnter) TimeGo();
                _isRightClickOn = false;
            }
        }

        /*
        if(Input.GetMouseButton(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _initialMousePosition = mousePos;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(mousePos, 0.16f, Vector2.up);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Target"))
                {
                    
                    _isRightClickOn = false;
                    return;
                }
            }
        }*/

    }

    public void InputRightClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _initialMousePosition = mousePos;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mousePos, 0.2f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            /*
            if (_isTargetRepos)
            {
                TargetManager.instance.SelectTargetAct(hit.collider.gameObject);
                return;
            }*/

            if (hit.collider != null && hit.collider.CompareTag("Line"))
            {
                TimeStop();
                InitArrow(mousePos);
            }

            if (hit.collider != null && hit.collider.CompareTag("Target"))
            {
                //TargetManager.instance.SelectTargetAct(hit.collider.gameObject, mousePos);
                //TargetManager.instance.RemoveTarget(hit.collider.gameObject);
                _isRightClickOn = false;
                TargetManager.instance.SelectTargetAct(hit.collider.gameObject);
                return;
            }



            if (hit.collider != null && hit.collider.CompareTag("Trash"))
            {
                TargetManager.instance.RemoveTarget(hit.collider.transform.parent.gameObject);
                return;
            }

        }
    }

    public void DragRightClick()
    {
        _isRightClickOn = true;

        //InitArrow();
        Debug.Log("Line hit!");
        return;
    }

    private void InitArrow(Vector2 mousePos)
    {
        _isRightClickOn = true;

        // Arrow를 생성하고 초기 위치를 설정합니다.
        _currentArrow = Instantiate(_arrowObject, _arrowParent);
        _arrows.Add(_currentArrow);
        _initialMousePosition = mousePos;
        _currentArrow.transform.position = _initialMousePosition + new Vector3(0, 0, -0.08333216f);

        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject temp = Instantiate(_arrowObject, mousePos + new Vector3(0,0,10), _arrowObject.transform.rotation);

        Vector3 vectorToTarget = mousePos - temp.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        temp.transform.rotation = Quaternion.Slerp(temp.transform.rotation, q, Time.deltaTime * 50);
        */
    }

    public void RemoveArrow(GameObject arrow)
    {
        _arrows.Remove(arrow);
        Destroy(arrow);
    }

    public void RemoveAllArrow()
    {
        for (int i = 0; i < _arrows.Count; i++)
        {
            Destroy(_arrows[i].gameObject);
        }
        _arrows.Clear();
    }

    public void TimeStop()
    {
        UIManager.Instance.Pause();
        Time.timeScale = 0f;
    }

    public void TimeGo()
    {
        UIManager.Instance.StartGame();
        Time.timeScale = 1f;
    }

    public void StageStart()
    {
        _isFirstTurn = true;
        isSpaceEnter = true;
        _isRightClickOn = false;
        TimeStop();
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_initialMousePosition, 0.05f);
    }
}
