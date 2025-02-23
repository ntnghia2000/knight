using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, IObserver
{
    [SerializeField] protected Subject target;
    [SerializeField] protected float speed;

    Vector3[] path;
    int currentPathIndex;

    private void OnEnable()
    {
        target.AddObserver(this);
    }

    private void Start()
    {
        FindPathRequestManager.RequestPath(transform.position, target.transform.position, onPathFound);
    }

    protected void onPathFound(Vector3[] newPath, bool isPathFound)
    {
        if (isPathFound) {
            path = newPath;
            currentPathIndex = 0;
            StopCoroutine("followPath");
            StartCoroutine("followPath");
        }
    }

    protected IEnumerator followPath()
    {
        if (path.Length > 0) {
            Vector3 currentWayPoint = path[0];
            while(true) {
                if (transform.position == currentWayPoint) {
                    currentPathIndex++;
                    if (currentPathIndex >= path.Length) {
                        yield break;
                    }
                    currentWayPoint = path[currentPathIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed);
                yield return null;
            }
        }
        yield return null;
    }

    public void OnNotify()
    {
        Debug.Log("Initialize unit success");
    }

    public void TriggerAction(PlayerActions action)
    {
        if (action == PlayerActions.Walk) {
            FindPathRequestManager.RequestPath(transform.position, target.transform.position, onPathFound);
        }
    }

    private void OnDisable()
    {
        target.RemoveObserver(this);
    }
}
