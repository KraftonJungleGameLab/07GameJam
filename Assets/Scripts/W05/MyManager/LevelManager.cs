using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static int _nowStage = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switch (_nowStage)
        {
            case 0:
                SoundManager.instance.PlayBGM("Intro");
                break;
            case 1:
                SoundManager.instance.PlayBGM("Intro");
                break;
            case 2:
                SoundManager.instance.PlayBGM("Intro");
                break;
            case 3:
                SoundManager.instance.PlayBGM("Stage1");
                SoundManager.instance.audioSourceBGM.volume = 0.5f;
                break;
            case 4:
                SoundManager.instance.PlayBGM("Stage3");
                SoundManager.instance.audioSourceBGM.volume = 0.5f;
                break;
            case 5:
                SoundManager.instance.PlayBGM("Restart");
                SoundManager.instance.audioSourceBGM.volume = 0.5f;
                break;
        }
    }

    public void NextStage()
    {
        _nowStage++;
        Debug.Log("NextScene");
        SceneManager.LoadScene(_nowStage);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
