using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    List<Node> openList;
    List<Node> closedList;

    const int STRAIGHT_COST = 10; // 1 * 10
    const int DIAGONAL_COST = 14; // square root of 2 * 10
    const int MAX_DISTANCE = 30; // max distance to search for endNode

    Node[,] nodeMap = new Node[World.WorldSizeInTiles, World.WorldSizeInTiles];
    
    public Pathfinding()
    {
        
    }

    public List<Vector2> FindVectorPath(Vector3 startPos, Vector3 endPos)
    {
        if (startPos != endPos) // if not on same tile
        {
            List<Node> path = FindPath((int)startPos.x, (int)startPos.z, (int)endPos.x, (int)endPos.z); // find path
            if (path == null) { return null; }
            else
            {
                // convert tiles to vector2s
                List<Vector2> vectorPath = new List<Vector2>();

                for (int i = 1; i < path.Count; i++)
                    vectorPath.Add(new Vector2(path[i].x + 0.5f, path[i].z + 0.5f));

                return vectorPath;
            }
        }
        else
            return null;
    }

    public List<Node> FindPath(int startX, int startZ, int endX, int endZ)
    {
        if (!World.IsInWorld(new Vector2Int(startX, startZ)) || !World.IsInWorld(new Vector2Int(endX, endZ)))
            return null;

        if (Mathf.Abs(startX - endX) >= MAX_DISTANCE || Mathf.Abs(startZ - endZ) >= MAX_DISTANCE)
        {
            Debug.Log("target out of range");
            return null;
        }

        if (!World.tileMap[endX, endZ].canWalk)
        {
            int xDiff = Mathf.Abs(startX - endX);
            int zDiff = Mathf.Abs(startZ - endZ);
            if (xDiff > zDiff)
                endX = (startX < endX) ? endX - 1 : endX + 1;
            else
                endZ = (startZ < endZ) ? endZ - 1 : endZ + 1;
        }

        // initializes nodeMap for pathfinding
        for (int x = Mathf.Max(0, startX - MAX_DISTANCE); x < Mathf.Min(startX + MAX_DISTANCE, World.WorldSizeInTiles); x++)
            for (int z = Mathf.Max(0, startZ - MAX_DISTANCE); z < Mathf.Min(startZ + MAX_DISTANCE, World.WorldSizeInTiles); z++)
                nodeMap[x, z] = new Node(x, 0f, z, World.tileMap[x, z].canWalk, World.tileMap[x, z].wall);

        Node startNode = nodeMap[startX, startZ];
        Node endNode = nodeMap[endX, endZ];

        openList = new List<Node> { startNode };
        closedList = new List<Node>();

        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.calculateFCost();

        // main algorithm loop
        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList); // checks best candidate tile

            if (currentNode == endNode)
                return CalculatePath(endNode); // END OF PATH

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                // if statements check if neighbour is valid
                if (closedList.Contains(neighbour)) continue; // if already checked
                if (!neighbour.canWalk) // if neighbour is not walkable
                {
                    closedList.Add(neighbour);
                    continue;
                }
                Vector2Int difference = new Vector2Int(neighbour.x, neighbour.z) - new Vector2Int(currentNode.x, currentNode.z);

                if (!CanMove(currentNode, neighbour, difference))
                    continue;

                if (neighbour.x != currentNode.x && neighbour.z != currentNode.z)
                {
                    // if there is an obstacle between a diagonal neighbour, skip
                    if (!nodeMap[currentNode.x + difference.x, currentNode.z].canWalk // (x + difference.x, y)
                        || !nodeMap[currentNode.x, currentNode.z + difference.y].canWalk // (x, y + difference.y)
                        || nodeMap[currentNode.x + difference.x, currentNode.z].wall != World.Walls.O
                        || nodeMap[currentNode.x, currentNode.z + difference.y].wall != World.Walls.O)
                        continue;
                }
                
                // calculates cost of neighbour
                int gCost = currentNode.gCost + CalculateHCost(currentNode, neighbour);
                if (gCost < neighbour.gCost)
                {
                    neighbour.previous = currentNode;
                    neighbour.gCost = gCost;
                    neighbour.hCost = CalculateHCost(neighbour, endNode);
                    neighbour.calculateFCost();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;

        while (currentNode.previous != null)
        {
            path.Add(currentNode.previous);
            currentNode = currentNode.previous;
        }
        
        path.Reverse();
        return path;
    }

    public int CalculateHCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + STRAIGHT_COST * remaining;
    }

    Node GetLowestFCostNode(List<Node> tileList)
    {
        Node lowest = tileList[0];
        for (int i = 1; i < tileList.Count; i++)
            if (tileList[i].fCost < lowest.fCost)
                lowest = tileList[i];

        return lowest;
    }

    List<Node> GetNeighbours(Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();

        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(nodeMap[currentNode.x - 1, currentNode.z]); // left
            if (currentNode.z - 1 >= 0)
                neighbourList.Add(nodeMap[currentNode.x - 1, currentNode.z - 1]); // left down
            if (currentNode.z + 1 < World.WorldSizeInTiles)
                neighbourList.Add(nodeMap[currentNode.x - 1, currentNode.z + 1]);// left up
        }

        if (currentNode.x + 1 < World.WorldSizeInTiles)
        {
            neighbourList.Add(nodeMap[currentNode.x + 1, currentNode.z]); // right
            if (currentNode.z - 1 >= 0)
                neighbourList.Add(nodeMap[currentNode.x + 1, currentNode.z - 1]); // right down
            if (currentNode.z + 1 < World.WorldSizeInTiles)
                neighbourList.Add(nodeMap[currentNode.x + 1, currentNode.z + 1]); // right up
        }

        if (currentNode.z - 1 >= 0)
            neighbourList.Add(nodeMap[currentNode.x, currentNode.z - 1]); // down
        if (currentNode.z + 1 < World.WorldSizeInTiles)
            neighbourList.Add(nodeMap[currentNode.x, currentNode.z + 1]); // up

        return neighbourList;
    }

    bool CanMove(Node currentNode, Node neighbour, Vector2Int difference)
    {
        if (difference.x == 0 && difference.y > 0) // N
        {
            if (currentNode.wall == World.Walls.N || currentNode.wall == World.Walls.NE
                || currentNode.wall == World.Walls.NW || neighbour.wall == World.Walls.S
                || neighbour.wall == World.Walls.SW || neighbour.wall == World.Walls.SE)
                return false;
        }
        else if (difference.x == 0 && difference.y < 0) // S
        {
            if (currentNode.wall == World.Walls.S || currentNode.wall == World.Walls.SE
                || currentNode.wall == World.Walls.SW || neighbour.wall == World.Walls.N
                || neighbour.wall == World.Walls.NW || neighbour.wall == World.Walls.NE)
                return false;
        }
        else if (difference.x > 0 && difference.y == 0) // E
        {
            if (currentNode.wall == World.Walls.E || currentNode.wall == World.Walls.NE
                || currentNode.wall == World.Walls.SE || neighbour.wall == World.Walls.W
                || neighbour.wall == World.Walls.NW || neighbour.wall == World.Walls.SW)
                return false;
        }
        else if (difference.x < 0 && difference.y == 0) // W
        {
            if (currentNode.wall == World.Walls.W || currentNode.wall == World.Walls.NW
                || currentNode.wall == World.Walls.SW || neighbour.wall == World.Walls.E
                || neighbour.wall == World.Walls.NE || neighbour.wall == World.Walls.SE)
                return false;
        }
        else if (difference.x > 0 && difference.y > 0) // NE
        {
            if (currentNode.wall == World.Walls.N || currentNode.wall == World.Walls.NE
                || currentNode.wall == World.Walls.NW || currentNode.wall == World.Walls.E
                || currentNode.wall == World.Walls.SE
                || neighbour.wall == World.Walls.S || neighbour.wall == World.Walls.W
                || neighbour.wall == World.Walls.SE || neighbour.wall == World.Walls.SW
                || neighbour.wall == World.Walls.NW)
                return false;
        }
        else if (difference.x < 0 && difference.y > 0) // NW
        {
            if (currentNode.wall == World.Walls.N || currentNode.wall == World.Walls.NE
                || currentNode.wall == World.Walls.NW || currentNode.wall == World.Walls.W
                || currentNode.wall == World.Walls.SW
                || neighbour.wall == World.Walls.S || neighbour.wall == World.Walls.E
                || neighbour.wall == World.Walls.SE || neighbour.wall == World.Walls.SW
                || neighbour.wall == World.Walls.NE)
                return false;
        }
        else if (difference.x > 0 && difference.y < 0) // SE
        {
            if (currentNode.wall == World.Walls.S || currentNode.wall == World.Walls.SE
                || currentNode.wall == World.Walls.SW || currentNode.wall == World.Walls.E
                || currentNode.wall == World.Walls.NE
                || neighbour.wall == World.Walls.N || neighbour.wall == World.Walls.W
                || neighbour.wall == World.Walls.NE || neighbour.wall == World.Walls.NW
                || neighbour.wall == World.Walls.SW)
                return false;
        }
        else if (difference.x < 0 && difference.y < 0) // SW
        {
            if (currentNode.wall == World.Walls.S || currentNode.wall == World.Walls.SE
                || currentNode.wall == World.Walls.SW || currentNode.wall == World.Walls.W
                || currentNode.wall == World.Walls.NW
                || neighbour.wall == World.Walls.N || neighbour.wall == World.Walls.E
                || neighbour.wall == World.Walls.NE || neighbour.wall == World.Walls.NW
                || neighbour.wall == World.Walls.SE)
                return false;
        }
        
        return true;
    }
}
