using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScheduler : MonoBehaviour
{
    // Components
    private IAction currentAction = null;
    
    public void StartAction(IAction action)
    {
        if(currentAction == action) return;
        if(currentAction != null)
            currentAction.Cancel();
        currentAction = action;
    }

    public void CancelCurrent() => StartAction(null);

}
