using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Material material;
    public GameObject fog;
    public Transform obstacles;

    public static readonly int ChunkWidth = 16;
    public static readonly int WorldSizeInChunks = 7;

    static readonly int ViewDistance = 2;

    public enum Walls { X, N, E, W, S, NE, NW, SE, SW };

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
        fog.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        fog.transform.position = player.position; // fog follows player

        playerChunkPos = GetChunk(player.position); // update player chunk position
        if (playerChunkPos != lastPlayerChunkPos) // if on different chunk
            CheckViewDistance();
    }

    void InitWorld()
    {
        for (int i = 0; i < tileMap.GetLength(0); i++)
            for (int j = 0; j < tileMap.GetLength(1); j++)
                tileMap[i, j] = new Tile(i, 0f, j); // initializes tileMap

        Transform chunksParent = transform.GetChild(0).GetChild(0); // gets Chunks group
        List<Transform> chunkList = new List<Transform>(); // creates list

        foreach (Transform child in chunksParent) // foreach child in parent
        {
            if (!child.TryGetComponent<MeshCollider>(out MeshCollider meshCollider)) // add MeshCollider
            {
                meshCollider = child.gameObject.AddComponent<MeshCollider>();
                Mesh mesh = child.GetComponent<MeshFilter>().mesh;
                meshCollider.sharedMesh = mesh;
                Debug.Log(child.name + " has no collider");
            }
                
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

        /*int length = obstaclesParent.childCount;
        for (int i = 0; i < length; i++)
        {
            Chunk chunk = GetChunk(obstaclesParent.GetChild(0).position);
            if (chunk != null)
                obstaclesParent.GetChild(0).parent = chunk.chunk;
        }

        if (obstaclesParent.childCount > 0)
            Debug.Log("Not all children moved under chunk");*/
    }

    void GetObstacles()
    {
        foreach (Transform child in obstacles)
        {
            Obstacle obstacle = child.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                int x = Mathf.FloorToInt(obstacle.transform.position.x);
                int z = Mathf.FloorToInt(obstacle.transform.position.z);

                for (int i = x, ix = 0; i < x + obstacle.xSize; i++, ix++)
                    for (int j = z, jz = 0; j < z + obstacle.zSize; j++, jz++)
                        if (obstacle.walls[ix, jz] != Walls.X)
                            tileMap[i, j].wall = obstacle.walls[ix, jz];
            }
            else
                Debug.Log("no obstacle script");
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

    public static bool IsInWorld(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < WorldSizeInTiles && pos.y >= 0 && pos.y < WorldSizeInTiles)
            return true;
        else
            return false;
    }

    Chunk GetChunk(Vector3 pos)
    {
        if (IsInWorld(new Vector2Int((int)pos.x, (int)pos.z)))
        {
            int x = Mathf.Abs(Mathf.FloorToInt(pos.x / ChunkWidth));
            int z = Mathf.Abs(Mathf.FloorToInt(pos.z / ChunkWidth));
            return chunks[x, z];
        }
        else
            return null;
    }
}
