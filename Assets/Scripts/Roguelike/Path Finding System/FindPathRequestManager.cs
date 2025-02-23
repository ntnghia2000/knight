using UnityEngine;
using System.Collections.Generic;
using System;

public class FindPathRequestManager : MonoBehaviour
{
    static FindPathRequestManager instance;

    Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    PathRequest currentRequest;

    PathFinding pathFinding;
    bool isProcessing;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    private void Start()
    {
        isProcessing = false;
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequests.Enqueue(newRequest);
        instance.TryProcessNextRequest();
    }

    private void TryProcessNextRequest()
    {
        if (!isProcessing && pathRequests.Count > 0) {
            currentRequest = pathRequests.Dequeue();
            isProcessing = true;
            pathFinding.startFindPath(currentRequest.pathStart, currentRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool isSucess)
    {
        currentRequest.callback(path, isSucess);
        isProcessing = false;
        TryProcessNextRequest();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
