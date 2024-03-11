using System.Collections;
using UnityEngine;

public class Ai : MonoBehaviour
{
    public CombatManager combatManager;

   

   public void AiTurn(Unit currentUnit){

    

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
  
}
