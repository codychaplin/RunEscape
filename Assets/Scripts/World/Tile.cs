using UnityEngine;

public class Tile
{
    // properties
    public Vector3 pos { get; private set; }

    public bool canWalk { get; private set; }

    public Tile()
    {

    }

    public Tile(Vector3 _pos, bool _canWalk)
    {
        pos = _pos;
        canWalk = _canWalk;
    }
}
