using System.Collections.Generic;

public static class TierTracker
{
    /// <summary>
    /// KEY: TIER_TYPE VALUE: TIER_VALUE
    /// </summary>
    public static Dictionary<TierTypes, int> Tiers = 
        Tiers = new Dictionary<TierTypes, int>()
        {
            { TierTypes.Tower, 1 },
            { TierTypes.Barn, 1 },
            { TierTypes.Tree, 1 },
            { TierTypes.Tractor, 1 }
        };

    public enum TierTypes
    {
        Tower,
        Barn,
        Tree,
        Tractor
    }
}