using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeafDisplay : MonoBehaviour
{
    public Leaf leaf;

    public Image leafImage;
    void Start()
    {
        leafImage.sprite = leaf.artWork;
    }    
}
