using UnityEngine;

public class Tile
{
    public int x { get; private set; }
    public float y { get; private set; }
    public int z { get; private set; }
    public bool canWalk { get; set; }
    public World.Walls wall { get; set; }
    public ObstacleObject obj { get; set; }

    public Tile()
    {
        
    }

    public Tile(int _x, float _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
        canWalk = true;
        wall = World.Walls.O;
        obj = null;
    }
}
