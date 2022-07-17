using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeafData 
{
    public int level;
    public int health;
    public float[] position;
    public bool isUnlocked;
    public bool isUsed;
    public LeafData(LeafList LeafList)
    {
        isUnlocked = LeafList.isUnlocked;
        isUsed = LeafList.isUsed;
    }
}//CLASS






























