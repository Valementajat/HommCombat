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

    void Start()
    {

        InitializeGame();
    }

  // void InitializeGame(Player player, Player enemyPlayer)
    void InitializeGame()
    {
        if (gridManager != null && tilemapController != null)
        {
            gridManager.InitializeGrid(combatWidth, combatHeight);
            tilemapController.Initialize(combatWidth, combatHeight);
            //gridManager.DisplayGridInConsole();
            combatAreaSize = new Vector2(combatWidth, combatHeight);
            cameraMovement.AdjustCamera();

            // For testing purposes
            Player player = new ();
            player.AddHero(new Hero("Necromancer"));
            createPlayerArmy = new CreatePlayerArmy();
            createPlayerArmy.CreateArmy(player, 2); 

            Player enemyplayer = new ();
            enemyplayer.AddHero(new Hero("Knight"));
            createPlayerArmy.CreateArmy(enemyplayer, 3); 
            // for testing purposes


            // Maybe pass the hero number 
            combatManager.InitializeCombat(player, enemyplayer);
            

           // CreatePlayerArmy createPlayerArmyScript = gameObject.AddComponent<CreatePlayerArmy>();
           // combatManager.StartCombat();

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
        
        
        InitializeGame();

    }
}
