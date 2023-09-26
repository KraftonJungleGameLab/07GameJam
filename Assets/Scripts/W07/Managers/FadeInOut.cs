using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public void FadeIn(float fadeOutTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(CoFadeIn(fadeOutTime));
    }

    public void FadeOut(float fadeOutTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(CoFadeOut(fadeOutTime));
    }

    // 투명 -> 불투명
    IEnumerator CoFadeIn(float fadeOutTime)
    {
        Image sr = this.gameObject.GetComponent<Image>();
        Color tempColor = sr.color;
        tempColor.a = 0.0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            sr.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        sr.color = tempColor;
    }

    IEnumerator CoFadeOut(float fadeOutTime)
    {
        Image sr = this.gameObject.GetComponent<Image>();
        Color tempColor = sr.color;
        tempColor.a = 1.0f;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            sr.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        sr.color = tempColor;
    }
}