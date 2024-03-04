using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Player player;

    public List<Unit> playerUnits;

    public List<Unit> enemyUnits;
    List<Unit> allUnits;
    public List<Unit> allUnitsInPlay;
    public CombatUI combatUI; // Reference to your CombatManager script

    private int currentTurnIndex;

  
    public void InitializeCombat(Player player, Player enemyplayer)
    {
        allUnits = new List<Unit>();
        allUnitsInPlay = new List<Unit>();

        if(enemyplayer == null){
            enemyUnits = new List<Unit>
            {
                new Skeleton(3),  // Example: 3 Skeletons in the enemy's party
                // Add more enemy units as needed
            };
        } else {
             enemyUnits = enemyplayer.Heroes[0].Army; // Assuming Player has an Army property


        }

         if(player == null)
            {
                playerUnits = new List<Unit>
                {
                    new Skeleton(3),  
                };
            }
            else
            {
                // Handle the case when the player is not null
                // You can initialize playerUnits with units from the player's army
                playerUnits = player.Heroes[0].Army; // Assuming Player has an Army property
            }
        // Add units to the player's army
         // Example: 5 Skeletons in the player's army
        // Add more units as needed
        SortUnitsBySpeed();
        

        // Set the initial turn to the player's turn
        currentTurnIndex = 0;
        combatUI.InstantiateUnitDisplays();
    }
/*
    IEnumerator CombatLoop()
    {
        while (!IsCombatOver())
        {
            Unit currentUnit = GetCurrentUnit();

            // Perform actions for the current unit
            Debug.Log($"It's {currentUnit.Name}'s turn.");

            // Example: Attack a random enemy unit
            if (currentUnit is Hero)
            {
                // Handle player-controlled Hero actions (e.g., player input)
            }
            else
            {
                // Handle enemy AI actions
                Unit target = GetRandomTarget(currentUnit, player.Army);
                currentUnit.Attack(target);
            }

            yield return new WaitForSeconds(1f);  // Optional delay between turns

            // Switch to the next turn
            currentTurnIndex = (currentTurnIndex + 1) % (player.Army.Count + enemyUnits.Count);
        }

        // Handle combat end (e.g., victory, defeat)
        Debug.Log("Combat is over!");

        
    }
*/

 private void SortUnitsBySpeed()
    {
        
        
        // For management
        allUnits.AddRange(playerUnits);
        allUnits.AddRange(enemyUnits);

        // For currentTurn
        allUnitsInPlay.AddRange(playerUnits);
        allUnitsInPlay.AddRange(enemyUnits);

        // Sort units based on speed in descending order
        allUnitsInPlay.Sort((a, b) => b.Speed.CompareTo(a.Speed));

    }
    // Other methods as before
}
