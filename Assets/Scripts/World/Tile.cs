using UnityEngine;

public class Tile
{
    public Vector3Int pos { get; private set; }
    public bool canWalk { get; set; }

    // pathfinding
    public int gCost { get; set; } // walking cost from start tile
    public int hCost { get; set; } // approximate cost to end tile
    public int fCost { get; set; } // gCost + hCost
    public Tile previous { get; set; } // used in final path

    public Tile()
    {
        
    }

    public Tile(Vector3Int _pos)
    {
        pos = _pos;
        canWalk = true;
    }

    public void calculateFCost()
    {
        fCost = gCost + hCost;
    }
}
