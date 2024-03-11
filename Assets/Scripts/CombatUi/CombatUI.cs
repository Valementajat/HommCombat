using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public CombatManager combatManager; // Reference to your CombatManager script
    public GameObject unitDisplayPrefab;
     public GameObject unitDisplayPrefabBig; // Reference to your UnitDisplayPrefab
    public Transform unitDisplayParent; // Parent transform for instantiated unit displays
    public TextMeshProUGUI  unitListText; // Reference to your UI Text element

    public Image  waitButton; // Reference to the Wait button
    public Image  defendButton; // Reference to the Defend button

   
  
    public void InstantiateUnitDisplays(List<Unit> turnOrder)
{
   

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

}
