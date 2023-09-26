using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private GameObject _lightArea;
    [SerializeField] private HardLight2D _playerLight;
    [SerializeField] private float _basicLightRange;
    [SerializeField] private float _basicShieldRange;
    [SerializeField] private float _maxFdt;
    [SerializeField] private float _basicIntensity;
    [SerializeField] private float _fdt;
    //[SerializeField] private float _maxAngularSpeed;

    Vector2 targetDirection;

    void OnEnable()
    {
        ResetLight();
    }

    // Update is called once per frame
    void Update()
    {
        _fdt += Time.deltaTime;

        _playerLight.Range = _basicLightRange - _fdt * (_basicLightRange / (_maxFdt * 2f));
        _lightArea.transform.localScale = new Vector3(_basicShieldRange - _fdt * (_basicShieldRange / (_maxFdt * 2f)), _basicShieldRange - _fdt * (_basicShieldRange / (_maxFdt * 2f)), 1);
        _playerLight.Intensity = _basicIntensity - _fdt * (_basicIntensity / (_maxFdt * 3f));

        if(_fdt > _maxFdt)
        {
            _playerLight.Range = 0.1f;
            _lightArea.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            _playerLight.Intensity = 0.1f;
        }
        /*
        if (!targetDirection.Equals(Vector3.zero))
        {
            var eulerZ = _playerLight.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            var currentDirection = Vector2.right * -Mathf.Sin(eulerZ) + Vector2.up * Mathf.Cos(eulerZ);
            var direction = Vector3.RotateTowards(currentDirection, targetDirection, _maxAngularSpeed * Time.deltaTime, 0);
            _playerLight.transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
            Debug.Log($"{currentDirection}, {eulerZ}, {targetDirection}, {direction}");
        }*/
        
    }

    public void ResetLight()
    {
        _fdt = 0;
        _playerLight.Range = _basicLightRange;
        _playerLight.Intensity = _basicIntensity;
        _lightArea.transform.localScale = new Vector3(_basicShieldRange, _basicShieldRange, 1);
    }
    /*
    public void SetTargetDirection(Vector2 _direction)
    {
        targetDirection = _direction;
    }*/
}
