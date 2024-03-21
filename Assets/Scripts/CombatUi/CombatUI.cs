using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public CombatManager combatManager; // Reference to your CombatManager script
    public GameObject unitDisplayPrefab;

    public GameObject GameEndMenu;

     public GameObject unitDisplayPrefabBig; // Reference to your UnitDisplayPrefab
    public GameObject GameEndUnitDisplayPrefab; 
    public Transform unitDisplayParent;
    //public Transform gameEndUnitDisplayParent; // Parent transform for instantiated unit displays


    public TextMeshProUGUI  GameEndText;

    public TextMeshProUGUI logText; 
    private List<string> combatLog = new List<string>();
  
    public void InstantiateUnitDisplays(List<Unit> turnOrder)
{
   
    GameEndMenu.SetActive(false);
    createUnitDisplay(turnOrder);
    // Ensure combatManager and references are not null
   
}

    void createUnitDisplay(List<Unit> turnOrder)
    {
        if (combatManager != null && unitDisplayPrefab != null && unitDisplayPrefabBig != null && unitDisplayParent != null)
            {
                // Clear existing unit displays if needed
                ClearUnitDisplays();

                // Calculate the initial position for the first unit display
                float initialX = 0f;
                float initialY = 0f;

                // Iterate through all units and create displays
                int maxUnitsToShow = 7;

                //First Unit or unit in play 
                Unit unit = turnOrder[0];

                GameObject unitDisplay = Instantiate(unitDisplayPrefabBig, unitDisplayParent);

                Image unitImage = unitDisplay.transform.Find("Image").GetComponentInChildren<Image>();
                unitImage.sprite = unit.UnitSprite;

                TextMeshProUGUI unitText = unitDisplay.GetComponentInChildren<TextMeshProUGUI>();
                unitText.text = unit.NumberOfUnits.ToString();
                Sprite friendlyBackgroundSprite = Resources.Load<Sprite>("Sprites/IconBigV2");
                Sprite foeBackgroundSprite = Resources.Load<Sprite>("Sprites/IconBigV2EnemyV3");

                Image backgroundImage = unitDisplay.transform.Find("BackGroundIcon").GetComponent<Image>();
                backgroundImage.sprite = unit.IsPlayerUnit ? friendlyBackgroundSprite : foeBackgroundSprite;

                for (int i = 1; i < Mathf.Min(maxUnitsToShow, turnOrder.Count); i++)
            {
                unit = turnOrder[i];

                unitDisplay = Instantiate(unitDisplayPrefab, unitDisplayParent);

                // Set the position of the unit display using the Transform component
                Transform unitDisplayTransform = unitDisplay.transform;
                unitDisplayTransform.localPosition = new Vector3(initialX, initialY, 0f);

                // Set the unit image sprite based on your unit's data
                unitImage = unitDisplay.transform.Find("Image").GetComponentInChildren<Image>();
                unitImage.sprite = unit.UnitSprite;

                // Optionally, display the unit's name or other information
                unitText = unitDisplay.GetComponentInChildren<TextMeshProUGUI>();
                unitText.text = unit.NumberOfUnits.ToString(); // Assuming NumberOfUnits is a property in your Unit class

                // Set the background image based on whether the unit is friendly or a foe
                friendlyBackgroundSprite = Resources.Load<Sprite>("Sprites/IconSmall");
                foeBackgroundSprite = Resources.Load<Sprite>("Sprites/IconSmallEnemyV2");

                backgroundImage = unitDisplay.transform.Find("BackGroundImage").GetComponent<Image>();
                backgroundImage.sprite = unit.IsPlayerUnit ? friendlyBackgroundSprite : foeBackgroundSprite;

                // Calculate the position for the next unit display
                initialX += 0.6f * unitImage.rectTransform.sizeDelta.x;
            }

            }
    }


    
    void ClearUnitDisplays()
    {
        // Destroy existing unit displays in the unitDisplayParent
        foreach (Transform child in unitDisplayParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void LogCombatEvent(string eventMessage)
    {
        // Add the event message to the combat log
        combatLog.Add(eventMessage);

        if (combatLog.Count > 10)
    {
        // Remove the oldest log entries if the limit is exceeded
        combatLog.RemoveAt(0);
    }

        // Update the log text to display the updated combat log
        UpdateLogText();
    }

public void CleanCombatEvent()
    {
        // Add the event message to the combat log
        combatLog = new List<string>();

        // Update the log text to display the updated combat log
        UpdateLogText();
    }
    void UpdateLogText()
    {
        // Concatenate all log entries into a single string
        string logContent = string.Join("\n", combatLog);

        // Set the text content of the log Text component
        logText.text = logContent;

        // Scroll the log to the bottom to display the latest entry
        Canvas.ForceUpdateCanvases(); // Ensure UI layout is updated before scrolling
    }

     public void GameOver(List<Unit> allUnits, int result)
    {
        GameEndMenu.SetActive(true);
        string resultText = "";
        if (result == 0)
        {
            resultText = "Draw!";
        }
        else if (result == 1)
        {
            resultText = "Victory!";
        }
        else if (result == -1)
        {
            resultText = "Defeat!";
        }
        GameEndText.text = resultText;
        float EnemyInitialX = -312.6f;
        float EnemyInitialY = -366.5f;
        float PlayerIntialX = -312.6f;
        float PlayerIntialY = -150f;

        int playerDisplaysCreated = 0;
        int enemyDisplaysCreated = 0;

       //-150, -85,5

        // -265.1, -452
        if (GameEndUnitDisplayPrefab != null)
        {   foreach (Unit unit in allUnits)
            {
                
                if (!unit.IsPlayerUnit && (unit.AbsoluteMaxHitPoints / unit.HitPointsPerUnit) > unit.NumberOfUnits)
                {
                    int unitsLost = (unit.AbsoluteMaxHitPoints / unit.HitPointsPerUnit) - unit.NumberOfUnits;
                    GameObject unitDisplay = Instantiate(GameEndUnitDisplayPrefab, GameEndMenu.transform);
                
                    Image unitImage = unitDisplay.transform.Find("Image").GetComponent<Image>();
                    unitImage.sprite = unit.UnitSprite;
                    TextMeshProUGUI unitText = unitDisplay.GetComponentInChildren<TextMeshProUGUI>();
                    unitText.text = unitsLost.ToString();
                    

                    unitDisplay.transform.localPosition = new Vector3(EnemyInitialX, EnemyInitialY, 0f);
                   enemyDisplaysCreated++;
                   EnemyInitialX += 90f;
                   if(enemyDisplaysCreated == 4){
                        EnemyInitialY = -452f;
                        EnemyInitialX = -265.1f;
                   }
                    
                }else if (unit.IsPlayerUnit && (unit.AbsoluteMaxHitPoints / unit.HitPointsPerUnit) > unit.NumberOfUnits)
                {
                    int unitsLost = (unit.AbsoluteMaxHitPoints / unit.HitPointsPerUnit) - unit.NumberOfUnits;
                    GameObject unitDisplay = Instantiate(GameEndUnitDisplayPrefab, GameEndMenu.transform);
                
                    Image unitImage = unitDisplay.transform.Find("Image").GetComponent<Image>();
                    unitImage.sprite = unit.UnitSprite;
                    TextMeshProUGUI unitText = unitDisplay.GetComponentInChildren<TextMeshProUGUI>();
                    unitText.text = unitsLost.ToString();
                    

                    unitDisplay.transform.localPosition = new Vector3(PlayerIntialX, PlayerIntialY, 0f);
                    playerDisplaysCreated++; // Increment the count of player displays created
                    PlayerIntialX += 90f;
                    if(playerDisplaysCreated == 4){
                        PlayerIntialY = -235.5f;
                        PlayerIntialX = -265.1f;
                   }
                }
                
            }

            while (enemyDisplaysCreated < 7)
            {
                GameObject unitDisplay = Instantiate(GameEndUnitDisplayPrefab, GameEndMenu.transform);
                unitDisplay.transform.localPosition = new Vector3(EnemyInitialX, EnemyInitialY, 0f);
                EnemyInitialX += 90f;
                enemyDisplaysCreated++;
                Image unitImage = unitDisplay.transform.Find("Image").GetComponent<Image>();
                unitImage.gameObject.SetActive(false);
                
                 if(enemyDisplaysCreated == 4){
                        EnemyInitialY = -452f;
                        EnemyInitialX = -265.1f;
                   }
            }
            while (playerDisplaysCreated < 7)
            {
                GameObject unitDisplay = Instantiate(GameEndUnitDisplayPrefab, GameEndMenu.transform);
                unitDisplay.transform.localPosition = new Vector3(PlayerIntialX, PlayerIntialY, 0f);
                PlayerIntialX += 90f;
                playerDisplaysCreated++;
                Image unitImage = unitDisplay.transform.Find("Image").GetComponent<Image>();
                unitImage.gameObject.SetActive(false);

                if(playerDisplaysCreated == 4){
                        PlayerIntialY = -235.5f;
                        PlayerIntialX = -265.1f;
                }
            }
                        
        }else
        {
            Debug.LogError("GameEndUnitDisplayPrefab or gameEndUnitDisplayParent is not assigned!");
        }

    }

}
