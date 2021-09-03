using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Material material;

    public static readonly int ChunkWidth = 16;
    public static readonly int WorldSizeInChunks = 2;

    // used to get the world size in tiles, given size of world in chunks
    public static int WorldSizeInTiles
    {
        get { return WorldSizeInChunks * ChunkWidth; }
    }

    // stores tiles (globally)
    public static Tile[,] tileMap = new Tile[WorldSizeInTiles, WorldSizeInTiles];

    // array of chunks in game
    Transform[,] chunks = new Transform[WorldSizeInChunks, WorldSizeInChunks];

    // Start is called before the first frame update
    void Start()
    {
        InitWorld();
        GetData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitWorld()
    {
        for (int i = 0; i < tileMap.GetLength(0); i++)
            for (int j = 0; j < tileMap.GetLength(1); j++)
                tileMap[i, j] = new Tile(new Vector2Int(i, j), true); // initializes tileMap

        Transform chunkParent = transform.GetChild(0); // gets WorldMesh object
        List<Transform> chunkList = new List<Transform>(); // creates list

        foreach (Transform child in chunkParent) // foreach child in parent
            if (child.tag == "Chunk") // if tag = Chunk
                chunkList.Add(child); // add to list

        int index = 0;
        for (int i = 0; i < chunks.GetLength(0); i++)
            for (int j = 0; j < chunks.GetLength(1); j++)
                chunks[i, j] = chunkList[index++]; // transfers list to array
    }

    void GetData()
    {
        StreamReader textIn = new StreamReader(new FileStream(@"Assets\WorldData\Obstacles.txt", FileMode.OpenOrCreate, FileAccess.Read));

        while (textIn.Peek() != -1)
        {
            string row = textIn.ReadLine();
            string[] column = row.Split(',');

            // swap y and z when importing from Blender
            Vector2Int pos = new Vector2Int(int.Parse(column[0]), int.Parse(column[1]));

            bool flag = false;
            for (int i = 0; i < tileMap.GetLength(0); i++) // checks each row
            {
                if (pos.x == tileMap[i, tileMap.GetLength(0) - 1].pos.x) // if pos.x matches pos.x in row
                {
                    // if pos.y >= pos.y halfway through tileMap[i, ], search second half, otherwise, start at 0
                    int index = (pos.y >= tileMap[i, (tileMap.GetLength(1) - 1) / 2].pos.y) ? (tileMap.GetLength(1) - 1) / 2 : 0;

                    for (int j = index; j < tileMap.GetLength(1); j++) // checks each column
                    {
                        if (tileMap[i, j].pos == pos) // is pos matches, assign and break
                        {
                            tileMap[i, j].canWalk = false;
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag)
                    break;
            }
        }
    }

    public static Tile GetTile(int x, int z)
    {
        return tileMap[x, z];
    }

    void OnDrawGizmosSelected()
    {

    }
}
