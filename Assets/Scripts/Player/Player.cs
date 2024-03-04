using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public List<Hero> Heroes { get; private set; }

     public Player()
    {
        Heroes = new List<Hero>();
    }

    public void AddHero(Hero hero)
    {
        Heroes.Add(hero);
    }

     public void RemoveHero(Hero hero)
    {
        Heroes.Remove(hero);
    }

}
    // Add more methods or properties specific to the player, if needed
