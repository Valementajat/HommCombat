using UnityEngine;


public class DeathSpider : Unit
{
    public DeathSpider(int numberOfUnits) : base("DeathSpider", 28, 4, 4, 28, 5, 143, numberOfUnits)
    {
        // Additional setup specific to Infantry
        UnitSprite = Resources.Load<Sprite>("Sprites/DeathSpiderSprite");

    }

    // You can add specific methods or properties for Infantry
}