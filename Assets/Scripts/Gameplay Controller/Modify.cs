using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    public static Modify instance;
    public bool isModified, canRemove;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, int> leafDictionary = new Dictionary<string, int>();
    public GameObject slot1, slot2, slot3, slot4;
    int slot1Index, slot2Index, slot3Index, slot4Index;
    int slot1Order, slot2Order, slot3Order, slot4Order;
    public GameObject Holder;
    public int newLeaf;
    void Start()
    {
       
    }
    private void FixedUpdate()
    {
        slot1Index = slot1.GetComponent<SlotDetection>().leafIndex;
        slot2Index = slot2.GetComponent<SlotDetection>().leafIndex;
        slot3Index = slot3.GetComponent<SlotDetection>().leafIndex;
        slot4Index = slot4.GetComponent<SlotDetection>().leafIndex;

        slot1Order = slot1.GetComponent<SlotDetection>().leafOrder;
        slot2Order = slot2.GetComponent<SlotDetection>().leafOrder;
        slot3Order = slot3.GetComponent<SlotDetection>().leafOrder;
        slot4Order = slot4.GetComponent<SlotDetection>().leafOrder;
    }
    // Update is called once per frame
    void Update()
    {      
        if (canRemove)
        {
            DecreaseLeafQuantity();

            for (int i = 0; i < Holder.transform.childCount; i++)
            {
                GameObject child = Holder.transform.GetChild(i).gameObject;

                child.SetActive(false);
            }
            canRemove = false;
        }
    }
    public void ModifyLeaf()
    {
        isModified = true;
        canRemove = true;
        newLeaf = slot1Index + slot2Index + slot3Index + slot4Index;
        print(newLeaf);
    }
    public void DecreaseLeafQuantity() // when modifying, leaves quantity in slot 1,2,3,4 will be decreased
    {
        if (slot1Order > 0)
        {
            GameObject leaf1 = GameObject.Find("Leaf " + slot1Order).gameObject;
            GameObject leafPanel1 = GameObject.Find("Leaf " + slot1Order + " Panel").gameObject;
            leaf1.GetComponent<LeafList>().leafQuantity--;
            leafPanel1.GetComponent<LeafList>().leafQuantity--;
        }

        if (slot2Order > 0)
        {
            GameObject leaf2 = GameObject.Find("Leaf " + slot2Order).gameObject;
            GameObject leafPanel2 = GameObject.Find("Leaf " + slot2Order + " Panel").gameObject;
            leaf2.GetComponent<LeafList>().leafQuantity--;
            leafPanel2.GetComponent<LeafList>().leafQuantity--;
        }

        if (slot3Order > 0)
        {
            GameObject leaf3 = GameObject.Find("Leaf " + slot3Order).gameObject;
            GameObject leafPanel3 = GameObject.Find("Leaf " + slot3Order + " Panel").gameObject;
            leaf3.GetComponent<LeafList>().leafQuantity--;
            leafPanel3.GetComponent<LeafList>().leafQuantity--;
        }

        if (slot4Order > 0)
        {
            GameObject leaf4 = GameObject.Find("Leaf " + slot4Order).gameObject;
            GameObject leafPanel4 = GameObject.Find("Leaf " + slot4Order + " Panel").gameObject;
            leaf4.GetComponent<LeafList>().leafQuantity--;
            leafPanel4.GetComponent<LeafList>().leafQuantity--;
        }

    }
}
