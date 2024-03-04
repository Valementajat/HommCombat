using UnityEngine;


public class Ghost : Unit
{
    public Ghost(int numberOfUnits) : base("Ghost", 16, 5, 4, 10, 5, 100, numberOfUnits)
    {
        // Additional setup specific to Infantry
                        UnitSprite = Resources.Load<Sprite>("Sprites/GhostSprite");

    }

    // You can add specific methods or properties for Infantry
}