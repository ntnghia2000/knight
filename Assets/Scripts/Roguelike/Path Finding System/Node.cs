using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private int col;
    private int row;
    private int gCost;
    private int hCost;
    private int fCost;
    private bool isWalkable;

    public Tile(int col, int row, bool isWalkable)
    {
        this.col = col;
        this.row = row;
        gCost = 0;
        hCost = 0;
        fCost = 0;
        this.isWalkable = isWalkable;
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int GCost
    {
        get { return gCost; }
        set { gCost = value; }
    }

    public int HCost
    {
        get { return hCost; }
        set { hCost = value; }
    }

    public int FCost
    {
        get { return fCost; }
        set { fCost = value; }
    }

    public bool IsWalkable
    {
        get { return isWalkable; }
        set { isWalkable = value; }
    }
}

public class Node : IHeapItem<Node>
{
    public Vector3 worldPosition;

    private int straightCost = 10;
    private int diagonalCost = 14;
    private int heapIndex;

    private List<Node> neighbours = new List<Node>();
    private Node parentNode;
    private Tile nodeTile;

    public List<Node> Neighbours
    {
        get { return neighbours; }
    }

    public Node(bool isWalkable, Vector2 position, int col, int row)
    {
        worldPosition = position;
        nodeTile = new Tile(col, row, isWalkable);
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public Tile getNodeTile()
    {
        return nodeTile;
    }

    public Node ParentNode
    {
        get { return parentNode; }
        set { parentNode = value; }
    }

    public int calculateHCost(Node targerNode)
    {
        Tile tile = targerNode.getNodeTile();
        float xVector = Mathf.Abs(tile.Col - nodeTile.Col);
        float yVector = Mathf.Abs(tile.Row - nodeTile.Row);
        float diagonal = Mathf.Min(xVector, yVector);
        return Mathf.RoundToInt(Mathf.Abs(xVector - yVector) * straightCost + diagonal * diagonalCost);
    }

    public void calculateFCost()
    {
        nodeTile.FCost = nodeTile.GCost + nodeTile.HCost;
    }

    public void addNeighbour(Node neighbour)
    {
        if (neighbour.getNodeTile().IsWalkable) {
            neighbours.Add(neighbour);
        }
    }

    public int CompareTo(Node node)
    {
        int compareValue = nodeTile.FCost.CompareTo(node.getNodeTile().FCost);
        if (compareValue == 0) {
            compareValue = nodeTile.HCost.CompareTo(node.getNodeTile().HCost);
        }
        return -compareValue;
    }
}
