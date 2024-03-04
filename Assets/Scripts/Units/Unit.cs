using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class Unit
{
    public string Name { get; protected set; }
    public int HitPointsPerUnit { get; protected set; }
    public int AttackDamagePerUnit { get; protected set; }
    public int DefensePerUnit { get; protected set; }

    public int Initiative { get; protected set; }
    public int Speed { get; protected set; }

    public int Cost { get; protected set; }

    public int NumberOfUnits { get; set; }

    public int MaxHitPoints => HitPointsPerUnit * NumberOfUnits;
    public int HitPoints { get; protected set; }

    public int AttackDamage => AttackDamagePerUnit * NumberOfUnits;
    public int Defense => DefensePerUnit * NumberOfUnits;

    public Sprite UnitSprite { get; protected set; }


    public Unit(string name, int hitPoints, int attackDamage, int defense, int initiative, int speed, int cost, int numberOfUnits)
    {
        Name = name;
        HitPointsPerUnit = hitPoints;
        AttackDamagePerUnit = attackDamage;
        DefensePerUnit = defense;
        NumberOfUnits = numberOfUnits;
        HitPoints = HitPointsPerUnit * NumberOfUnits;
        Initiative = initiative;
        Speed = speed;
        Cost = cost;
        UnitSprite = null;
    } 

    public virtual void Attack(Unit target)
    {
        int damageDealt = Mathf.Max(0, AttackDamage - target.Defense);
        target.TakeDamage(damageDealt);
      
    }

    public virtual void Retaliate()
    {
    }

    public void TakeDamage(int amount)
    {
        int unitsLost = amount / HitPointsPerUnit;
        NumberOfUnits -= unitsLost;
        HitPoints -= amount;
        if (NumberOfUnits <= 0)
        {
            NumberOfUnits = 0;
        }
    }
}
