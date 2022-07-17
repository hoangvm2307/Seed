using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDetection : MonoBehaviour
{
    public int leafIndex;
    public int leafOrder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Try to take index of leaf to modify
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            leafIndex = hitObject.GetComponent<LeafList>().index;
            leafOrder = hitObject.GetComponent<LeafList>().orderIndex;
            print(leafOrder);
        }
        else
        {
            leafIndex = 0;
            leafOrder = 0;
        }
        
    }
}
