using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileHoverManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D activeTileCursor;
    public Texture2D attackCursor;

    public Tilemap tilemap;


   private Vector3Int hoveredTilePosition;
    //private Vector3Int originalTilePosition;
   // private Tile originalTile;

   private Tile newTile;
    private void Start()
    {
        newTile = Resources.Load<Tile>("Sprites/GroundTileActiveV2V1");
        SetDefaultCursor();
    }

    private void Update()
    {
        HandleTileHover();
    }
    private void HandleTileHover()
    {
        Tile tile = GetTileUnderMouse();
        if (tile != null && tile.sprite.name == "GroundTileActive" )
        {

            SetActiveTileCursor();
            // You can add additional logic or UI feedback for the active tile here
        }
        else
        {
            SetDefaultCursor();
            // You can reset any additional UI feedback for the active tile here
        }
    }
   
    private Tile GetTileUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 hitPoint = raycastHit.point;
            hoveredTilePosition = tilemap.WorldToCell(hitPoint);
            hoveredTilePosition.z = 0;

            Tile hitTile = tilemap.GetTile<Tile>(hoveredTilePosition);
            return hitTile;
            
        }

        return null;
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
       /*  if (originalTile != null)
        {
            tilemap.SetTile(originalTilePosition, originalTile);
            originalTile = null;
        } */
    }

    private void SetActiveTileCursor()
    {
        Cursor.SetCursor(activeTileCursor, Vector2.zero, CursorMode.Auto);
       /*  originalTilePosition = hoveredTilePosition;
        originalTile = tilemap.GetTile<Tile>(originalTilePosition);
        SetActiveTile(); */

    }

    private void SetActiveTile(){
        
        tilemap.SetTile(hoveredTilePosition, newTile);
    }
}
