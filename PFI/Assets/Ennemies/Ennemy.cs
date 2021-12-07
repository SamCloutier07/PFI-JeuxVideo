using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(CombatTarget))]
[RequireComponent(typeof(HoverOutline))]
[RequireComponent(typeof(Animator))]
public class Ennemy : MonoBehaviour
{
    private Animator animator;
    public bool isDead = false;
    private ActionScheduler actionscheduler;
    private GameObject player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        actionscheduler = GetComponent<ActionScheduler>();
        player = GameObject.FindWithTag("Player");
    }
    
    public void Die()
    {
        actionscheduler.CancelCurrent();
        animator.SetBool("IsAssassinated", true);
        transform.GetComponentInChildren<FieldOfView>().gameObject.SetActive(false);
        transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
        transform.position = player.transform.position + player.transform.TransformDirection(new Vector3(0, 0, 5));
        
        isDead = true;
       
    }
    
   
}
