// CreatePlayerArmy.cs
using System;

public class CreatePlayerArmy
{
    private Random random = new Random();

    public void CreateArmy(Player player, int count)
    {
        int randomValue = random.Next(1, 5);
        // Add 4 Skeletons to the player's army
        for (int i = 0; i < 1; i++)
        {
            
            Skeleton skeletonUnit = new(randomValue); 
            player.Heroes[0].AddUnitToArmy(skeletonUnit);
        }
            DeathSpider deathSpider = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(deathSpider);

            EbonSpider ebonySpider = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(ebonySpider);

            FateSpinner fateSpinner = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(fateSpinner);

            FateWeaver fateWeaver = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(fateWeaver);


            Ghost ghost = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(ghost);

            Spectre spectre = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(spectre);

            Lich lich = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(lich);

            Archlich archlich = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(archlich);

            SkeletonArcher skeletonArcher = new(random.Next(1, 5)); 
            player.Heroes[0].AddUnitToArmy(skeletonArcher);
    }
}
