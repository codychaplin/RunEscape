using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Material material;
    public GameObject fog;
    public Transform chunksParent;
    public Canvas canvas;

    public static readonly int ChunkWidth = 128;
    public static readonly int WorldSizeInChunksX = 2;
    public static readonly int WorldSizeInChunksZ = 2;

    static readonly int ViewDistance = 1;

    public enum Walls { O, X, N, E, W, S, NE, NW, SE, SW };

    public static string playerName { get { return "Cody"; } }

    // used to get the world size in tiles, given size of world in chunks
    public static int WorldSizeX
    {
        get { return WorldSizeInChunksX * ChunkWidth; }
    }

    public static int WorldSizeZ
    {
        get { return WorldSizeInChunksZ * ChunkWidth; }
    }

    // stores tiles (globally)
    public static Tile[,] tileMap = new Tile[WorldSizeX, WorldSizeZ];

    // array of chunks in game
    Chunk[,] chunks = new Chunk[WorldSizeInChunksX, WorldSizeInChunksZ];
    List<Chunk> activeChunks = new List<Chunk>();
    Chunk playerChunkPos;
    Chunk lastPlayerChunkPos;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(true);

        InitWorld(); // initialize tileMap and chunks
        GetObstacles(); // get unwalkable positions

        lastPlayerChunkPos = GetChunk(player.position); // set chunk player is on
        CheckViewDistance();
        fog.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChatBox.instance.input.isFocused && Input.GetKeyDown(KeyCode.Space))
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);

        if (player.position.x <= 25 && player.position.z <= 25)
            fog.transform.position = new Vector3(25f, 0f, 25f);
        else if (player.position.x <= 25 && player.position.z > 25)
            fog.transform.position = new Vector3(25f, 0f, player.position.z);
        else if (player.position.x > 25 && player.position.z <= 25)
            fog.transform.position = new Vector3(player.position.x, 0f, 25f);
        else
            fog.transform.position = player.position; // fog follows player

        playerChunkPos = GetChunk(player.position); // update player chunk position
        if (playerChunkPos != lastPlayerChunkPos) // if on different chunk
            CheckViewDistance();
    }

    private void FixedUpdate()
    {
        //Debug.Log(Time.time);
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
        foreach (Transform chunk in chunksParent)
            if (chunk.childCount > 0)
                foreach (Transform child in chunk)
                {
                    if (child.tag == "Environment")
                        continue;

                    Obstacle obstacle = child.GetComponent<Obstacle>();
                    if (obstacle != null)
                    {
                        int x = Mathf.FloorToInt(obstacle.transform.position.x);
                        int z = Mathf.FloorToInt(obstacle.transform.position.z);

                        if (obstacle.obj != null)
                            tileMap[x, z].obj = obstacle.obj;

                        if (obstacle.xSize == 1 && obstacle.zSize == 1)
                        {
                            tileMap[x, z].wall = World.Walls.X;
                            tileMap[x, z].canWalk = false;
                        }
                        else
                        {
                            for (int i = x, ix = 0; i < x + obstacle.xSize; i++, ix++)
                                for (int j = z, jz = 0; j < z + obstacle.zSize; j++, jz++)
                                {
                                    if (obstacle.walls[ix, jz] != Walls.O)
                                        tileMap[i, j].wall = obstacle.walls[ix, jz];
                                    if (obstacle.walls[ix, jz] == Walls.X)
                                        tileMap[i, j].canWalk = false;
                                }
                        }
                    }
                    else
                        Debug.Log("no obstacle script on " + child.name);
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

            for (int i = Mathf.Max(0, chunkPos.x - ViewDistance); i <= Mathf.Min(WorldSizeInChunksX - 1, chunkPos.x + ViewDistance); i++)
                for (int j = Mathf.Max(0, chunkPos.z - ViewDistance); j <= Mathf.Min(WorldSizeInChunksZ - 1, chunkPos.z + ViewDistance); j++)
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
        if (pos.x >= 0 && pos.x < WorldSizeX && pos.y >= 0 && pos.y < WorldSizeZ)
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

    public static int XPFormula(float x)
    {
        return Mathf.FloorToInt(0.25f * (x - 1 + 800 * (Mathf.Pow(2f, (x - 1f) / 8f))));
    }
}
