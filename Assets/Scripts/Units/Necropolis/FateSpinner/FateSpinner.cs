using UnityEngine;


public class FateSpinner : Unit
{
    public FateSpinner(int numberOfUnits) : base("FateSpinner", 150, 27, 28, 11, 6, 1600, numberOfUnits)
    {
        // Additional setup specific to Infantry
                UnitSprite = Resources.Load<Sprite>("Sprites/FateSpinnerSprite");

    }

    // You can add specific methods or properties for Infantry
}