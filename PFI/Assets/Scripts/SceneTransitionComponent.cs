
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionComponent : MonoBehaviour
{
    public int requiredLetters;
    public int pickedLetters;
    private Fader fader;
    private Image ninjaDialog;

    private void Awake()
    {
        fader = GameObject.Find("Fader").GetComponent<Fader>();
        ninjaDialog = GameObject.Find("NinjaDialog").GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Equals("Player"))
            return;

        if (requiredLetters > 0)
        {
            if (pickedLetters >= requiredLetters)
                StartCoroutine(LoadNextScene());
            else
            {
                ninjaDialog.enabled = true;
                Time.timeScale = 0f;
            }
        }
        else
            StartCoroutine(LoadNextScene());
    }

    private void Update()
    {
        if (ninjaDialog.enabled && Input.GetKeyDown(KeyCode.Escape))
        {
            ninjaDialog.enabled = false;
            Time.timeScale = 1f;
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return fader.FadeOut(1);
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;

            yield return null;
        }
    }
}
