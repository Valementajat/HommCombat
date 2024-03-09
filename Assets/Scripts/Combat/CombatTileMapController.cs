using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

public class CombatTilemapController : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject combatPlanePrefab; // Prefab of the plane you want to instantiate

    // Specify the relative path to your models folder within the "Resources" directory
    private string modelsFolder = "PrefabModels";

    private GameObject[] modelsToPlace;
    public CombatGridManager gridManager;

    private Vector3Int clickedTilePosition; // Variable to store the clicked tile position


    public int combatWidth = 16;
    public int combatHeight = 14;

    public void Initialize(int CombatWidth, int CombatHeight)
    {
        this.combatWidth = CombatWidth;
        this.combatHeight = CombatHeight;
        if (tilemap != null && combatPlanePrefab != null)
        {
            LoadModelsFromFolder(modelsFolder);
            StartCombat();
            SetInactiveTiles();
        }
        else
        {
            Debug.LogError("Tilemap or combatPlanePrefab not assigned in the Inspector.");
        }
    }

    void LoadModelsFromFolder(string folderPath)
    {
        // Load all prefabs from the specified folder within the "Resources" directory
         Object[] loadedModels = Resources.LoadAll("PrefabModels", typeof(Object));

        // Convert the array of loaded models to a List<GameObject>
        modelsToPlace = System.Array.ConvertAll(loadedModels, item => (GameObject)item);

    }

  

    void StartCombat()
    {
       

        // Set the Tilemap size
        SetTilemapSize(combatWidth, combatHeight);

        // Set the plane size
        SetPlaneSize();

        // Place a random number of models on the Tilemap
        PlaceRandomModels();
    }

    void SetTilemapSize(int width, int height)
    {
        tilemap.ClearAllTiles();

        tilemap.size = new Vector3Int(width, height, 0);
    }

    void SetPlaneSize()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3 cellSize = tilemap.cellSize + tilemap.cellGap;
        float planeWidth = bounds.size.x * cellSize.x / 10;
        float planeHeight = bounds.size.y * cellSize.y / 10;

        combatPlanePrefab.transform.localScale = new Vector3(planeWidth, 1f, planeHeight);
        SetPlanePosition();
    }
    void SetPlanePosition()
{
    BoundsInt bounds = tilemap.cellBounds;
   

    // Get the position of the bottom-left corner of the bottom-left tile
    Vector3 bottomLeftPosition = tilemap.CellToWorld(new Vector3Int(bounds.xMin , bounds.yMin, 0));

    // Adjust the position based on the plane's pivot offset
    bottomLeftPosition += new Vector3(combatPlanePrefab.GetComponent<Renderer>().bounds.extents.x, 0, combatPlanePrefab.GetComponent<Renderer>().bounds.extents.z);
    bottomLeftPosition.y -= 0.01f;
    // Set the position of the plane to the bottom-left corner of the bottom-left tile
    combatPlanePrefab.transform.position = bottomLeftPosition;
}
    

  void PlaceRandomModels()
{
    int childs = combatPlanePrefab.transform.childCount;
    for (int i = childs - 1; i >= 0; i--)
{
    GameObject.Destroy(combatPlanePrefab.transform.GetChild(i).gameObject);
}
    int numberOfModelsToPlace = Random.Range(2, 6);

    List<GameObject> shuffledModels = new List<GameObject>(modelsToPlace);
    ShuffleList(shuffledModels);

    int modelsPlaced = 0;

    List<Vector3Int> usedPositions = new List<Vector3Int>();

    while (modelsPlaced < numberOfModelsToPlace)
    {
        int x = Random.Range(2, tilemap.size.x - 2);
        int y = Random.Range(0, tilemap.size.y);

        Vector3Int tilePosition = new Vector3Int(x, y, 0);

        // Check if the position is not already occupied and follows the rules
        if (!usedPositions.Contains(tilePosition) && IsValidPosition(x))
        {
            GameObject modelToPlace = shuffledModels[modelsPlaced % shuffledModels.Count];
            ObjectSize objectSize = modelToPlace.GetComponent<ObjectSize>();

            if (modelToPlace != null)
            {
                
                // Get the x and z scales of the placeable model
                float modelXScale = objectSize.GetWidth();
                float modelZScale = objectSize.GetHeight();
             

                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f);
                objectSize.SetOrientation(randomRotation);
                // Switch x and y scales based on rotation
                if (randomRotation.eulerAngles.y == 90f || randomRotation.eulerAngles.y == 270f) 
                {
                    float temp = modelXScale;
                    modelXScale = modelZScale;
                    modelZScale = temp;
                }
                // Calculate the total width and height occupied by the model
                float totalWidth = modelXScale;
                float totalHeight = modelZScale;

                // Check if the model exceeds the tile size
                if (x + totalWidth <= tilemap.size.x - 2)
                {
                    tilemap.SetTile(tilePosition, null);

                    // Adjust placement based on model size
                    Vector3 centerPosition = tilemap.GetCellCenterWorld(tilePosition);
                    centerPosition.x -= (totalWidth - 1) / 2f;
                    centerPosition.z += (totalHeight - 1) / 2f;
                    
                    GameObject modelInstance = Instantiate(modelToPlace, centerPosition, randomRotation);
                    modelInstance.transform.parent = combatPlanePrefab.transform;
                    modelsPlaced++;

                    MarkUsedPositions(tilePosition, objectSize,usedPositions );
                }
            }
        }
    }
}

