// CombatGameManager.cs (Parent Script)
using UnityEngine;

public class InitializeCombat : MonoBehaviour
{
    public CombatGridManager gridManager;
    public  CombatTilemapController tilemapController;

    public  CombatManager combatManager;
    private  CreatePlayerArmy createPlayerArmy;
    public CameraMovement cameraMovement;

    //public CombatManager combatManager; // Assuming you have a CombatManager script


    public int combatWidth = 16;
    public int combatHeight = 14;

    private Vector2 combatAreaSize;
    private int difficulty = 1;
  
  // void InitializeGame(Player player, Player enemyPlayer)
    public void InitializeGame(int passedDifficulty)
    {
         difficulty = passedDifficulty;

        if (gridManager != null && tilemapController != null)
        {
            gridManager.InitializeGrid(combatWidth, combatHeight);
            tilemapController.Initialize(combatWidth, combatHeight,difficulty);
            //gridManager.DisplayGridInConsole();
            combatAreaSize = new Vector2(combatWidth, combatHeight);
            cameraMovement.AdjustCamera();

            // For testing purposes
            Player player = new ();
            player.AddHero(new Hero("Necromancer"));
            createPlayerArmy = new CreatePlayerArmy();
            createPlayerArmy.CreateArmy(player, difficulty, true); 

            Player enemyplayer = new ();
            enemyplayer.AddHero(new Hero("Knight"));
            createPlayerArmy.CreateArmy(enemyplayer, difficulty, false); 
            // for testing purposes
         
            combatManager.InitializeCombat(player, enemyplayer);
            

        

        }
        else
        {
            Debug.LogError("GridManager or TilemapController not assigned in the Inspector.");
        }

        
    }
    public Vector2 GetCombatAreaSize()
    {
        return combatAreaSize;
    }

    public void RestartGame(){
        
        
        InitializeGame(difficulty);

    }
}
