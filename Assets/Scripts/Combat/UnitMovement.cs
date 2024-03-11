using System.Collections;
using UnityEngine;

using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class UnitMovement : MonoBehaviour
{
    public CombatTilemapController tileMapController;
    public CombatGridManager gridManager;
    public CombatManager combatManager;
    public Tilemap tilemap;
    public bool unitTurn = false;
    public bool attackComplete = false;

    public Button skipBtn;
    public Button waitBtn;
    private Unit enemyUnit;
    private Unit currentUnit;

      public IEnumerator WaitForTileClick()
    {

        bool tileClicked = false;
        while (!tileClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = GetCurrentmousePosition();
                if(pos.z == 100){

                    yield return null;
                } else {
                    Vector3Int tilePos = tilemap.WorldToCell(pos);
                
                    Unit unit = combatManager.currentUnit;
                    Vector3Int newTilePos = tilemap.WorldToCell(pos);
                    
                        // Try to get a tile from cell position
                        if (!gridManager.IsPositionOccupied(newTilePos))
                        {
                            if (tileMapController.IsTileWithinRange(unit.Position, tilePos,unit.Speed))
                            {
                                
                                pos = GetGridPosFromWorldPos(pos);
                                MoveUnit(unit, pos);
                                tileClicked = true;  // Set the flag to exit the loop
                                unitTurn = true;
                            } 
                        } else {

                            if (CheckEnemyUnitClicked(tilePos))
                            {
                                /*  currentUnit.Attacker = true;
                                    currentUnit.Attack(enemyUnit, combatManager);
                                    currentUnit.Attacker = false;

                                    tileClicked = true;  // Set the flag to exit the loop
                                    unitTurn = true; */
                                // Handle the case where an enemy unit is clicked
                                //HandleEnemyUnitClick(tilePos);
                            }
                        }
                    }

                
            }

            yield return null;
        }
    }

    public void InitializeBtns()
    {
        skipBtn.onClick.AddListener(SkipTurn);
        waitBtn.onClick.AddListener(WaitEvent);
       
           
     }
    
    private void SkipTurn()
        {
            // Handle the skip turn button click
            unitTurn = true;
        }    
    private void WaitEvent()
    {
        // Handle the skip turn button click
        combatManager.MoveCurrentUnitToBack();
        unitTurn = true;
    }    

    public void ResetUnitMovement()
    {
        // Remove existing event listeners
        skipBtn.onClick.RemoveAllListeners();
        waitBtn.onClick.RemoveAllListeners();
        
        // Reset variables
        unitTurn = false;
        attackComplete = false;

    }
    public static Vector3 GetCurrentmousePosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;

        } else {
            // Change to some arbita
            Vector3 Fail = new Vector3
            {
                z = 100
            };
            return Fail;
        }

    }
    public Vector3 GetGridPosFromWorldPos(Vector3 position){

        Vector3Int cellPos = tilemap.WorldToCell(position);
        position = tilemap.GetCellCenterWorld(cellPos);
        return position;


    }

    private void MoveUnit(Unit unit, Vector3 targetPosition)
    {
        Vector3Int tilePos = tilemap.WorldToCell(targetPosition);
        GameObject unitObject = unit.UnitObject;

        Vector3Int oldTilePos = tilemap.WorldToCell(unit.UnitObject.transform.position);
        Vector3Int newTilePos = tilemap.WorldToCell(targetPosition);
        newTilePos.z = 0;
            

            gridManager.SetPositionOccupied(oldTilePos, false);
            gridManager.SetPositionOccupied(newTilePos, true);


            targetPosition.y = unitObject.transform.position.y;
            // Update the unit's properties
            unit.Position = tilePos;

            unitObject.transform.position = targetPosition;
            tileMapController.SetInactiveTiles();

        
        // Additional logic for unit movement (e.g., animations, effects)
    }


  private bool MoveUnitAttack(Unit unit)
{
    GameObject unitObject = unit.UnitObject;
    Debug.Log("Moving");

    // Calculate the angle based on where you click on the tile
    Vector3 clickPosition = GetCurrentmousePosition();
    Vector3 clickOnTile = tilemap.WorldToCell(clickPosition) + new Vector3(0.5f, 0.5f, 0); // Center of the clicked tile
    Vector3Int clickOnTileInt = new Vector3Int(Mathf.FloorToInt(clickOnTile.x), Mathf.FloorToInt(clickOnTile.y), Mathf.FloorToInt(clickOnTile.z));
    Vector3 clickedTileWorld  = tilemap.CellToWorld(clickOnTileInt);
    Vector3 direction = clickPosition - clickedTileWorld;

    // Calculate the angle in radians
    float angleRadians = Mathf.Atan2(direction.y, direction.x);

    // Convert the angle to degrees
    float angleDegrees = Mathf.Rad2Deg * angleRadians;
    int roundedAngle = Mathf.RoundToInt(angleDegrees / 45) * 45 % 360;

    // Now, angleDegrees contains the angle of clicking on the tile
    Debug.Log("Angle: " + angleDegrees);
    Debug.Log("Angle: " + roundedAngle);

    //Vector3 clickedTileWorld = tilemap.CellToWorld(clickOnTile);
    /* float angle = Mathf.Atan2(clickOnTile.y - targetPosition.y, clickOnTile.x - targetPosition.x) * Mathf.Rad2Deg;

    // Round the angle to the nearest 90 degrees (assuming 90-degree tile layout)
    int roundedAngle = Mathf.RoundToInt(angle / 90) * 90;

    // Convert the rounded angle back to radians
    float finalAngle = roundedAngle * Mathf.Deg2Rad;

    // Calculate the new position based on the angle and move the unit
    Vector3 newPosition = targetPosition + new Vector3(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)) * tilemap.cellSize.x;
    Vector3Int newTilePos = tilemap.WorldToCell(newPosition);
    newTilePos.z = 0;

    if (IsValidMoveTile(newTilePos))
    {
        // Update the unit's properties
        unit.Position = newTilePos;
        unitObject.transform.position = newPosition;

        Vector3Int oldTilePos = tilemap.WorldToCell(unitObject.transform.position);
        gridManager.SetPositionOccupied(oldTilePos, false);
        gridManager.SetPositionOccupied(newTilePos, true);

        tileMapController.SetInactiveTiles();
        return true;
    } */

    return false;
}

    private bool CheckEnemyUnitClicked(Vector3Int tilePosition)
    {
        currentUnit = combatManager.currentUnit;
        // Check if there is an enemy unit at the specified tile position
        foreach (Unit unit in combatManager.turnOrder)
        {
            if (!unit.IsPlayerUnit && unit.Position == tilePosition)
            {
                Tile tile = tilemap.GetTile<Tile>(tilePosition);
                if (tile != null && tile.sprite.name == "GroundTileAttackV2")
                {
                    enemyUnit = unit;

                    //Vector3 adjacentWorld = tilemap.CellToWorld(adjacentTilePosition);
                    if(MoveUnitAttack(currentUnit)){
                        return true;

                    }
                    
                } else {
                    Debug.Log("Attack enemy failed");
                }
                return false;
            }
        }

        return false;
    }
    private bool IsValidMoveTile(Vector3Int tilePosition)
{
    // Check if the tile is a valid move tile
    Tile tile = tilemap.GetTile<Tile>(tilePosition);
    return tile != null && tile.sprite.name == "GroundTileActive";
}

}

