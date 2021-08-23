using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    World world;
    GameObject chunk;

    // chunk coordinates
    public int x { get; private set; }
    public int z { get; private set; }

    // Used to render voxel
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;

    // used to store vertices and such for an individual voxel
    Vector3[] vertices = new Vector3[(Voxel.ChunkWidth + 1) * (Voxel.ChunkWidth + 1)];
    int[] triangles = new int[Voxel.ChunkWidth * Voxel.ChunkWidth * 6];
    List<Vector2> uvs = new List<Vector2>();

    public Chunk(int _x, int _z, World _world)
    {
        x = _x;
        z = _z;
        world = _world;
        chunk = new GameObject();
        meshFilter = chunk.AddComponent<MeshFilter>();
        meshRenderer = chunk.AddComponent<MeshRenderer>();
        meshCollider = chunk.AddComponent<MeshCollider>();
        meshRenderer.material = world.material;

        chunk.transform.SetParent(world.transform); // puts chunk game objects under world parent
        chunk.name = "Chunk " + x + "," + z; // gives chunk corresponding name

        CreateChunk();
    }

    void CreateChunk()
    {
        // creates vertices for chunk
        for (int z = 0, i = 0; z < Voxel.ChunkWidth; z++)
            for (int x = 0; x < Voxel.ChunkWidth; x++)
            {
                vertices[i] = (new Vector3Int(x, 0, z));
                i++;
            }

        // increment variables
        int vert = 0;
        int tris = 0;

        // creates triangles for chunk
        for (int z = 0; z < Voxel.ChunkWidth - 1; z++)
        {
            for (int x = 0; x < Voxel.ChunkWidth - 1; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + Voxel.ChunkWidth;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + Voxel.ChunkWidth;
                triangles[tris + 5] = vert + Voxel.ChunkWidth + 1;

                vert++; // increments to next square
                tris += 6; // increments to next set of triangles
            }

            vert++; // used so last vert in row doesn't try to connect to first vert in next row
        }
        
        CreateMesh(); // adds information needed to render faces
    }

    // RESPONSIBLE FOR CREATING MESHES USED TO RENDER FACES
    void CreateMesh()
    {
        Mesh mesh = new Mesh(); // creates new mesh object
        mesh.vertices = vertices; // vertex positions from list get set as positions for mesh vertices
        mesh.triangles = triangles;
        //mesh.uv = uvs.ToArray(); // adds UVs to mesh
        mesh.RecalculateNormals(); // calculates normal (direction) of face
        meshFilter.mesh = mesh; // sets mesh to mesh filter
        meshCollider.sharedMesh = meshFilter.mesh; // adds collider to chunk
    }

    void AddTexture()
    {
        uvs.Add(Voxel.voxelUVs[0]);
        uvs.Add(Voxel.voxelUVs[1]);
        uvs.Add(Voxel.voxelUVs[2]);
        uvs.Add(Voxel.voxelUVs[3]);
    }
}
