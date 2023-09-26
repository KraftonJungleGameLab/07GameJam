using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private PlayerBehavior _player;

    public Image _coolTimeImage;

    public TextMeshProUGUI _coolTimeText;

    public GameObject _pause;

    private float _maxCT;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _player = FindObjectOfType<PlayerBehavior>();
        _maxCT = _player._skillMaxCT;
    }

    public void UpdateCoolTime(float _fdt)
    {
        _coolTimeText.gameObject.SetActive(true);
        _coolTimeImage.fillAmount = 1f - _fdt / _maxCT;
        _coolTimeText.text = (_maxCT - _fdt).ToString("N1");
        if(_fdt > _maxCT)
        {
            _coolTimeImage.fillAmount = 0f;
            _coolTimeText.gameObject.SetActive(false);
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        _pause.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        _pause.SetActive(false);
    }

    public void RestartLevel()
    {
        Debug.Log("Restart");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif       
    }


}
