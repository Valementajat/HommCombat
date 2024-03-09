using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class UnitPlacement : MonoBehaviour
{




public CombatGridManager combatGrid;


public Tilemap tilemap;
public void InitializeUnits(List<Unit> playerUnits, List<Unit> enemyUnits)
    {
        int childs = tilemap.transform.childCount;

        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(tilemap.transform.GetChild(i).gameObject);
        }
        InitializePlayerUnits(playerUnits);
        InitializeEnemyUnits(enemyUnits);
    }

 private void InitializePlayerUnits(List<Unit> playerUnits)
    {
        foreach (Unit unit in playerUnits)
        {
            PlaceUnitRandomly(unit, true);
        }
    }

    private void InitializeEnemyUnits(List<Unit> enemyUnits)
    {
        foreach (Unit unit in enemyUnits)
        {
            PlaceUnitRandomly(unit, false);
        }
    }

    private void PlaceUnitRandomly(Unit unit, bool isPlayerUnit)
    {

        
        // Adjust the placement logic based on your game's requirements
        Vector3Int randomPosition = GetRandomEmptyPosition( isPlayerUnit);
        
       
            // Create the unit object
            GameObject unitPrefab = unit.UnitModel;
            Vector3 unitPosition = tilemap.GetCellCenterWorld(randomPosition);
            float unitHeight = unitPrefab.GetComponent<Renderer>().bounds.size.y;
            unitPosition.y += unitHeight / 2f;
            GameObject unitObject = Instantiate(unitPrefab, unitPosition, Quaternion.identity);
            unit.UnitObject = unitObject;
            // Set the unit on the grid
            combatGrid.MarkPositionAsOccupied(randomPosition);

            // Optionally, set unit-specific properties or behaviors
           /*  UnitBehavior unitBehavior = unitObject.GetComponent<UnitBehavior>();
            if (unitBehavior != null)
            {
                unitBehavior.SetUnit(unit, isPlayerUnit);
               } */
            unit.Position = randomPosition;
            unitObject.transform.parent = tilemap.transform;
            if (isPlayerUnit)
            {
                unitObject.transform.rotation = Quaternion.Euler(0f, 270f, 0f);  // Face right
            }
            else
            {
                unitObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);  // Face left
            }
       
    }

    private Vector3Int GetRandomEmptyPosition(bool isPlayerUnit)
    {
       
        while(true){
            List<Vector3Int> emptyPositions = new List<Vector3Int>();
            if(isPlayerUnit) {
                for (int x = 0; x < combatGrid.GridWidth- (combatGrid.GridWidth -2); x++)
                {
                    for (int y = 0; y < combatGrid.GridHeight; y++)
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);

                        if (!combatGrid.IsPositionOccupied(position))
                        {
                            emptyPositions.Add(position);
                           
                        }
                    }
                }
            } else {
                for (int x = combatGrid.GridWidth - 2; x < combatGrid.GridWidth; x++)
                {
                    for (int y = 0; y < combatGrid.GridHeight; y++)
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);

                        if (!combatGrid.IsPositionOccupied(position))
                        {
                            emptyPositions.Add(position);
                        }
                    }
                }

            }

            if (emptyPositions.Count > 0)
            {
                return emptyPositions[Random.Range(0, emptyPositions.Count)];
                
            }
        }

    }



}
