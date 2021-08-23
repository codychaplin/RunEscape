using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    // chunk width
    public static readonly int ChunkWidth = 16;

    // vertices of box, repesented in an array
    public static readonly Vector2[] voxelVerts = new Vector2[]
    {
        new Vector2(0.0f, 0.0f), // 0, back right
        new Vector2(1.0f, 0.0f), // 1, back left
        new Vector2(0.0f, 1.0f), // 2, front right
        new Vector2(1.0f, 1.0f), // 3, front left
    };

    // creates each face using vertices to create two triangles, forming square
    public static readonly int[,] voxelTris = new int[6, 4]
    {
        // should be 6 each but fourth and fifth are duplicates of second and third (mirrored). eg. {0, 3, 1, 1, 3, 2}
        // indexes go in order of 0 1 2 2 1 3
        {0, 3, 1, 2}, // 0, back face
        {5, 6, 4, 7}, // 1, front face
        {3, 7, 2, 6}, // 2, top face
        {1, 5, 0, 4}, // 3, bottom face
        {4, 7, 0, 3}, // 4, left face
        {1, 2, 5, 6}  // 5, right face
    };
}
