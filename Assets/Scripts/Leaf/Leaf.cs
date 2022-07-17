using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Leaf", menuName = "Leaf")]
public class Leaf : ScriptableObject
{
    public string leafName;
    public string description;
    public Sprite artWork;

    public int amount;
    public int duration;

}
