using UnityEngine;

public class Obstacle : MonoBehaviour
{
    new public string name = "New Obstacle";
    public string examineText = "A new obstacle";
    public int xSize = 10;
    public int zSize = 10;
    [TextArea(13, 5)]
    public string shape;

    public World.Walls[,] walls;

    private void Start()
    {
        walls = new World.Walls[xSize, zSize];
        ConvertToEnum(); // converts string shape to enum values in array
    }

    void ConvertToEnum()
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
