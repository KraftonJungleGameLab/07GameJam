using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;

    [SerializeField] private GameObject _targetPref;
    [SerializeField] private Transform _targetParent;

    public List<GameObject> _targets = new List<GameObject>();

    public bool _playerCanGo;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _playerCanGo = false;
    }

    public void GenerateTarget(Vector3 pos)
    {
        _playerCanGo = true;
        GameObject temp = Instantiate(_targetPref, _targetParent);
        temp.transform.position = pos;
        _targets.Add(temp);
    }

    public void RemoveAllTarget()
    {
        /*
        Transform[] childList = _targetParent.GetComponentsInChildren<Transform>();

        if(childList != null)
        {
            for(int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform) 
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }
        */

        for(int i = 0; i < _targets.Count; i++) 
        {
            Destroy(_targets[i].gameObject);
        }
        _targets.Clear();
        _playerCanGo = false;
    }
    
    public void SelectTargetAct(GameObject target)
    {
        //target.transform.position = mousePos;
        
        Target _targetscript = target.GetComponent<Target>();
        _targetscript.ActiveTrashIcon();
        
    }
    
    public void RemoveTarget(GameObject target)
    {
        if(target == _targets[0])
        {
            GameManager.instance.RemoveAllArrow();
        }

        _targets.Remove(target);
        Destroy(target);
        
        if (_targets.Count == 0)
        {
            _playerCanGo = false;
        }
    }
}
