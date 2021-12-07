using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class collectLetter : MonoBehaviour
{
    private PlayerControler playerController;
    private NavMeshAgent playerNavMesh;
    private Text collectText;
    [SerializeField] [TextArea] string letterContent;

    private HoverOutline hoverOutline;
    private Outline outline;
    private bool collected = false;
    private bool isReading = false;

    private Text Letter;
    private RawImage letterBackground;
    

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
        collectText = GameObject.Find("ReadLetter").GetComponent<Text>();
        hoverOutline = GetComponent<HoverOutline>();
        outline = GetComponent<Outline>();
        Letter = GameObject.Find("LetterText").GetComponent<Text>();
        letterBackground = GameObject.Find("LetterBackground").GetComponent<RawImage>();
        playerNavMesh = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
    }

    private void OnMouseOver()
    {
        if (playerController.IsInRange(transform.gameObject, 18.0f))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerNavMesh.isStopped = true;
                collected = true;
                transform.parent.GetChild(1).gameObject.SetActive(false);
                collectText.text = "ESC to close letter";
                collectText.color = Color.red;
                hoverOutline.enabled = false;
                outline.OutlineMode = Outline.Mode.OutlineHidden;
                isReading = true;
                DisplayLetter();
            }
            if(!collected)
                collectText.enabled = true;
        }
        else
        {
           
            collectText.enabled = false;
        }
    }

    private void OnMouseExit()
    {
        collectText.enabled = false;
    }

    private void Update()
    {
        if (collected && isReading)
        {
            collectText.enabled = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerNavMesh.isStopped = false;
                HideLetter();
                collectText.enabled = false;
                isReading = false;
            }
        }
    }

    private void DisplayLetter()
    {
        letterBackground.enabled = true;
        Letter.enabled = true;
        Letter.text = letterContent;
        
    }
    
    private void HideLetter()
    {
        collectText.text = "F to Read Letter";
        collectText.color = Color.green;
        Letter.text = "";
        Letter.enabled = false;
        letterBackground.enabled = false;
    }
}
