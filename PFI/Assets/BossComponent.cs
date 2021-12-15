using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossComponent : MonoBehaviour
{
    [SerializeField]private int nbTalismans = 4;
    [SerializeField] private List<Image> imageList = new List<Image>();
    
    private int pickedTalismans;
    private Vector3 scaleChangeVector = new Vector3(5f, 5f, 5f);
    private Image currentDisplay;

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
        if (currentDisplay != null && Input.GetKeyDown(KeyCode.Escape))
        {
            currentDisplay.enabled = false;
            Time.timeScale = 1f;
            currentDisplay = null;
        }
    }
}