using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class HoverOutline : MonoBehaviour
{
    private Text assassinateText;
    private PlayerController Player;

    private void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        assassinateText = GameObject.Find("Assassinate").GetComponent<Text>();
    }

    private void Update()
    {
        if(!Player.CanKill()) RemoveCanKillText();
    }

    private void OnMouseOver()
    {
        if(Player.IsInRange(transform.gameObject))
        {
            ShowKillText();
        }
        
        transform.GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        RemoveCanKillText();
        transform.GetComponent<Outline>().enabled = false;
    }

    private void ShowKillText()
    {
        assassinateText.enabled = true;
    }

    public void RemoveCanKillText()
    {
        assassinateText.enabled = false;
    }
}
