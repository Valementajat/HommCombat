using System;

using UnityEngine;

public class Skeleton : Unit
{
        private static readonly System.Random random = new System.Random();

    public Skeleton(int numberOfUnits) : base("Skeleton", 4, 1, 2, 10, random.Next(1, 5), 19, numberOfUnits)
    {
        // Additional setup specific to Infantry
        UnitSprite = Resources.Load<Sprite>("Sprites/SkeletonSprite");
        UnitModel = Resources.Load<GameObject>("Models/Necropolis/Skeleton/SkeletonPrefab");
       
    }

    // You can add specific methods or properties for Infantry
}