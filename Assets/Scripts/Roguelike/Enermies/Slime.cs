using UnityEngine;

public class Slime : Unit
{
    public new void TriggerAction(PlayerActions action)
    {
        if (action == PlayerActions.Walk) {
            FindPathRequestManager.RequestPath(transform.position, target.transform.position, onPathFound);
        }
    }
}
