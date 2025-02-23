using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeDiameter;
    [SerializeField] private GameObject player;
    [SerializeField] private bool canDrawGizmos;

    private Node[,] grid;
    private int totalCol;
    private int totalRow;
    private float nodeRadius;
    private bool isWalkable = true;

    private void Awake()
    {
        nodeRadius = nodeDiameter / 2;
        totalCol = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        totalRow = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    public int MaxSize()
    {
        return totalCol * totalRow;
    }

    private void createGrid()
    {
        grid = new Node[totalCol, totalRow];
        Vector3 bottomLeftPosition = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int col = 0; col < totalCol; col++) {
            for (int row = 0; row < totalRow; row++) {
                Vector3 nodePosition = bottomLeftPosition + Vector3.right * (col * nodeDiameter + nodeRadius) + Vector3.up * (row * nodeDiameter + nodeRadius);
                grid[col, row] = new Node(isWalkable, nodePosition, col, row);
                //isWalkable = !isWalkable;
            }
        }
    }

    public Node convertObjectPositionToNode(Vector3 position)
    {
        float percentX = Mathf.Clamp01((position.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((position.y + gridWorldSize.y / 2) / gridWorldSize.y);
        int x = Mathf.RoundToInt((totalCol - 1) * percentX);
        int y = Mathf.RoundToInt((totalRow - 1) * percentY);

        return grid[x, y];
    }

    public void getNodeNeighbours(Node currentNode)
    {
        Tile currentTile = currentNode.getNodeTile();

        int tileCol = currentTile.Col;
        int tileRow = currentTile.Row;

        if (tileCol + 1 < totalCol) {
            if (tileRow + 1 < totalRow) {
                currentNode.addNeighbour(grid[tileCol + 1, tileRow + 1]);
            }
            if (tileRow - 1 >= 0) {
                currentNode.addNeighbour(grid[tileCol + 1, tileRow - 1]);
            }
            currentNode.addNeighbour(grid[tileCol + 1, tileRow]);
        }
        if (tileCol - 1 >= 0) {
            if (tileRow + 1 < totalRow) {
                currentNode.addNeighbour(grid[tileCol - 1, tileRow + 1]);
            }
            if (tileRow - 1 >= 0) {
                currentNode.addNeighbour(grid[tileCol - 1, tileRow - 1]);
            }
            currentNode.addNeighbour(grid[tileCol - 1, tileRow]);
        }
        if (tileRow + 1 < totalRow) {
            currentNode.addNeighbour(grid[tileCol, tileRow + 1]);
        }
        if (tileRow - 1 >= 0) {
            currentNode.addNeighbour(grid[tileCol, tileRow - 1]);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        //if (grid != null && canDrawGizmos) {
        //    Node playerNode = convertObjectPositionToNode(player.transform.position);
        //    foreach (Node node in grid) {
        //        Gizmos.color = node.getNodeTile().IsWalkable ? new Color(1, 1, 0, 0.5f) : new Color(1, 0, 1, 0.5f);
        //        Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeDiameter);
        //    }
        //}
    }
}
