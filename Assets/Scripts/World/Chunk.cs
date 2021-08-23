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
    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
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
        CreateMesh();
    }

    void CreateChunk()
    {
        ClearMesh(); // clears any mesh data before creating new data

        for (int x = 0; x < Voxel.ChunkWidth; x++)
            for (int z = 0; z < Voxel.ChunkWidth; z++)
                CreateTile(new Vector3Int(x, 0, z)); // creates block

        CreateMesh(); // adds information needed to render faces
    }

    void CreateTile(Vector3Int pos)
    {
        // adds vertices
        vertices.Add(pos + Voxel.voxelVerts[Voxel.voxelTris[0]]);
        vertices.Add(pos + Voxel.voxelVerts[Voxel.voxelTris[1]]);
        vertices.Add(pos + Voxel.voxelVerts[Voxel.voxelTris[2]]);
        vertices.Add(pos + Voxel.voxelVerts[Voxel.voxelTris[3]]);

        AddTexture();

        // adds UVs
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);

        vertexIndex += 4;
    }

    // RESPONSIBLE FOR CREATING MESHES USED TO RENDER FACES
    void CreateMesh()
    {
        Mesh mesh = new Mesh(); // creates new mesh object
        mesh.vertices = vertices.ToArray(); // vertex positions from list get set as positions for mesh vertices
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray(); // adds UVs to mesh
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

    // clears all mesh data
    void ClearMesh()
    {
        vertexIndex = 0;
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
    }
}
