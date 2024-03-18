using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ai : MonoBehaviour
{
    public CombatManager combatManager;
    public CombatGridManager combatGridManager;

    public CombatTilemapController tileMapController;
    public UnitMovement unitMovement;
    public Unit currentAiUnit;

    public Tilemap tilemap;

   public void AiTurn(Unit currentUnit){

   /*  currentAiUnit = currentUnit;
    bool canAttack = CheckAndAttackPlayerUnits(currentAiUnit);

        if (!canAttack)
        {
            // If no attack options are available, find the closest player unit and move towards it
            MoveTowardsClosestPlayerUnit(currentAiUnit);
        }  */
   }


   private void MoveTowardsClosestPlayerUnit(Unit currentAiUnit)
{
    Unit closestPlayerUnit = FindClosestPlayerUnit(currentAiUnit);

    if (closestPlayerUnit != null)
    {
        // Move the AI unit towards the closest player unit
        Vector3Int moveDirection = GetMoveDirection(currentAiUnit.Position, closestPlayerUnit.Position);
        Vector3Int newTilePos = currentAiUnit.Position + moveDirection;
        newTilePos.z = 0;

        // Check if the new tile position is valid for movement
        while (tileMapController.IsTileWithinRange(currentAiUnit.Position, newTilePos, currentAiUnit.Speed)
            && !unitMovement.IsValidMoveTileAi(newTilePos))
        {
            // If the position is occupied, find the closest free position
            newTilePos = FindClosestFreePosition(newTilePos, currentAiUnit.Position);

        }

        if (unitMovement.IsValidMoveTileAi(newTilePos))
        {
            unitMovement.MoveUnit(currentAiUnit, tilemap.GetCellCenterWorld(newTilePos));
        }
    }
}

private Vector3Int FindClosestFreePosition(Vector3Int currentPosition, Vector3Int originalPosition)
{
    // Implement logic to find the closest free position from the current position
    // You may use algorithms like breadth-first search or others to find the nearest available position

    
    int maxDistance = Mathf.Max(Mathf.Abs(currentPosition.x - originalPosition.x), Mathf.Abs(currentPosition.y - originalPosition.y));

    for (int distance = 1; distance <= maxDistance; distance++)
    {
        // Check positions in a square around the original position
        for (int x = -distance; x <= distance; x++)
        {
            for (int y = -distance; y <= distance; y++)
            {
                Vector3Int newPosition = originalPosition + new Vector3Int(x, y, 0);
                newPosition.z = 0;

                if (unitMovement.IsValidMoveTileAi(newPosition))
                {
                    // Found a free position, return it
                    return newPosition;
                }
            }
        }
    }

    // If no free position is found, return the original position
    return originalPosition;
}

    private Unit FindClosestPlayerUnit(Unit currentAiUnit)
    {
        // Implement logic to find the closest player unit within the movement range
        // You might want to consider factors like proximity and movement range
        // For now, let's find the closest player unit without considering movement range
        float closestDistance = float.MaxValue;
        Unit closestPlayerUnit = null;

        foreach (Unit playerUnit in combatManager.playerUnits)
        {
            float distance = Vector3Int.Distance(currentAiUnit.Position, playerUnit.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayerUnit = playerUnit;
            }
        }

        return closestPlayerUnit;
    }

    private Vector3Int GetMoveDirection(Vector3Int currentPos, Vector3Int targetPos)
    {
        // Calculate the direction to move towards the target position
        Vector3Int moveDirection = targetPos - currentPos;

        // Ensure the move direction is within the movement range (considering unit speed)
        moveDirection.x = Mathf.Clamp(moveDirection.x, -currentAiUnit.Speed, currentAiUnit.Speed);
        moveDirection.y = Mathf.Clamp(moveDirection.y, -currentAiUnit.Speed, currentAiUnit.Speed);

        return moveDirection;
    }

    private bool CheckAndAttackPlayerUnits(Unit currentAiUnit)
    {

        // Filter turnOrder to only contain player units
        List<Unit> playerUnits = combatManager.allUnitsInPlay.Where(unit => unit.IsPlayerUnit).ToList();

        // Iterate through player units to check if any is in attack range
        foreach (Unit unit in playerUnits)
        {
            if (tileMapController.IsTileWithinRange(currentAiUnit.Position, unit.Position, currentAiUnit.Speed))
            {

                // Find an adjacent unoccupied tile that is also within range
                Vector3Int adjacentTile = FindAdjacentUnoccupiedTileWithinRange(unit.Position, currentAiUnit.Speed);

                if (adjacentTile != Vector3Int.zero)
                {
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(adjacentTile);
                    Vector3 worldPosition2 = tilemap.GetCellCenterWorld(unit.Position);
                    Debug.Log(worldPosition);
                    Debug.Log(worldPosition2);


                    Vector3 direction = worldPosition - worldPosition2;
                    direction.Normalize();

                    // Calculate the angle from the direction vector
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    Debug.Log(angle);

                    angle = Mathf.Round(angle / 45) * 45;
                    Quaternion targetRotation = Quaternion.Euler(0f, angle , 0f);
                    currentAiUnit.UnitObject.transform.rotation = targetRotation;

                    // Rotate the player unit to face the AI unit
                    Quaternion enemyRotation = Quaternion.Euler(0f, angle + 180f, 0f);
                    unit.UnitObject.transform.rotation = enemyRotation;
                    // Move to the adjacent tile
                    unitMovement.MoveUnit(currentAiUnit, tilemap.GetCellCenterWorld(adjacentTile));
                   
                    // Attack the player unit
                    /* currentAiUnit.Attacker = true;
                    currentAiUnit.Attack(unit, combatManager);
                    currentAiUnit.Attacker = false; */
                    return true; // Attack successful
                }
                else
                {
                    Debug.Log("No adjacent unoccupied tile within range found");
                    return false; // No adjacent unoccupied tile within range found
                }
            }
        }

        return false; // No player units in range
    }


private Vector3Int FindAdjacentUnoccupiedTileWithinRange(Vector3Int unitPosition, int unitRange)
{
    // Define the adjacent directions (including diagonals)
    Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 1, 0),   // Up-right
        new Vector3Int(-1, 1, 0),  // Up-left
        new Vector3Int(1, -1, 0),  // Down-right
        new Vector3Int(-1, -1, 0), // Down-left
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    // Iterate through each direction to find adjacent tiles
    foreach (Vector3Int direction in directions)
    {
        Vector3Int adjacentTile = unitPosition + direction;

        // Check if the adjacent tile is within range and unoccupied
        if (tileMapController.IsTileWithinRange(unitPosition, adjacentTile, unitRange) && !combatGridManager.IsPositionOccupied(adjacentTile))
        {
            return adjacentTile; // Return the position of the first unoccupied adjacent tile found within range
        }
    }

    return Vector3Int.zero; // Return zero if no adjacent unoccupied tile within range is found
}


  
}
