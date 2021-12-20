using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGameComponent : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene(1);
}
