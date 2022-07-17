using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEffect : MonoBehaviour
{
    private Tree Tree; //Normalize Tree Status in GroundController
    private bool hitLeaf1, hitLeaf2, hitLeaf3,hitLeaf17,hitLeaf20,
        hitLeaf24,hitLeaf27,hitLeaf33,hitLeaf10,hitLeaf43, hitLeaf44;
    void Start()
    {
        Tree = GetComponentInParent<Tree>();
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            Vector2 treePos = Tree.transform.position;
            GameObject hitObject = hit.collider.gameObject;
            LeafGround LeafGround = hitObject.GetComponent<LeafGround>();
            if (hitObject.CompareTag("Leaf"))
            {
                Tree.touchedLeaf = true;
          
                if (LeafGround.leafType == LeafType.Leaf1
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.leafRate = LeafGround.OPM;
                    hitLeaf1 = true;
                }//incr Oxy
                else
                {
                    hitLeaf1 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf44
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.leafRate = LeafGround.OPM;
                    hitLeaf44 = true;
                }//incr Oxy
                else
                {
                    hitLeaf44 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf2
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf2 = true;
                    Tree.chanceOfSurvival = 0.75f;
                }//forecaster
                else
                {
                    hitLeaf2 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf3
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf3 = true;
                    Tree.fireResis = LeafGround.fire_Resis;
                }//fireResis
                else
                {
                    hitLeaf3 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf17
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Purple)
                {
                    hitLeaf17 = true;
                    Tree.radResis = LeafGround.Rad_Resis;
                }//radiation resis
                else
                {
                    hitLeaf17 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf20
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf20 = true;
                    Tree.gasResis = LeafGround.Gas_Resis;
                }//gas emission resis
                else
                {
                    hitLeaf20 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf24
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf24 = true;
                    Tree.biohazardResis = LeafGround.Biohazard_Resis;
                }//biohazard resis
                else
                {
                    hitLeaf24 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf27
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf27 = true;
                    Tree.hurricaneResis = LeafGround.Hurricane_Resis;
                }//hurricane resis
                else
                {
                    hitLeaf27 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf10
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf10 = true;
                    Tree.freezeResis = LeafGround.freeze_Resis;
                }//freeze resis
                else
                {
                    hitLeaf10 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf33
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf33 = true;
                    Tree.sandstormResis = LeafGround.SandStorm_Resis;
                }//sandstorm resis
                else
                {
                    hitLeaf33 = false;
                }
                if (LeafGround.leafType == LeafType.Leaf34
                   && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.droughtThreshold = 50;
                    Tree.onFireThreshold = 50;
                    Tree.overwateredThreshold = 50;
                    LeafGround.gameObject.SetActive(false);
                }//revive tree
                if (LeafGround.leafType == LeafType.Leaf39
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.hp = 90;
                }//+ chance survival
                if (LeafGround.leafType == LeafType.Leaf40
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    Tree.hp = 100;
                }//++ chance survival

                if (LeafGround.leafType == LeafType.Leaf43
                    && LeafGround.currently_Stand_On != CurrentlyStandOn.Blue)
                {
                    hitLeaf43 = true;
                    Tree.freezeResis = Tree.radResis = Tree.gasResis
                        = Tree.fireResis = Tree.waterResis = Tree.hurricaneResis = Tree.droughtResis = 1.70f;
                }//increase all types of resistance for surrounding trees
                else
                {
                    hitLeaf43 = false;
                }
            }
             
            #region hitObject not detecting - Normalize tree status
            else if (hitLeaf1 == true)
            {
                hitLeaf1 = false;
                Tree.leafRate = 1f;
            }
            else if(hitLeaf2 == true)
            {
                hitLeaf2 = false;
                Tree.chanceOfSurvival = 0f;
            }
            else if (hitLeaf3 == true)
            {
                hitLeaf3 = false;
                Tree.fireResis = 0f;
            }
            else if (hitLeaf10 == true)
            {
                hitLeaf10 = false;
                Tree.freezeResis = 0f;
            }
            else if (hitLeaf17 == true)
            {
                hitLeaf17 = false;
                Tree.radResis = 1f;
            }
            else if (hitLeaf20 == true)
            {
                hitLeaf20 = false;
                Tree.gasResis = 1f;
            }
            else if (hitLeaf24== true)
            {
                hitLeaf24 = false;
                Tree.biohazardResis = 1f;
            }
            else if (hitLeaf27 == true)
            {
                hitLeaf27 = false;
                Tree.hurricaneResis = 1f;
            }
            else if (hitLeaf33 == true)
            {
                hitLeaf33 = false;
                Tree.sandstormResis = 1f;
            }         
            else if (hitLeaf43 == true)
            {
                hitLeaf43 = false;
                Tree.lowTempResis = Tree.highTempResis = Tree.radResis = Tree.gasResis
            = Tree.fireResis = Tree.waterResis = Tree.hurricaneResis = Tree.droughtResis = 1f;
            }
            else if(hitLeaf44 == true)
            {
                hitLeaf44 = false;
                Tree.leafRate = 1;
            }
            #endregion
            if (!hitObject.CompareTag("Leaf"))
            {
                if (Tree.touchedLeaf)
                {
                    print("No leaf");
                    Tree.oneTime = true;
                    Tree.touchedLeaf = false;
                }
            }
        }
         

    }
    void NormalizeTreeStatus()
    {
        Tree.opm = 0.1f;
        Tree.leafRate = 2;
        Tree.lowTempResis = Tree.highTempResis = Tree.radResis = Tree.gasResis
            = Tree.fireResis = Tree.waterResis = Tree.hurricaneResis = Tree.droughtResis = 1f;
    }
}//CLASS










































