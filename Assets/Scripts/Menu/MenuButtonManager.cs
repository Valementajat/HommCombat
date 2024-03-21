using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    public InitializeCombat initializeCombat;
    public GameObject[] objectsToActivate;


    public GameObject menuCanvas;


    private void Start()
    {
        // Deactivate all game objects initially
        DeactivateObjects();
    }
    public void StartEasyGame()
    {
        // Set combat parameters for easy difficulty
        initializeCombat.combatWidth = 16;
        initializeCombat.combatHeight = 14;
        ActivateObjects();

        // Initialize the game
        initializeCombat.InitializeGame(1);
    }

    public void StartNormalGame()
    {
        // Set combat parameters for normal difficulty
        initializeCombat.combatWidth = 16;
        initializeCombat.combatHeight = 14;
        ActivateObjects();

        // Initialize the game
        initializeCombat.InitializeGame(2);
    }

    public void StartHardGame()
    {
        // Set combat parameters for hard difficulty
        initializeCombat.combatWidth = 18;
        initializeCombat.combatHeight = 16;
        ActivateObjects();

        // Initialize the game
        initializeCombat.InitializeGame(3);
    }

    public void StartRandomGame()
    {
        // Set random combat parameters
        initializeCombat.combatWidth = Random.Range(16, 25);
        initializeCombat.combatHeight = Random.Range(12, 19);
        ActivateObjects();
        // Initialize the game
        initializeCombat.InitializeGame(4);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    public void DeactivateObjects()
    {
        // Deactivate all game objects in the array
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }
    }

    public void ActivateObjects()
    {
        // Activate all game objects in the array
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
        menuCanvas.SetActive(false);
    }
}
