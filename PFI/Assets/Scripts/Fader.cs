using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private float sceneLoadFadeIn = 1f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (sceneLoadFadeIn > 0f)
        {
            canvasGroup.alpha = 1f;
            StartCoroutine(FadeIn(sceneLoadFadeIn));
        }
    }

    public void FadeOutImmidiate() => canvasGroup.alpha = 1f;

    public IEnumerator FadeOut(float time, bool freezeTime = false)
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }

        if (freezeTime)
            Time.timeScale = 0f;
    }

    public IEnumerator FadeIn(float time, bool freezeTime = false)
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        
        if (freezeTime)
            Time.timeScale = 0f;
    }
}