// CombatGridManager.cs
using UnityEngine;

public class CombatGridManager : MonoBehaviour
{
    public static CombatGridManager Instance { get; private set; }

    private bool[,] gridOccupied; // To track occupied positions

    void Awake()
    {
        Instance = this;
    }

    public void InitializeGrid(int width, int height)
    {
        gridOccupied = new bool[width, height];
        // You can add more initialization logic here if needed
    }

    public bool IsPositionOccupied(Vector3Int position)
    {
        return gridOccupied[position.x, position.y];
    }

    public void SetPositionOccupied(Vector3Int position, bool occupied)
    {
        gridOccupied[position.x, position.y] = occupied;
    }

    // Methods for handling obstacles spanning multiple tiles

    public bool IsTileOccupied(int x, int y)
    {
        return gridOccupied[x, y];
    }

    public void SetTileOccupied(int x, int y, bool occupied)
    {
        gridOccupied[x, y] = occupied;
    }

public void MarkPositionAsOccupied(Vector3Int position)
    {
        gridOccupied[position.x, position.y] = true;
    }
    public bool AreTilesOccupied(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                if (IsTileOccupied(x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // Add more methods for grid-related logic as needed

    // Example method:
    public bool IsValidCombatPosition(Vector3Int position, int width, int height)
    {
        // Add additional validation logic if needed
        return position.x >= 2 && position.x + width <= gridOccupied.GetLength(0) - 2 &&
               position.y >= 0 && position.y + height <= gridOccupied.GetLength(1) &&
               !AreTilesOccupied(position.x, position.y, width, height);
    }

public void DisplayGridInConsole()
{
    string gridString = "";

    for (int y = gridOccupied.GetLength(1) - 1; y >= 0; y--)
    {
        for (int x = 0; x < gridOccupied.GetLength(0); x++)
        {
            char cellChar = gridOccupied[x, y] ? 'X' : 'O';
            gridString += cellChar + " ";
        }

        gridString += "\n"; // Newline for each row
    }

    Debug.Log(gridString);
}
}
