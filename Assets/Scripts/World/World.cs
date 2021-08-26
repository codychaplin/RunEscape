using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        //generateChunks();
        GetVertices();
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

    void GetVertices()
    {
        StreamReader textIn = new StreamReader(new FileStream(@"Assets\WorldData\data.txt", FileMode.OpenOrCreate, FileAccess.Read));

        int i = 0;
        while (textIn.Peek() != -1)
        {
            string row = textIn.ReadLine();
            string[] column = row.Split('|');

            for (int j = 0; j < column.Length; j++)
            {
                string[] vector = column[j].Split(',');

                Vector3 pos = new Vector3(int.Parse(vector[0]), int.Parse(vector[2]), int.Parse(vector[1]));
                Tile tile = new Tile(pos, true);
                worldMap[i, j] = tile;
            }

            i++;
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < worldMap.GetLength(0); i++)
        {
            for (int j = 0; j < worldMap.GetLength(1); j++)
            {
                Gizmos.DrawSphere(worldMap[i, j].pos, 0.2f);
            }
        }
    }
}
