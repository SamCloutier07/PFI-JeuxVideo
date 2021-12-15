using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class HoverOutline : MonoBehaviour
{
    private Text assassinateText;
    private PlayerController player;
    private CombatTarget combatTarget;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        assassinateText = GameObject.Find("Assassinate").GetComponent<Text>();
        combatTarget = GetComponent<CombatTarget>();
    }

    private void Update()
    {
        if(!player.CanKill()) RemoveCanKillText();
    }

    private void OnMouseOver()
    {
        if(player.IsInRange(transform.gameObject) && player.CanAttack(combatTarget))
            ShowKillText();

        transform.GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        RemoveCanKillText();
        transform.GetComponent<Outline>().enabled = false;
    }

    private void ShowKillText() =>
        assassinateText.enabled = true;

    public void RemoveCanKillText() =>
        assassinateText.enabled = false;
}