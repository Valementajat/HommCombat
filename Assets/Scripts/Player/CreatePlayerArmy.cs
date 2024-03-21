using System;

public class CreatePlayerArmy
{
    private Random random = new Random();

   public void CreateArmy(Player player, int difficulty, bool isPlayer)
{
    // Adjust the composition based on the difficulty level
    int skeletonCount = 0;
    int archerCount = 0;
    switch (difficulty)
    {
        case 1: // Easy
            skeletonCount = isPlayer ? random.Next(3, 6) : random.Next(2, 5); // 3 to 5 for player, 2 to 4 for enemy
            archerCount = isPlayer ? random.Next(1, 2) : random.Next(1, 3); // 1 to 3 for player, 1 to 2 for enemy
            break;
        case 2: // Normal
            skeletonCount = random.Next(5, 8); // 5 to 7 for player, 4 to 6 for enemy
            archerCount = random.Next(2, 3); // 2 to 4 for player, 2 to 3 for enemy
            break;
        case 3: // Hard
            skeletonCount = isPlayer ? random.Next(6, 8) :  random.Next(7, 9); // 7 to 8 for player, 6 to 7 for enemy
            archerCount = isPlayer ?  random.Next(3, 5) : random.Next(3, 4); // 3 to 5 for player, 3 to 4 for enemy
            break;
        case 4: // Random
            /* skeletonCount = random.Next(3, 10); // Random number of skeletons
            archerCount = random.Next(1, 6); // Random number of archers */
            archerCount =  isPlayer ?  7 : 2;
            break;
        default:
            throw new ArgumentException("Invalid difficulty level.");
    }

    int totalUnits = skeletonCount + archerCount;
    int maxTotalUnits = 7;

    if (totalUnits > maxTotalUnits)
    {
        int excessUnits = totalUnits - maxTotalUnits;
        if (skeletonCount > archerCount)
        {
            skeletonCount -= excessUnits;
        }
        else
        {
            archerCount -= excessUnits;
        }
    }

    // Create skeleton units
    for (int i = 0; i < skeletonCount; i++)
    {
        Skeleton skeletonUnit = new Skeleton(random.Next(10, 25)); 
        player.Heroes[0].AddUnitToArmy(skeletonUnit);
    }

    // Create archer units
    for (int i = 0; i < archerCount; i++)
    {
        SkeletonArcher archerUnit = new SkeletonArcher(random.Next(6, 14)); 
        player.Heroes[0].AddUnitToArmy(archerUnit);
    }
}

}
