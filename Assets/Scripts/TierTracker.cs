using System.Collections.Generic;

public static class TierTracker
{
    public static Dictionary<TierTypes, int> CurrentTier = 
        CurrentTier = new Dictionary<TierTypes, int>()
        {
            { TierTypes.Tower, 1 },
            { TierTypes.Barn, 1 },
            { TierTypes.Tree, 1 },
            { TierTypes.Tractor, 1 }
        };

    public static Dictionary<TierTypes, List<TierCost>> TierCosts =
        new Dictionary<TierTypes, List<TierCost>>()
        {
            { TierTypes.Tower, new List<TierCost>()
                { new TierCost(1, 0), new TierCost(2, 30), new TierCost(3, 150)} },
            { TierTypes.Barn, new List<TierCost>()
                { new TierCost(1, 0), new TierCost(2, 20), new TierCost(3, 250)} },
            { TierTypes.Tree, new List<TierCost>()
                { new TierCost(1, 0), new TierCost(2, 35), new TierCost(3, 100)} },
            { TierTypes.Tractor, new List<TierCost>()
                { new TierCost(1, 0), new TierCost(2, 40), new TierCost(3, 200)} },
        };

    public enum TierTypes
    {
        Tower,
        Barn,
        Tree,
        Tractor
    }
}

public class TierCost
{
    public int TierLevel;
    public int TierUpgradeCost;

    public TierCost(int level, int cost)
    {
        TierLevel = level;
        TierUpgradeCost = cost;
    }
}