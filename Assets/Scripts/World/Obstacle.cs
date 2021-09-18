using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleObject obj;
    public bool canWalkOver = false;
    public int xSize = 1;
    public int zSize = 1;
    [TextArea(13, 5)]
    public string shape = "X";

    public World.Walls[,] walls;

    private void Awake()
    {
        walls = new World.Walls[xSize, zSize];
        ConvertToEnum(); // converts string shape to enum values in array
    }

    void ConvertToEnum()
    {
        if (shape == "")
        {
            for (int x = 0; x < xSize; x++)
                for (int z = 0; z < zSize; z++)
                    walls[x, z] = World.Walls.X;
        }
        else
        {
            string[] rows = shape.Split('.');
            for (int x = 0; x < rows.Length; x++)
            {
                string[] column = rows[x].Split(',');
                for (int z = 0; z < column.Length; z++)
                    walls[x, z] = (World.Walls)System.Enum.Parse(typeof(World.Walls), column[z]);
            }
        }
    }
}
