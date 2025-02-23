using UnityEngine;

public interface IObserver
{
    public void OnNotify();

    public void TriggerAction(PlayerActions action);
}
