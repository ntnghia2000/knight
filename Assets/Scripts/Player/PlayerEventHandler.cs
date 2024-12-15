using UnityEngine;

public class PlayerEventHandler : MonoBehaviour, IObserver
{
    [SerializeField] private Subject _playerSubject;

    private void OnEnable() {
        _playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }

    public void OnNotify() {
        Debug.Log("Say Hi To Observer");
    }

    public void TriggerAction(PlayerActionEnum action) {
        //
    }
}
