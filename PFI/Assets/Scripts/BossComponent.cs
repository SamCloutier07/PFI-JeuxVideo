using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossComponent : MonoBehaviour
{
    [SerializeField] private List<Image> imageList = new List<Image>();
    
    private int pickedTalismans;
    private Vector3 scaleChangeVector = new Vector3(5f, 5f, 5f);
    private Image currentDisplay;
    private Ennemy boss;
    

    private void Awake()
    {
        boss = GetComponent<Ennemy>();
    }

    public void PickTalisman()
    {
        currentDisplay = imageList[pickedTalismans];
        currentDisplay.enabled = true;
        pickedTalismans++;
        transform.localScale = transform.localScale - scaleChangeVector;
        Time.timeScale = 0f;
    }

    private void Update()
    {
       

        if (boss.isDead)
        {
            
            if (currentDisplay != null )
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if(currentDisplay == imageList[pickedTalismans + 1])
                        SceneManager.LoadScene(0);
                    else
                    {
                        currentDisplay = imageList[pickedTalismans + 1];
                        currentDisplay.enabled = true;
                    }
                }
            }
            else
                ShowVictory();
            return;
        }
        if (currentDisplay != null && Input.GetKeyDown(KeyCode.Escape))
        {
            currentDisplay.enabled = false;
            Time.timeScale = 1f;
            currentDisplay = null;
        }

    }
    
    private void ShowVictory()
    {
        currentDisplay = imageList[pickedTalismans];
        currentDisplay.enabled = true;
    }
}