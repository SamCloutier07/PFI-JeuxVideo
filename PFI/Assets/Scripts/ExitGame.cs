
using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
   public void Exit()
   {
      // if(EditorApplication.isPlaying)
      //    EditorApplication.ExitPlaymode();
    Application.Quit();  
   }
}
