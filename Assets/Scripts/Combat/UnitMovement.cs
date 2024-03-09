using System.Collections;
using System.Collections.Generic;
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
  

      public IEnumerator WaitForTileClick()
    {

        bool tileClicked = false;
        while (!tileClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = GetCurrentmousePosition();
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

        // Add new event listeners
    }
    public static Vector3 GetCurrentmousePosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;

        } else {

            return Vector3.zero;
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
        
        
            gridManager.SetPositionOccupied(oldTilePos, false);
            gridManager.SetPositionOccupied(newTilePos, true);


            targetPosition.y = unitObject.transform.position.y;
            // Update the unit's properties
            unit.Position = tilePos;

            unitObject.transform.position = targetPosition;
            tileMapController.SetInactiveTiles();

        
        // Additional logic for unit movement (e.g., animations, effects)
    }

    private void HandleAttack(Unit unit, Unit target)
{
    // Logic for handling the attack
    // ...

    attackComplete = true;
}
}
