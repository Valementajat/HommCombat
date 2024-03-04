using UnityEngine;


public class SkeletonArcher : RangedUnit
{
    public SkeletonArcher(int numberOfUnits) : base("Skeleton Archer", 5, 1, 2, 10, 4, 19, numberOfUnits, 8)
    {
        // Additional setup specific to Infantry
                UnitSprite = Resources.Load<Sprite>("Sprites/SkeletonArcherSprite");

    }

    // You can add specific methods or properties for Infantry
}