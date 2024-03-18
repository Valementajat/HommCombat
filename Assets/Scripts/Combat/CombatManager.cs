using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        string startCombat = "Start of combat";
        LogCombatEvent(startCombat);
        StartCoroutine(CombatLoop());
    }
  IEnumerator CombatLoop()
    {
       
       // Debug.Log("Testing combatLoop");
        while (!IsCombatOver())
        {
            currentUnit = GetCurrentUnit();


            // Set tiles active for unit
            if (currentUnit.IsPlayerUnit)
            {
                string turnMessage = "It's player unit:" + currentUnit.Name + " turn";
                LogCombatEvent(turnMessage);
                // Set tiles active for unit
                tileMapController.SetActiveTiles(currentUnit.Position, currentUnit.Speed);
                StartCoroutine(unitMovement.WaitForTileClick());
                unitMovement.InitializeBtns();


                // Handle player-controlled Hero actions (e.g., player input)
               // Debug.Log(currentTurnIndex+"Players turn");
               //yield return WaitForKeyPress();
               // unitMovement.HandleUnitMovement(currentUnit);

                yield return new WaitUntil(() => unitMovement.unitTurn);
                
            }
            else
            {
                string turnMessage = "It's enemy unit:" + currentUnit.Name + " turn";
                LogCombatEvent(turnMessage);
                // Handle enemy AI actions
                // WaitForKeyPress();
                enemyAi.AiTurn(currentUnit);
                //Debug.Log(currentTurnIndex + " AIs turn");
                
                yield return new WaitForSeconds(0.01f);  // Optional delay between turns
            }
            // Jesus christ is my NiGga
             // Optional delay between turns
            turnOrder.Remove(currentUnit);

            if(turnOrder.Count == 0){
            currentTurnIndex = 0;
            currentRoundIndex++;
            foreach(Unit unit in allUnitsInPlay){
            unit.Retaliated = false;

            }
            foreach (Unit unit in allUnitsInPlay)
            {
                turnOrder.Add(unit);
            }
            
            string RoundMessage = "Round: " + currentRoundIndex;
            LogCombatEvent(RoundMessage);
            Debug.Log("It is Round: " + currentRoundIndex);

            } else {
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            

            }
            tileMapController.SetInactiveTiles();
            unitMovement.ResetUnitMovement();
            // Switch to the next turn
            combatUI.InstantiateUnitDisplays(turnOrder);

            yield return new WaitForSeconds(0.01f); 

        }
        
        // Handle combat end (e.g., victory, defeat)
        string COmbatOver = "Combat is over";
        LogCombatEvent(COmbatOver);
    }

/*     private void WaitForKeyPress()
{
    while (!Input.GetKeyDown(KeyCode.Space))
    {
        // Wait until space key is pressed
    }
}
 */

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

public void RemoveDeadUnit(Unit deadUnit)
{
    // Remove the dead unit from the turnOrder
    turnOrder.Remove(deadUnit);
    allUnitsInPlay.Remove(deadUnit);

    // Destroy the game object associated with the dead unit
    if (deadUnit.UnitObject != null)
    {
        Destroy(deadUnit.UnitObject);
    }

    // Update the combat grid to make the dead unit's position available
    tileMapController.SetTileAvailable(deadUnit.Position);
}

private bool IsCombatOver()
    {
        // Check if there are no more player or enemy units in the allUnitsInPlay list
        bool noMorePlayerUnits = !allUnitsInPlay.Any(unit => unit.IsPlayerUnit);
        bool noMoreEnemyUnits = !allUnitsInPlay.Any(unit => !unit.IsPlayerUnit);

        // Combat is over if either all player units or all enemy units are defeated
        return noMorePlayerUnits || noMoreEnemyUnits;
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

public void LogCombatEvent(string message){
    combatUI.LogCombatEvent(message);

}
}
