using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CoverHoverOutline : MonoBehaviour
{
    private PlayerController Player;
    private Text coverText;

    private void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        coverText = GameObject.Find("Assassinate").GetComponent<Text>();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        transform.GetComponent<Outline>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    
    private void OnMouseExit()
    {
        transform.GetComponent<Outline>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
