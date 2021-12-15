using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private Image toShow;
    [SerializeField] private string toActivateAfter;
    private GameObject objectToActivate;

    private void Awake()
    {
        if (!string.IsNullOrWhiteSpace(toActivateAfter))
        {
            objectToActivate = GameObject.Find(toActivateAfter);
            objectToActivate.SetActive(false);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Equals("Player"))
            return;

        toShow.enabled = true;
        Time.timeScale = 0f;
    }
    
    private void Update()
    {
        if (toShow.enabled && Input.GetKeyDown(KeyCode.Escape))
        {
            toShow.enabled = false;
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            
            if (objectToActivate != null)
                objectToActivate.SetActive(true);
        }
    }
}
