
using UnityEngine;

public class Skeleton : Unit
{
    public Skeleton(int numberOfUnits) : base("Skeleton", 4, 1, 2, 10, 5, 19, numberOfUnits)
    {
        // Additional setup specific to Infantry
        UnitSprite = Resources.Load<Sprite>("Sprites/SkeletonSprite");
    }

    // You can add specific methods or properties for Infantry
}