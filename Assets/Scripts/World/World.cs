using UnityEngine;
using System.IO;

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
    GameObject[,] chunks = new GameObject[WorldSizeInChunks, WorldSizeInChunks];

    // Start is called before the first frame update
    void Start()
    {
        GetVertices();
        GetData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GetData()
    {
        StreamReader textIn = new StreamReader(new FileStream(@"Assets\WorldData\data.txt", FileMode.OpenOrCreate, FileAccess.Read));

        while (textIn.Peek() != -1)
        {
            string row = textIn.ReadLine();
            string[] column = row.Split(',');

            // swap y and z when importing from Blender
            Vector3Int pos = new Vector3Int(int.Parse(column[0]), int.Parse(column[2]), int.Parse(column[1]));

            bool flag = false;
            for (int i = 0; i < tileMap.GetLength(0); i++) // checks each row
            {
                if (pos.x == tileMap[i, tileMap.GetLength(0) - 1].pos.x) // if pos.x matches pos.x in row
                {
                    // if pos.z >= pos.z halfway through tileMap[i, ], search second half, otherwise, start at 0
                    int index = (pos.z >= tileMap[i, (tileMap.GetLength(1) - 1) / 2].pos.z) ? (tileMap.GetLength(1) - 1) / 2 : 0;

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

    void GetVertices()
    {
        StreamReader textIn = new StreamReader(new FileStream(@"Assets\WorldData\vertices.txt", FileMode.OpenOrCreate, FileAccess.Read));

        int i = 0;
        while (textIn.Peek() != -1)
        {
            string row = textIn.ReadLine();
            string[] column = row.Split('|');

            for (int j = 0; j < column.Length - 1; j++)
            {
                string[] vector = column[j].Split(',');

                // swap y and z when importing from Blender
                Vector3Int pos = new Vector3Int(int.Parse(vector[0]), int.Parse(vector[2]), int.Parse(vector[1]));
                Tile tile = new Tile(pos, true);
                tileMap[i, j] = tile;
            }

            i++;
        }
    }

    public static Tile GetTile(int x, int z)
    {
        return tileMap[x, z];
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < tileMap.GetLength(0); i++)
            for (int j = 0; j < tileMap.GetLength(1); j++)
            {
                if (tileMap[i, j].canWalk)
                    Gizmos.DrawSphere(tileMap[i, j].pos, 0.2f);
                else
                    Gizmos.DrawCube(tileMap[i, j].pos, Vector3.one / 2);
            }
    }
}
