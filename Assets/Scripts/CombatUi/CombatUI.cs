using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public CombatManager combatManager; // Reference to your CombatManager script
    public GameObject unitDisplayPrefab; // Reference to your UnitDisplayPrefab
    public Transform unitDisplayParent; // Parent transform for instantiated unit displays
    public TextMeshProUGUI  unitListText; // Reference to your UI Text element


   
    void Update()
    {
        UpdateUnitList();
    }

    public void InstantiateUnitDisplays()
{
    createUnitDisplay();
    // Ensure combatManager and references are not null
   
}

    void createUnitDisplay()
    {
        if (combatManager != null && unitDisplayPrefab != null && unitDisplayParent != null)
            {
                // Clear existing unit displays if needed
                ClearUnitDisplays();

                // Calculate the initial position for the first unit display
                float initialX = 0f;
                float initialY = 0f;

                // Iterate through all units and create displays
                int maxUnitsToShow = 11;

                for (int i = 0; i < Mathf.Min(maxUnitsToShow, combatManager.allUnitsInPlay.Count); i++)
                {
                    Unit unit = combatManager.allUnitsInPlay[i];

                    GameObject unitDisplay = Instantiate(unitDisplayPrefab, unitDisplayParent);
                    Image unitImage = unitDisplay.GetComponentInChildren<Image>();

                    // Set the unit image sprite based on your unit's data
                    unitImage.sprite = unit.UnitSprite;

                    // Set the position of the unit display using the Transform component
                    Transform unitDisplayTransform = unitDisplay.transform;
                    unitDisplayTransform.localPosition = new Vector3(initialX, initialY, 0f);

                    // Optionally, display the unit's name or other information
                    TextMeshProUGUI unitText = unitDisplay.GetComponentInChildren<TextMeshProUGUI>();
                    unitText.text = unit.NumberOfUnits.ToString(); // Assuming NumberOfUnits is a property in your Unit class

                    // Calculate the position for the next unit display
                    initialX += 0.45f * unitImage.rectTransform.sizeDelta.x;
                }

            }
    }


    void UpdateUnitList()
    {
        // Add any dynamic updates if needed
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
