using UnityEngine;
using System;

public class Slime : Unit
{
    public new void TriggerAction(PlayerActions action, Action callback)
    {
        if (action == PlayerActions.Walk) {
            FindPathRequestManager.RequestPath(transform.position, target.transform.position, onPathFound);
        }
    }
}
