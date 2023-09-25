using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _nextScene;
    public GameObject _menu;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pause.SetActive(true);
        _nextScene.SetActive(false);
    }

    public void Pause()
    {
        _pause.SetActive(true);
    }

    public void StartGame()
    {
        _pause.SetActive(false);
    }

    public void NextSceneOn()
    {
        _nextScene.SetActive(true);
    }

    public void MenuOn()
    {
        GameManager.instance.TimeStop();
        _menu.SetActive(true);
    }

    public void MenuOff()
    {
        GameManager.instance.TimeGo();
        _menu.SetActive(false);
    }


    public void gameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif       
    }
}
