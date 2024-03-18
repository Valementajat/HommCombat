using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedUnit : Unit
{
    public int Ammo { get; protected set; }
    private System.Random random = new System.Random();
    
    public bool rangedAttack { get;  set; }


    public RangedUnit(string name, int hitPointsPerUnit, int attackDamagePerUnit, int defensePerUnit,int initiative, int speed, int cost , int numberOfUnits, int Ammo)
        : base(name, hitPointsPerUnit, attackDamagePerUnit, defensePerUnit,initiative,speed, cost, numberOfUnits, new int[] { 1, 1 })
    {
        this.Ammo = Ammo;
        this.rangedAttack = true; 

    }

    public virtual void Shoot(Unit target, CombatManager combatManager)
    {        
        int damageDealt = (int)Mathf.Max(0, Mathf.Ceil(NumberOfUnits * random.Next(AttackDamageRange[0], AttackDamageRange[1]) * Mathf.Max(1 + 0.05f * (AttackDamage - target.Defense))), 0 );
        string attackMessage = $"{(IsPlayerUnit ? "Player" : "Enemy")} {Name} shot {(target.IsPlayerUnit ? "Player" : "Enemy")} {target.Name} with {damageDealt} damage!";
        combatManager.LogCombatEvent(attackMessage);
        
        target.TakeRangedDamage(damageDealt,combatManager,this);
        Ammo--;
       
    }

     public bool IsAdjacentToEnemy( CombatManager combatManager)
    {
        // Get the current unit's tile position
        Vector3Int unitTilePosition = Position;

        // Check if the specified tile is adjacent to the unit's position
        foreach(Unit unit in combatManager.allUnitsInPlay){
            if(!unit.IsPlayerUnit){
                if(Mathf.Abs(unit.Position.x - unitTilePosition.x) <= 1 && Mathf.Abs(unit.Position.y - unitTilePosition.y) <= 1){
                    return true;
                }

            }
        
        }
        return false;
    }

}