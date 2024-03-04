using UnityEngine;


public class Archlich  : RangedUnit
{
    public Archlich (int numberOfUnits) : base("Archlich ", 55, 19, 19, 10, 3, 850, numberOfUnits, 6)
    {
        // Additional setup specific to Infantry
                UnitSprite = Resources.Load<Sprite>("Sprites/ArchlichSprite");

    }

    // You can add specific methods or properties for Infantry
}