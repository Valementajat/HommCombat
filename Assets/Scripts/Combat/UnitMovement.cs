using System.Collections;
using System.Collections.Generic;
using TMPro;
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

                            if (CheckEnemyUnitClicked(pos))
                            {
                                    currentUnit.Attacker = true;
                                    if(currentUnit is RangedUnit rangedUnit) {
                                        if (rangedUnit.rangedAttack){
                                        rangedUnit.Shoot(enemyUnit, combatManager);

                                        } else {
                                        currentUnit.Attack(enemyUnit, combatManager);
                                        }

                                    } else {

                                        currentUnit.Attack(enemyUnit, combatManager);
                                    }
                                    
                                    currentUnit.Attacker = false;

                                    tileClicked = true;  // Set the flag to exit the loop
                                    unitTurn = true; 
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
            //Debug.Log(raycastHit.point);
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

    public void MoveUnit(Unit unit, Vector3 targetPosition)
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


  private bool MoveUnitAttack(Unit unit, Unit enemyUnit)
{
    GameObject unitObject = unit.UnitObject;
    GameObject enemyUnitObject = enemyUnit.UnitObject;
    Debug.Log("Moving");

    // Calculate the angle based on where you click on the tile
    Vector3 clickPosition = GetCurrentmousePosition();

    Vector3Int clickOnTile = tilemap.WorldToCell(clickPosition);
    clickOnTile.z = 0; // Center of the clicked tile
    //Debug.Log("clickOnTile: " + clickOnTile);

    Vector3 clickedTileWorld  = tilemap.GetCellCenterWorld(clickOnTile);

    Vector3 direction = clickPosition - clickedTileWorld;

    // Normalize the direction vector
    direction.Normalize();

    // Calculate the angle between the normalized direction vector and the right vector (1, 0, 0)
    float angleDegrees = Vector3.Angle(Vector3.right, direction);

   // Check the cross product of the direction vector and the up vector (0, 1, 0)
    float crossProduct = Vector3.Cross(Vector3.right, direction).y;

    // Adjust the angle based on the cross product to cover the full 360 degrees
    if (crossProduct < 0)
    {
        angleDegrees = 360 - angleDegrees;
    }

    int roundedAngle = Mathf.RoundToInt(angleDegrees / 45) * 45 % 360;

    // Now, angleDegrees contains the angle of clicking on the tile
 

    // Calculate the new position based on the angle and move the unit
   int tileMoveDistance = 1;

// Calculate the new tile position based on the angle and fixed distance
Vector3Int newTilePos = clickOnTile;
Quaternion  targetRotation;   // Face left


switch (roundedAngle)
{
    case 0:
        targetRotation = Quaternion.Euler(0f, 90f, 0f);  // Face left
        newTilePos.x += tileMoveDistance;
        break;
    case 45:
        targetRotation = Quaternion.Euler(0f, 135f, 0f); // Face up-right

        newTilePos.x += tileMoveDistance;
        newTilePos.y -= tileMoveDistance;
        break;
    case 90:
        targetRotation = Quaternion.Euler(0f, 180f, 0f); // Face Down
        newTilePos.y -= tileMoveDistance;
        break;
    case 135:
        targetRotation = Quaternion.Euler(0f, 225f, 0f); // Face down-right
        newTilePos.x -= tileMoveDistance;
        newTilePos.y -= tileMoveDistance;
        break;
    case 180:
        targetRotation = Quaternion.Euler(0f, 270f, 0f); // Face right
        newTilePos.x -= tileMoveDistance;
        break;
    case 225:
        targetRotation = Quaternion.Euler(0f, 315f, 0f); // Face down-left

        newTilePos.x -= tileMoveDistance;
        newTilePos.y += tileMoveDistance;
        break;
    case 270:
        targetRotation = Quaternion.Euler(0f, 0f, 0f);   // Face Up
        newTilePos.y += tileMoveDistance;
        break;
    case 315:
        targetRotation = Quaternion.Euler(0f, 45f, 0f);  // Face up-left

        newTilePos.x += tileMoveDistance;
        newTilePos.y += tileMoveDistance;
        break;
        default:
        targetRotation = Quaternion.identity; // Default rotation if angle is not in the defined cases
        break;
}


     newTilePos.z = 0;

    if (IsValidMoveTile(newTilePos))
    {
        // Update the unit's properties
        Vector3 NewPos = tilemap.GetCellCenterWorld(newTilePos);
        NewPos.y = unitObject.transform.position.y;

        Vector3Int oldTilePos = tilemap.WorldToCell(unitObject.transform.position);

        unit.Position = newTilePos;
        unitObject.transform.position = NewPos;

         // Calculate the rotation to face the target direction
        unitObject.transform.rotation = targetRotation;
        enemyUnitObject.transform.rotation = targetRotation * Quaternion.Euler(0f, 180f, 0f);
        
        gridManager.SetPositionOccupied(oldTilePos, false);
        gridManager.SetPositionOccupied(newTilePos, true);

        tileMapController.SetInactiveTiles();
        return true;
    } 
 
    return false;
}

    private bool CheckEnemyUnitClicked(Vector3 tilePosition)
    {
        Vector3Int tilePos = tilemap.WorldToCell(tilePosition);
        tilePos.z = 0;
        currentUnit = combatManager.currentUnit;
        // Check if there is an enemy unit at the specified tile position
        foreach (Unit unit in combatManager.turnOrder)
        {
            if (!unit.IsPlayerUnit && unit.Position == tilePos)
            {
                Tile tile = tilemap.GetTile<Tile>(tilePos);
                if (tile != null && tile.sprite.name == "GroundTileAttackV2")
                {
                    enemyUnit = unit;
                    if (currentUnit is RangedUnit rangedUnit && rangedUnit.rangedAttack){
                        return true;
                    //Vector3 adjacentWorld = tilemap.CellToWorld(adjacentTilePosition);
                       
                    } else {
                         if(MoveUnitAttack(currentUnit, enemyUnit)){
                            return true;

                        }
                    }
                    
                } else {
                    Debug.Log("Attack enemy failed");
                }
                return false;
            }
        }

        return false;
    }
    public bool IsValidMoveTile(Vector3Int tilePosition)
    {
        // Check if the tile is a valid move tile
        Tile tile = tilemap.GetTile<Tile>(tilePosition);
        return tile != null && tile.sprite.name == "GroundTileActive";
    }

    public bool IsValidMoveTileAi(Vector3Int tilePosition)
{
    // Check if the tile position is not occupied in combatGridManager
    return !gridManager.IsPositionOccupied(tilePosition);
}

}

