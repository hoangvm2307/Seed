using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRoot : MonoBehaviour
{
    private LeafGround leafGround;
    private CurrentlyStandOn currently_Stand_On;
    void Start()
    {
        leafGround = GetComponentInParent<LeafGround>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            print(hit.collider.gameObject);
            GameObject hitObject = hit.collider.gameObject;
            GroundController Ground = hitObject.GetComponent<GroundController>();
            LeafGround Leaf = hitObject.GetComponent<LeafGround>();
            Tree Tree = hitObject.GetComponent<Tree>();
            if (hitObject.CompareTag("Ground"))
            {
                if(Ground.typeGround == TypeGround.Blue)
                {
                    leafGround.onGerminating = true;
                    leafGround.currently_Stand_On = CurrentlyStandOn.Blue;
                }
                if (Ground.typeGround == TypeGround.Brown)
                {
                    leafGround.onCountdown = true;
                    leafGround.currently_Stand_On = CurrentlyStandOn.Brown;
                    if(leafGround.leafType == LeafType.Leaf48)
                    {
                        //if(leafGround.onBoard)
                        //leafGround.gameObject.SetActive(false);
                    }
                }
                if(Ground.typeGround == TypeGround.Purple)
                {
                    leafGround.currently_Stand_On = CurrentlyStandOn.Purple;
                    if(leafGround.leafFunc == LeafFunction.HYBRID)
                    {
                        leafGround.SpawnForecaster();
                    }
                    else
                    {

                    }
                }
                if(Ground.typeGround == TypeGround.Green)
                {
                    leafGround.currently_Stand_On = CurrentlyStandOn.Green;
                }
            }
            if (hitObject.CompareTag("Leaf"))
            {
                //if(Leaf.leafType == LeafType.Leaf45)
                //{                   
                //    leafGround.germination_Time = 0;
                //    Leaf.gameObject.SetActive(false);
                //}
            }
            if (hitObject.CompareTag("Tree"))
            {
                if (leafGround.leafType == LeafType.Leaf42
                        && leafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.hp = 100;
                    Tree.sr.sprite = Tree.treeSprite;
                    leafGround.gameObject.SetActive(false);
                }//++ chance survival
            }
        }

    }
}//CLASS

































