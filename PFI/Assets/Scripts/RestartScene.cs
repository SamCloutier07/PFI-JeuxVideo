
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private Fader fader;

    private void Awake()
    {
        fader = GameObject.Find("Fader").GetComponent<Fader>();
    }

    public void callRestartScene()
    {
        //StartCoroutine(ResetScene());

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public IEnumerator ResetScene()
    {
        yield return fader.FadeIn(0);
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        asyncOperation.allowSceneActivation = false;
        
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;

            yield return null;
        }
    }
}
