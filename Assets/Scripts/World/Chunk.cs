using UnityEngine;

public class Chunk
{
    public Transform chunk { get; private set; }
    
    public int x { get; private set; }
    public int z { get; private set; }

    bool _isActive;

    public Chunk(Transform _chunk, int _x, int _z)
    {
        chunk = _chunk;
        x = _x;
        z = _z;
    }

    public bool isActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            if (chunk != null)
                chunk.gameObject.SetActive(value);
        }
    }
}
