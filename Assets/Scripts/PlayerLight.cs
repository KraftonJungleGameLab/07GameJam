using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Vector3 _basicVector3;
    [SerializeField] private float _basicRange;
    [SerializeField] private float _maxFdt;
    [SerializeField] private float _basicIntensity;
    [SerializeField] private float _fdt;
    NavMeshModifierVolume _navMeshDistance;
    Light _playerLight;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshDistance = GetComponent<NavMeshModifierVolume>();
        _playerLight = GetComponent<Light>();
        _navMeshDistance.size = _basicVector3;
        _playerLight.range = _basicRange;
        _playerLight.intensity = _basicIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        _fdt += Time.deltaTime;
        _playerLight.range = _basicRange - _fdt * (_basicRange / (_maxFdt * 2f));
        _navMeshDistance.size = new Vector3(_basicVector3.x - _fdt * (_basicVector3.x / _maxFdt), _basicVector3.y, _basicVector3.z - _fdt * (_basicVector3.x / _maxFdt));
        _playerLight.intensity = _basicIntensity - _fdt * (_basicIntensity / (_maxFdt * 2f));
        if(_fdt > _maxFdt )
        {
            _navMeshDistance.gameObject.SetActive(false);
        }
    }
}
