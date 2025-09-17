using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UpgradeData
{
    public string Name;
    public int UpgradeTier;
    public GameObject UpgradePrefab;
    public string Type;
    public float Value;

    public UpgradeData(string name,float value, int tier,string type, GameObject upgradePrefab)
    {
        Name = name;
        UpgradeTier = tier;
        Value = value;
        UpgradePrefab = upgradePrefab;
        Type = type;
    }
}
