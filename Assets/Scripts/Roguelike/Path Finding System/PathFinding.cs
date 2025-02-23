using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PathFinding : MonoBehaviour
{
    private GridManager grid;
    private FindPathRequestManager requestManager;
    private bool isShow = true;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
        requestManager = GetComponent<FindPathRequestManager>();
    }

    public void startFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.convertObjectPositionToNode(startPosition);
        Node targetNode = grid.convertObjectPositionToNode(targetPosition);
        List<Node> unvisitedNodes = new List<Node>();
        List<Node> visitedNodes = new List<Node>();
        Vector3[] path = new Vector3[0];
        bool isPathFound = false;

        if (startNode.getNodeTile().IsWalkable && targetNode.getNodeTile().IsWalkable) {
            startNode.getNodeTile().HCost = startNode.calculateHCost(targetNode);
            startNode.calculateFCost();
            unvisitedNodes.Add(startNode);

            while (unvisitedNodes.Count > 0) {
                Node currentNode = getNodeWithLowestCost(unvisitedNodes);
                //Node currentNode = unvisitedNodes.RemoveFirstItem();
                Tile currentTile = currentNode.getNodeTile();
                grid.getNodeNeighbours(currentNode);

                if (currentNode == targetNode) {
                    isPathFound = true;
                    break;
                }
                foreach (Node neighbour in currentNode.Neighbours) {
                    if (visitedNodes.Contains(neighbour)) {
                        continue;
                    }
                    Tile neighbourTile = neighbour.getNodeTile();
                    int costToNeighbour = currentTile.GCost + currentNode.calculateHCost(neighbour);
                    if (costToNeighbour < neighbourTile.GCost || !unvisitedNodes.Contains(neighbour)) {
                        neighbourTile.GCost = costToNeighbour;
                        neighbourTile.HCost = neighbour.calculateHCost(targetNode);
                        neighbour.calculateFCost();
                        neighbour.ParentNode = currentNode;

                        unvisitedNodes.Add(neighbour);
                    }
                }

                unvisitedNodes.Remove(currentNode);
                visitedNodes.Add(currentNode);
            }
        }

        yield return null;

        if (isPathFound) {
            path = getPath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(path, isPathFound);
    }

    private Node getNodeWithLowestCost(List<Node> nodes)
    {
        Node currentNode = nodes[0];
         
        for (int i = 0; i < nodes.Count; i++) {
            Tile currentTile = currentNode.getNodeTile();
            Tile nodeTile = nodes[i].getNodeTile();

            if (currentTile.FCost > nodeTile.FCost) {
                currentNode = nodes[i];
            } else if (currentTile.FCost == nodeTile.FCost) {
                if (currentTile.HCost > nodeTile.HCost) {
                    currentNode = nodes[i];
                }
            }
        }
        return currentNode;
    }

    private Vector3[] getPath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }
        Vector3[] wayPoints = convertNodesToPath(path);
        Array.Reverse(wayPoints);

        return wayPoints;
    }

    private Vector3[] convertNodesToPath(List<Node> nodes)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        if (nodes.Count > 0) {
            wayPoints.Add(nodes[0].worldPosition);
            for (int i = 1; i < nodes.Count; i++) {
                Tile currentTile = nodes[i].getNodeTile();
                Tile preTile = nodes[i - 1].getNodeTile();
                Vector2 directionNew = new Vector2(preTile.Col - currentTile.Col, preTile.Row - currentTile.Row);
                if (directionNew != directionOld) {
                    wayPoints.Add(nodes[i].worldPosition);
                }
                directionOld = directionNew;
            }
        }
        return wayPoints.ToArray();
    }
}
