using UnityEngine;

public class TalismanComponent : MonoBehaviour
{
    [SerializeField] private BossComponent bossComponent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Equals("Player"))
            return;
        
        bossComponent.PickTalisman();
        transform.gameObject.SetActive(false);
    }
}