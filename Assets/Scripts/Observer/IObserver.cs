using UnityEngine;
using System.Collections.Generic;
using System;

public interface IObserver
{
    public void OnNotify();

    public void TriggerAction(PlayerActions action, Action callback);
}
