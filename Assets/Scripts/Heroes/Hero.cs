using System.Collections.Generic;

public class Hero
{
    public string Name { get; private set; }
    public List<Unit> Army { get; private set; }

    public Hero(string name)
    {
        Name = name;
        Army = new List<Unit>();
    }

    public void AddUnitToArmy(Unit unit)
    {
        Army.Add(unit);
    }
}