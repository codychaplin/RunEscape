using UnityEngine;

public static class Voxel
{
    // chunk width
    public static readonly int ChunkWidth = 16;
    //defined world width/length in chunks
    public static readonly int WorldSizeInChunks = 4;

    // used to get the world size in voxels, given size of world in chunks
    public static int WorldSizeInBlocks
    {
        get { return WorldSizeInChunks * ChunkWidth; }
    }

    // vertices of box, repesented in an array
    public static readonly Vector3Int[] voxelVerts = new Vector3Int[4]
    {
        new Vector3Int(0, 0, 0), // 0, back right
        new Vector3Int(1, 0, 0), // 1, back left
        new Vector3Int(0, 0, 1), // 2, front right
        new Vector3Int(1, 0, 1)  // 3, front left
    };

    // creates each face using vertices to create two triangles, forming square
    public static readonly int[] voxelTris = new int[4] { 0, 2, 1, 3 };

    // Lookup table for texture UVs
    public static readonly Vector2Int[] voxelUVs = new Vector2Int[4]
    {
        new Vector2Int(0, 0), // bottom left corner
        new Vector2Int(0, 1), // top left corner
        new Vector2Int(1, 0), // bottom right corner
        new Vector2Int(1, 1)  // top right corner
    };
}
