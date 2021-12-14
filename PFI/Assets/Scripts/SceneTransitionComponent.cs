using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionComponent : MonoBehaviour
{
    private Fader fader;

    private void Awake() => fader = GameObject.Find("Fader").GetComponent<Fader>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Equals("Player"))
            return;
        
        //Time.timeScale = 0f;
        StartCoroutine(LoadNextScene());
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
