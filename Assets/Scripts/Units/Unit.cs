using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




public abstract class Unit
{
        private System.Random random = new System.Random();

    public string Name { get; protected set; }
    public int HitPointsPerUnit { get; protected set; }
    public int AttackDamagePerUnit { get; protected set; }
    public int DefensePerUnit { get; protected set; }

    public int Initiative { get; protected set; }
    public int Speed { get; protected set; }

    public int Cost { get; protected set; }

    public int NumberOfUnits { get; set; }
    public int[] AttackDamageRange { get; protected set; }

    public int MaxHitPoints => HitPointsPerUnit * NumberOfUnits;
    public int HitPoints { get; protected set; }

    public int AttackDamage => AttackDamagePerUnit * NumberOfUnits;
    public int Defense => DefensePerUnit * NumberOfUnits;
    public bool IsPlayerUnit;

    //Combat
     public Vector3Int Position { get; set; }
    public Sprite UnitSprite { get; protected set; }

    public GameObject UnitModel { get; protected set; }
    public GameObject UnitObject { get;  set; }

    public bool Retaliated { get;  set; }
    public bool Attacker { get;  set; }


    public Unit(string name, int hitPoints, int attackDamage, int defense, int initiative, int speed, int cost, int numberOfUnits, int[] attackDamageRange)
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
        IsPlayerUnit = false;
        Position = Vector3Int.zero; 
        AttackDamageRange = attackDamageRange;
        Attacker = false;
    } 

    public virtual void Attack(Unit target, CombatManager combatManager)
    {
        Debug.Log(IsPlayerUnit + " : " + Name + ": Attack damage  "+  AttackDamage);
        Debug.Log(  "False : " + Name + ": Defence "+  target.Defense);

        /* float fuckshit = 1 + 0.05f * (AttackDamage - target.Defense);
        float test = Mathf.Ceil(NumberOfUnits * random.Next(AttackDamageRange[0], AttackDamageRange[1]) * fuckshit);
        Debug.Log("test: " + test); */
        int damageDealt = (int)Mathf.Max(0, Mathf.Ceil(NumberOfUnits * random.Next(AttackDamageRange[0], AttackDamageRange[1]) * Mathf.Max(1 + 0.05f * (AttackDamage - target.Defense))), 0 );
        Debug.Log(IsPlayerUnit + " : " + Name + ": Attacked with "+  damageDealt);
        target.TakeDamage(damageDealt, combatManager, this);
      
    }

    public virtual void Retaliate(Unit attacker, CombatManager combatManager)
    {
        if(!Retaliated && !Attacker){
            Debug.Log(IsPlayerUnit + " : " +  Name + ": Retaliated");
            Retaliated = true;
            Attack(attacker, combatManager);
        }

    }


//FIX
    public int TakeDamage(int amount, CombatManager combatManager, Unit attacker)
{

    Debug.Log(IsPlayerUnit + " : " + Name + " Hitpoints:  "+  HitPoints);

    HitPoints -= amount;

    // Ensure HitPoints don't go below zero
    HitPoints = Mathf.Max(0, HitPoints);
    Debug.Log(IsPlayerUnit + " : " + Name + " Hitpoints:  "+  HitPoints);
    int unitsLost = (MaxHitPoints - HitPoints) / HitPointsPerUnit;

    Debug.Log(Name + " unitsLost:  "+  unitsLost);

    
    // Update NumberOfUnits and clamp it to zero
    NumberOfUnits = Mathf.Max(0, NumberOfUnits - unitsLost);
    if (NumberOfUnits == 0){

        combatManager.RemoveDeadUnit(this);
    } else {
        Retaliate(attacker, combatManager);
    }


    Debug.Log($"Remaining Units: {NumberOfUnits}");
    
    return NumberOfUnits;
}


    public bool IsAdjacent(Vector3Int tilePosition)
    {
        // Get the current unit's tile position
        Vector3Int unitTilePosition = Position;

        // Check if the specified tile is adjacent to the unit's position
        return Mathf.Abs(tilePosition.x - unitTilePosition.x) <= 1 && Mathf.Abs(tilePosition.y - unitTilePosition.y) <= 1;
    }

}
