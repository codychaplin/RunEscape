using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Material material;

    public static readonly int ChunkWidth = 16;
    public static readonly int WorldSizeInChunks = 6;
    static readonly int ViewDistance = 1;

    // used to get the world size in tiles, given size of world in chunks
    public static int WorldSizeInTiles
    {
        get { return WorldSizeInChunks * ChunkWidth; }
    }

    // stores tiles (globally)
    public static Tile[,] tileMap = new Tile[WorldSizeInTiles, WorldSizeInTiles];

    // array of chunks in game
    Chunk[,] chunks = new Chunk[WorldSizeInChunks, WorldSizeInChunks];
    List<Chunk> activeChunks = new List<Chunk>();
    Chunk playerChunkPos;
    Chunk lastPlayerChunkPos;

    // Start is called before the first frame update
    void Start()
    {
        InitWorld(); // initialize tileMap and chunks
        GetObstacles(); // get unwalkable positions

        lastPlayerChunkPos = GetChunk(player.position); // set chunk player is on
        CheckViewDistance();
    }

    // Update is called once per frame
    void Update()
    {
        playerChunkPos = GetChunk(player.position); // update player chunk position
        if (playerChunkPos != lastPlayerChunkPos) // if on different chunk
            CheckViewDistance();
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
                if (!child.GetComponent<MeshCollider>()) // add MeshCollider
                {
                    MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    Mesh mesh = child.GetComponent<MeshFilter>().mesh;
                    meshCollider.sharedMesh = mesh;
                    child.gameObject.SetActive(false);
                    chunkList.Add(child); // add to list
                }

        if (chunkList.Count == chunks.Length)
        {
            int index = 0;
            for (int i = 0; i < chunks.GetLength(0); i++)
                for (int j = 0; j < chunks.GetLength(1); j++)
                    chunks[i, j] = new Chunk(chunkList[index++], i, j); // transfers list to array
        }
        else
            Debug.Log("Chunks list/array not same size");
    }

    void GetObstacles()
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

    void CheckViewDistance()
    {
        Chunk chunkPos = GetChunk(player.position);
        if (chunkPos != null)
        {
            lastPlayerChunkPos = playerChunkPos;

            List<Chunk> previouslyActiveChunks = new List<Chunk>(activeChunks);
            activeChunks.Clear();

            for (int i = Mathf.Max(0, chunkPos.x - ViewDistance); i <= Mathf.Min(WorldSizeInChunks - 1, chunkPos.x + ViewDistance); i++)
                for (int j = Mathf.Max(0, chunkPos.z - ViewDistance); j <= Mathf.Min(WorldSizeInChunks - 1, chunkPos.z + ViewDistance); j++)
                {
                    Chunk thisChunk = chunks[i, j];
                    if (thisChunk != null)
                    {
                        thisChunk.isActive = true;
                        activeChunks.Add(thisChunk);
                    }

                    for (int k = 0; k < previouslyActiveChunks.Count; k++)
                        if (previouslyActiveChunks[k] == thisChunk)
                            previouslyActiveChunks.RemoveAt(k);
                }

            foreach (Chunk chunk in previouslyActiveChunks)
            {
                chunks[chunk.x, chunk.z].isActive = false;
            }
        }
    }

    public static Tile GetTile(int x, int z)
    {
        return tileMap[x, z];
    }

    bool IsInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < WorldSizeInTiles && pos.z >= 0 && pos.z < WorldSizeInTiles)
            return true;
        else
            return false;
    }

    Chunk GetChunk(Vector3 pos)
    {
        if (IsInWorld(pos))
        {
            int x = Mathf.Abs(Mathf.FloorToInt(pos.x / ChunkWidth));
            int z = Mathf.Abs(Mathf.FloorToInt(pos.z / ChunkWidth));
            return chunks[x, z];
        }
        else
            return null;
    }
}
