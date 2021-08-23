using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Transform player;
    public Material material;

    // stores block types of each block in world (globally)
    public Tile[,] worldMap = new Tile[Voxel.WorldSizeInBlocks, Voxel.WorldSizeInBlocks];

    // array of chunks in game
    Chunk[,] chunks = new Chunk[Voxel.WorldSizeInChunks, Voxel.WorldSizeInChunks];

    // Start is called before the first frame update
    void Start()
    {
        generateChunks();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateChunks()
    {
        for (int x = 0; x < Voxel.WorldSizeInChunks; x++)
            for (int z = 0; z < Voxel.WorldSizeInChunks; z++)
                chunks[x, z] = new Chunk(x, z, this); // creates a chunk at the given coordinates
    }
}
