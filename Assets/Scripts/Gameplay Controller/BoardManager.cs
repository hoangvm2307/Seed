using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] List<Transform>position = new List<Transform>();
    private void Update()
    {
        foreach(Transform pos in position)
        {
             
        }
        for (int i = 0; i < position.Count; i++) { }
    }
    private void Awake()
    {

    }
}//CLASS


























