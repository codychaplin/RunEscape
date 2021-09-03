﻿using UnityEngine;

public class Tile
{
    public Vector2Int pos { get; private set; }
    public bool canWalk { get; set; }

    // pathfinding
    public int gCost { get; set; } // walking cost from start tile
    public int hCost { get; set; } // approximate cost to end tile
    public int fCost { get; set; } // gCost + hCost
    public Tile previous { get; set; } // used in final path

    public Tile()
    {
        
    }

    public Tile(Vector2Int _pos, bool _canWalk)
    {
        pos = _pos;
        canWalk = _canWalk;
    }

    public void calculateFCost()
    {
        fCost = gCost + hCost;
    }
}
