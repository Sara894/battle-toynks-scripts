using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TierChances 
{
    public int tier1;
    public int tier2;
    public int tier3;

    public TierChances(int t1, int t2, int t3)
    {
        tier1 = t1;
        tier2 = t2;
        tier3 = t3;
    }
}
