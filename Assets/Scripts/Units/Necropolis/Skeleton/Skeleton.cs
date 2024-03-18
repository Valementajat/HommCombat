using System;

using UnityEngine;

public class Skeleton : Unit
{
        private static readonly System.Random random = new System.Random();

    public Skeleton(int numberOfUnits) : base("Skeleton", 4, 3, 2, 10, 4, 19, numberOfUnits, new int[] { 1, 1 })
    {
        //random.Next(1, 5)
        // Additional setup specific to Infantry
        UnitSprite = Resources.Load<Sprite>("Sprites/SkeletonSprite");
        UnitModel = Resources.Load<GameObject>("Models/Necropolis/Skeleton/SkeletonPrefab");
       
    }

}