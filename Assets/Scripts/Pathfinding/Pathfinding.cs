using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    List<Tile> openList;
    List<Tile> closedList;

    const int STRAIGHT_COST = 10; // 1 * 10
    const int DIAGONAL_COST = 14; // square root of 2 * 10

    public static Pathfinding Instance { get; private set; }

    public Pathfinding()
    {
        Instance = this;
    }

    public List<Vector2> FindVectorPath(Vector3 startPos, Vector3 endPos)
    {
        if (startPos != endPos) // if not on same tile
        {
            List<Tile> path = FindPath((int)startPos.x, (int)startPos.z, (int)endPos.x, (int)endPos.z); // find path
            if (path == null) { return null; }
            else
            {
                // convert tiles to vector3s
                List<Vector2> vectorPath = new List<Vector2>();

                for (int i = 0; i < path.Count; i++)
                    vectorPath.Add(new Vector2(path[i].pos.x + 0.5f, path[i].pos.z + 0.5f));

                return vectorPath;
            }
        }
        else
            return null;
    }

    public List<Tile> FindPath(int startX, int startZ, int endX, int endZ)
    {
        Tile startTile = World.GetTile(startX, startZ); // gets starting tile object
        Tile endTile = World.GetTile(endX, endZ); // gets ending tile object

        if (startTile == null || endTile == null) // if either are null, return
            return null;

        openList = new List<Tile> { startTile };
        closedList = new List<Tile>();

        // initializes tileMap for pathfinding
        for (int x = 0; x < World.tileMap.GetLength(0); x++)
            for (int z = 0; z < World.tileMap.GetLength(1); z++)
            {
                World.tileMap[x, z].gCost = int.MaxValue;
                World.tileMap[x, z].calculateFCost();
                World.tileMap[x, z].previous = null;
            }

        startTile.gCost = 0;
        startTile.hCost = CalculateHCost(startTile, endTile);
        startTile.calculateFCost();

        // main algorithm loop
        while (openList.Count > 0)
        {
            Tile currentTile = GetLowestFCostTile(openList); // checks best candidate tile

            if (currentTile == endTile)
                return CalculatePath(endTile); // END OF PATH

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                if (closedList.Contains(neighbour)) continue; // if already checked
                if (!neighbour.canWalk) // if neighbour is not walkable
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int gCost = currentTile.gCost + CalculateHCost(currentTile, neighbour);
                if (gCost < neighbour.gCost)
                {
                    neighbour.previous = currentTile;
                    neighbour.gCost = gCost;
                    neighbour.hCost = CalculateHCost(neighbour, endTile);
                    neighbour.calculateFCost();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Tile> CalculatePath(Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;

        while (currentTile.previous != null)
        {
            path.Add(currentTile.previous);
            currentTile = currentTile.previous;
        }

        path.Reverse();
        return path;
    }

    public int CalculateHCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.pos.x - b.pos.x);
        int zDistance = Mathf.Abs(a.pos.z - b.pos.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + STRAIGHT_COST * remaining;
    }

    Tile GetLowestFCostTile(List<Tile> tileList)
    {
        Tile lowest = tileList[0];
        for (int i = 1; i < tileList.Count; i++)
            if (tileList[i].fCost < lowest.fCost)
                lowest = tileList[i];

        return lowest;
    }

    List<Tile> GetNeighbours(Tile currentTile)
    {
        List<Tile> neighbourList = new List<Tile>();

        if (currentTile.pos.x - 1 >= 0)
        {
            // left
            neighbourList.Add(World.GetTile(currentTile.pos.x - 1, currentTile.pos.z));
            // left down
            if (currentTile.pos.z - 1 >= 0)
                neighbourList.Add(World.GetTile(currentTile.pos.x - 1, currentTile.pos.z - 1));
            // left up
            if (currentTile.pos.z + 1 < World.WorldSizeInTiles)
                neighbourList.Add(World.GetTile(currentTile.pos.x - 1, currentTile.pos.z + 1));
        }

        if (currentTile.pos.x + 1 < World.WorldSizeInTiles)
        {
            // right
            neighbourList.Add(World.GetTile(currentTile.pos.x + 1, currentTile.pos.z));
            // right down
            if (currentTile.pos.z - 1 >= 0)
                neighbourList.Add(World.GetTile(currentTile.pos.x + 1, currentTile.pos.z - 1));
            // right up
            if (currentTile.pos.z + 1 < World.WorldSizeInTiles)
                neighbourList.Add(World.GetTile(currentTile.pos.x + 1, currentTile.pos.z + 1));
        }

        // down
        if (currentTile.pos.z - 1 >= 0)
            neighbourList.Add(World.GetTile(currentTile.pos.x, currentTile.pos.z - 1));
        // up
        if (currentTile.pos.z + 1 < World.WorldSizeInTiles)
            neighbourList.Add(World.GetTile(currentTile.pos.x, currentTile.pos.z + 1));

        return neighbourList;
    }
}
