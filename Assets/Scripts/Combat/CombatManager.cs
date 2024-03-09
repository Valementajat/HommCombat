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
    public List<Unit> turnOrder;

    public CombatUI combatUI;
    public CombatTilemapController tileMapController;

    public Ai enemyAi;
    public UnitPlacement unitPlacement; // Reference to your CombatManager script
    public UnitMovement unitMovement;

    private int currentTurnIndex;
    private int currentRoundIndex;

    public Unit  currentUnit;
  
    public void InitializeCombat(Player player, Player enemyplayer)
    {
        allUnits = new List<Unit>();
        allUnitsInPlay = new List<Unit>();
        turnOrder = new List<Unit>();


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
                    new Skeleton(3) { IsPlayerUnit = true },  
                };
            }
            else
            {
                // Handle the case when the player is not null
                // You can initialize playerUnits with units from the player's army
                playerUnits = player.Heroes[0].Army;
                foreach (Unit unit in playerUnits)
                {
                    unit.IsPlayerUnit = true;
                } // Assuming Player has an Army property
            }

        unitPlacement.InitializeUnits(playerUnits, enemyUnits);
        // Add units to the player's army
         // Example: 5 Skeletons in the player's army
        // Add more units as needed
        SortUnitsBySpeed();
         foreach (Unit unit in allUnitsInPlay)
        {
            turnOrder.Add(unit);
        }

        // Set the initial turn to the player's turn
        currentTurnIndex = 0;
        currentRoundIndex = 1;
      
        combatUI.InstantiateUnitDisplays(turnOrder);

        StartCoroutine(CombatLoop());
    }
  IEnumerator CombatLoop()
    {
        Debug.Log("Testing combatLoop");
        while (!IsCombatOver())
        {
            currentUnit = GetCurrentUnit();


            // Set tiles active for unit
            tileMapController.SetActiveTiles(currentUnit.Position, currentUnit.Speed);
            if (currentUnit.IsPlayerUnit)
            {
                StartCoroutine(unitMovement.WaitForTileClick());
                unitMovement.InitializeBtns();


                // Handle player-controlled Hero actions (e.g., player input)
                Debug.Log(currentTurnIndex+"Players turn");
               //yield return WaitForKeyPress();
               // unitMovement.HandleUnitMovement(currentUnit);

                yield return new WaitUntil(() => unitMovement.unitTurn);
                
            }
            else
            {
                // Handle enemy AI actions
                enemyAi.AiTurn(currentUnit);
                Debug.Log(currentTurnIndex + " AIs turn");
                
                yield return new WaitForSeconds(1f);  // Optional delay between turns
            }
            // Jesus christ is my NiGga
             // Optional delay between turns
            turnOrder.Remove(currentUnit);

            if(turnOrder.Count == 0){
            currentTurnIndex = 0;
            currentRoundIndex++;
            foreach (Unit unit in allUnitsInPlay)
            {
                turnOrder.Add(unit);
            }

            Debug.Log("It is Round: " + currentRoundIndex);

            } else {
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            

            }
            tileMapController.SetInactiveTiles();
            unitMovement.ResetUnitMovement();
            // Switch to the next turn
            combatUI.InstantiateUnitDisplays(turnOrder);

            yield return new WaitForSeconds(1f); 

        }
        
        // Handle combat end (e.g., victory, defeat)
        Debug.Log("Combat is over!");
    }

public void MoveCurrentUnitToBack()
{
    // Ensure the turnOrder list is not empty
    if (turnOrder.Count > 0)
    {
        // Remove the current unit from the front
        Unit currentUnit = GetCurrentUnit();

        // Add the current unit to the back
        turnOrder.Add(currentUnit);
        combatUI.InstantiateUnitDisplays(turnOrder);
    }
}
private bool IsCombatOver()
{
    // Combat is over if either all player units or all enemy units are defeated
    return playerUnits.Count == 0 || enemyUnits.Count == 0;
}



private Unit GetCurrentUnit()
{
    // Ensure currentTurnIndex is within the bounds of the allUnitsInPlay list
   

    // Return the unit at the current turn index
    return turnOrder[0];
}

 private void SortUnitsBySpeed()
    {
        
        
        // For management
        allUnits.AddRange(playerUnits);
        allUnits.AddRange(enemyUnits);

        // For currentTurn
        allUnitsInPlay.AddRange(playerUnits);
        allUnitsInPlay.AddRange(enemyUnits);
       
        // Sort units based on speed in descending order
      
        // Sort the list
        allUnitsInPlay.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        // After sorting
        
    // Other methods as before
}
}
