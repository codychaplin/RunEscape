using UnityEngine;

public class Node
{
    public int x { get; private set; }
    public float y { get; private set; }
    public int z { get; private set; }
    public bool canWalk { get; set; }
    public World.Walls wall { get; set; }

    public int gCost { get; set; } // walking cost from start tile
    public int hCost { get; set; } // approximate cost to end tile
    public int fCost { get; set; } // gCost + hCost
    public Node previous { get; set; } // used in final path

    public Node()
    {

    }

    public Node(int _x, float _y, int _z, bool _canWalk, World.Walls _wall)
    {
        x = _x;
        y = _y;
        z = _z;
        canWalk = _canWalk;
        wall = _wall;
        gCost = int.MaxValue;
        calculateFCost();
        previous = null;
    }

    public void calculateFCost()
    {
        fCost = gCost + hCost;
    }
}
