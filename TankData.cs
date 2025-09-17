using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Tanks/TankData", order = 1)]
public class TankData : ScriptableObject
{
    public string tankName;
    public float speed;
    public GameObject tankPrefab;
    public float health;
}
