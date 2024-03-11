using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedUnit : Unit
{
    public int Ammo { get; protected set; }

    public RangedUnit(string name, int hitPointsPerUnit, int attackDamagePerUnit, int defensePerUnit,int initiative, int speed, int cost , int numberOfUnits, int Ammo)
        : base(name, hitPointsPerUnit, attackDamagePerUnit, defensePerUnit,initiative,speed, cost, numberOfUnits, new int[] { 1, 1 })
    {
        this.Ammo = Ammo;
    }

    public virtual void Shoot(Unit target, CombatManager combatManager)
    {
        if (IsInRange(target))
        {
            int damageDealt = Mathf.Max(0, AttackDamage - target.Defense);
            target.TakeDamage(damageDealt,combatManager,this);
        }
       
    }

    protected bool IsInRange(Unit target)
    {
        return Mathf.Abs(target.NumberOfUnits - NumberOfUnits) <= Ammo;
    }
}