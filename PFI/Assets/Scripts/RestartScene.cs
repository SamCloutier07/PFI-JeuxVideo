
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
    
    
}
