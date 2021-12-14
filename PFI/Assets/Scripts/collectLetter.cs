using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class collectLetter : MonoBehaviour
{
    private PlayerController playerController;
    private NavMeshAgent playerNavMesh;
    private Text collectText;
    [SerializeField] [TextArea] string letterContent;

    private HoverOutline hoverOutline;
    private Outline outline;
    private bool collected;
    private bool isReading;

    private Text Letter;
    private RawImage letterBackground;
    

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        collectText = GameObject.Find("ReadLetter").GetComponent<Text>();
        hoverOutline = GetComponent<HoverOutline>();
        outline = GetComponent<Outline>();
        Letter = GameObject.Find("LetterText").GetComponent<Text>();
        letterBackground = GameObject.Find("LetterBackground").GetComponent<RawImage>();
        playerNavMesh = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
    }

    private void OnMouseOver()
    {
        if (playerController.IsInRange(transform.gameObject, 18.0f) && !playerController.IsInChase())
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
            collectText.enabled = false;
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
                Time.timeScale = 1f;
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
        Time.timeScale = 0;
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
