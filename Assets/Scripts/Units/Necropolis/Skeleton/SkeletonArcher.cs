using UnityEngine;


public class SkeletonArcher : RangedUnit
{
    public SkeletonArcher(int numberOfUnits) : base("Skeleton Archer", 5, 1, 2, 10, 4, 19, numberOfUnits, 8)
    {

        //speed 4
        // Additional setup specific to Infantry
        UnitSprite = Resources.Load<Sprite>("Sprites/SkeletonArcherSprite");
        UnitModel = Resources.Load<GameObject>("Models/Necropolis/Skeleton/ArcherSkeletonPrefab");

    }

    // You can add specific methods or properties for Infantry
}