void MarkUsedPositions(Vector3Int basePosition, ObjectSize objectSize, List<Vector3Int> usedPositions)
{
    int width = objectSize.GetWidth();
    int height = objectSize.GetHeight();
    Quaternion orientation = objectSize.GetOrientation();
    if (orientation.eulerAngles.y == 90 || orientation.eulerAngles.y == 270)
        {
            int temp = width;
            width = height;
            height = temp;
        }
    
    for (int offsetX = 0; offsetX < width; offsetX++)
    {
        for (int offsetY = 0; offsetY < height; offsetY++)
        {
            // Calculate the current tile based on the bottom-right assumption
            Vector3Int currentTile = new Vector3Int(
                basePosition.x -  offsetX,
                basePosition.y  + offsetY,
                0
            );
            if (InTheGrid(currentTile))
            {
                CombatGridManager.Instance.MarkPositionAsOccupied(currentTile);
                usedPositions.Add(currentTile);  // Add the position to the usedPositions list
            }

        }
    }
}


bool IsValidPosition(int x)
{
    // Add any additional rules for valid positions here
    return x >= 2 && x < tilemap.size.x - 2;
}
bool InTheGrid(Vector3Int currentTile)
{
    int x = tilemap.size.x;
    int y = tilemap.size.y;

    if (currentTile.x < 0 || currentTile.x >= x || currentTile.y < 0 || currentTile.y >= y)
    {

        return false;
    }

    return true;
}


    // Function to shuffle a list
    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

 public void SetInactiveTiles()
{
    BoundsInt bounds = tilemap.cellBounds;

    for (int x = bounds.xMin; x < bounds.xMax; x++)
    {
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            // Check if the tile is free (not occupied)
            
                // Load the Tile from the Resources folder
                Tile inactiveTile = Resources.Load<Tile>("Sprites/GroundTileInactive");

                // Set the tile to the inactive tile
                tilemap.SetTile(tilePosition, inactiveTile);
            
        }
    }
}

public void SetActiveTiles(Vector3Int baseTilePosition, int speed)
{
    BoundsInt bounds = tilemap.cellBounds;

    for (int x = bounds.xMin; x < bounds.xMax; x++)
    {
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            // Check if the tile is free (not occupied)
            if (!CombatGridManager.Instance.IsPositionOccupied(tilePosition))
            {
                // Check if the tile is within the creature's movement range based on speed
                if (IsTileWithinRange(baseTilePosition, tilePosition, speed))
                {
                    // Load the Tile from the Resources folder
                    Tile activeTile = Resources.Load<Tile>("Sprites/GroundTileActive");

                    // Set the tile to the active tile
                    tilemap.SetTile(tilePosition, activeTile);
                }
            }
        }
    }
}

    public bool IsTileWithinRange(Vector3Int baseTile, Vector3Int targetTile, int range)
    {
     
        int distance = Mathf.Abs(targetTile.x - baseTile.x) + Mathf.Abs(targetTile.y - baseTile.y);
        return distance <= range;
    }




}
