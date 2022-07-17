using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LeafEffect : MonoBehaviour
{
    private LeafGround leafGround;
    GameObject[] trees;
    UnityEvent myEvent;
    GameplayController gameplayController;
    WarningTabAnimation warningTabAnimation;
    [SerializeField] GameObject[] Ground;
    private void Start()
    {
        Ground = GameObject.FindGameObjectsWithTag("Row");
        leafGround = GetComponentInParent<LeafGround>();
        trees = GameObject.FindGameObjectsWithTag("Tree");
        gameplayController = GameplayController.instance;
        warningTabAnimation = WarningTabAnimation.instance;
        if (leafGround.leafFunc == LeafFunction.FORECASTER)
        {
            ParticleSystem fcFX = leafGround.ForecasterFX.GetComponent<ParticleSystem>();
            fcFX.Play();
        }
    }
    void Update()
    {
        ForecastClimate();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            Tree Tree = hitObject.GetComponent<Tree>();
            LeafGround Leaf = hitObject.GetComponent<LeafGround>();
            GroundController Ground = hitObject.GetComponent<GroundController>();
            switch (hitObject.name)
            {
                case "Tree(Clone)":
                    break;
            }
            if (hitObject.CompareTag("Leaf"))
            {
                switch (leafGround.leafType)
                {
                    case LeafType.Leaf4:
                        break;
                    case LeafType.Leaf38:
                        if (Leaf.currently_Stand_On == CurrentlyStandOn.Purple)
                            Leaf.rate = 1.25f;
                        break;
                    case LeafType.Leaf41:
                        Leaf.rate = (2/1.5f);
                        break;
                    case LeafType.Leaf36:
                        Leaf.germination_Time -= Time.deltaTime;
                        break;// -germi   

                    case LeafType.Leaf37:
                        Leaf.effectDurationRate = 0.75f;
                        break;// +duration

                    case LeafType.Leaf45:
                        Leaf.germination_Time = 0;
                        leafGround.gameObject.SetActive(false);
                        break;// eli germi
                }
                #region old leaf Effect
                //if (leafGround.leafType == LeafType.Leaf38)
                //{
                //    if (Leaf.currently_Stand_On == CurrentlyStandOn.Purple)
                //    {
                //        Leaf.rate = 1.25f;
                //    }
                //}//leaf on the purple ground increase effect
                //if (leafGround.leafType == LeafType.Leaf36)
                //{
                //    Leaf.germination_Time -= Time.deltaTime; //checked
                //}//reduce germination
                //if (leafGround.leafType == LeafType.Leaf37)
                //{
                //    Leaf.effectDurationRate = 0.75f;
                //}//lengthen duration
                //if (leafGround.leafType == LeafType.Leaf41)
                //{
                //    Leaf.rate = 1.75f;
                //}// strengthen effect surrounding leaves
                //if (leafGround.leafType == LeafType.Leaf45)
                //{
                //    Leaf.germination_Time = 0;
                //}//eliminate germination
                #endregion
            }
            if (hitObject.CompareTag("Ground"))
            {
                switch (leafGround.leafType)
                {
                    case LeafType.Leaf7:
                        Ground.groundTemper = 29;
                        break; //warm
                    case LeafType.Leaf8:
                        Ground.groundTemper = 59;
                        break;
                    case LeafType.Leaf16:
                        Ground.groundTemper = -10;
                        break;
                    case LeafType.Leaf14:
                        Ground.groundTemper = 7;
                        break; //cool
                    case LeafType.Leaf46:
                        Climate.instance.damageRate = 0.5f; //Ondisable
                        break;
                }
                #region old Ground affect
                //if (leafGround.leafType == LeafType.Leaf13)
                //{
                //    Ground.state = State.Normal;
                //    Ground.degrading_Time = 120;
                //    hitObject.GetComponent<SpriteRenderer>().sprite = Ground.brownGround;
                //}//humidify
                //if(leafGround.leafType == LeafType.Leaf6)
                //{
                //    Ground.state = State.Normal;
                //    Ground.humidThreshold = 100f;
                //    Ground.hitLeaf = false;
                //    hitObject.GetComponent<SpriteRenderer>().sprite = GameplayController.instance.brownGround;
                //    leafGround.gameObject.SetActive(false);
                //}//drying
                //if (leafGround.leafType == LeafType.Leaf7)
                //{
                //    Ground.groundTemper = 29;
                //}//warm surrounding ground
                //if (leafGround.leafType == LeafType.Leaf14)
                //{
                //    Ground.groundTemper = 7;
                //}//cool surrounding ground
                #endregion
            }
        }
        switch (leafGround.leafType)
        {
            case LeafType.Leaf9:
                if (leafGround.onBoard)
                {
                    DayNight.instance.rate = 0.55f; // Set normal in OnDisable
                }
                break;
            case LeafType.Leaf15:
                Climate.instance.rainRate = 8;// Set normal in OnDisable
                break;

            case LeafType.Leaf23:
                GameplayController.instance.reduceO2Rate = 0.75f;// Set normal in OnDisable
                break;
            case LeafType.Leaf48:
                GameObject[] Trees = GameObject.FindGameObjectsWithTag("Tree");
                for (int i = 0; i < Trees.Length; i++)
                {
                    if (Trees[i].activeInHierarchy)
                    {
                        Trees[i].GetComponent<Tree>().hp = 100;
                    }
                }
                leafGround.gameObject.SetActive(false);
                break;
            case LeafType.Leaf6:
                GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
                foreach(GameObject gr in ground)
                {
                    GroundController g = gr.GetComponent<GroundController>();
                    g.state = State.Normal;
                    g.humidThreshold = 100f;
                    if(!g.hitLeaf)
                    g.spriteRenderer.sprite = GameplayController.instance.brownGround;
                }
                leafGround.gameObject.SetActive(false);
                break; //dry
            case LeafType.Leaf13:
                GameObject[] ground1 = GameObject.FindGameObjectsWithTag("Ground");
                foreach (GameObject gr in ground1)
                {
                    GroundController g = gr.GetComponent<GroundController>();
                    g.state = State.Normal;
                    g.degrading_Time = 100;
                    if (!g.hitLeaf)
                        g.spriteRenderer.sprite = GameplayController.instance.brownGround;
                }
                leafGround.gameObject.SetActive(false);
                break; //humid
            case LeafType.Leaf30:
                GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
                foreach (GameObject gr in grounds)
                {
                    gr.GetComponent<GroundController>().degradeRate = 0.75f;
                }
                break;
            case LeafType.Leaf47:
                GameObject[] hiddenObject = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach(GameObject child in hiddenObject)
                {
                    if (child.name == "InputField")
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                leafGround.gameObject.SetActive(false);
                break;

        }
        #region old instant leaf
        //if (leafGround.leafType == LeafType.Leaf9)
        //{
        //    if (leafGround.onBoard)
        //    {
        //        DayNight.instance.rate = 0.55f; // Set normal in OnDisable
        //    }
        //}//increase daytime
        //if (leafGround.leafType == LeafType.Leaf23)
        //{
        //    Climate.instance.rainRate = 8;// Set normal in OnDisable
        //}
        //if (leafGround.leafType == LeafType.Leaf25)
        //{
        //    GameplayController.instance.reduceO2Rate = 0.75f;// Set normal in OnDisable
        //}
        //if (leafGround.leafType == LeafType.Leaf48)
        //{
        //    GameObject[] Trees = GameObject.FindGameObjectsWithTag("Tree");
        //    for (int i = 0; i < Trees.Length; i++)
        //    {
        //        if (Trees[i].activeInHierarchy)
        //        {
        //            Trees[i].GetComponent<Tree>().droughtThreshold = 50;
        //            Trees[i].GetComponent<Tree>().overwateredThreshold = 50;
        //            Trees[i].GetComponent<Tree>().onFireThreshold = 50;
        //        }
        //    }
        //    leafGround.gameObject.SetActive(false);
        //}
        #endregion
    }
    private void ForecastClimate()
    {
        Vector2 randPos = new Vector2(100, 100);
        Vector2 rootPos = leafGround.transform.position;
        switch (leafGround.leafType)
        {
            case LeafType.Leaf12:
                if (leafGround.onBoard && Climate.instance.isSnow)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf22:
                if (leafGround.onBoard && Climate.instance.isGreenGas)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf18:
                if (leafGround.onBoard && Climate.instance.isRadiation)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf5:
                if (leafGround.onBoard && Climate.instance.isFire)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf26:
                if (leafGround.onBoard && Climate.instance.isBiohazard)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf35:
                if (leafGround.onBoard && Climate.instance.isSandStorm)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf29:
                if (leafGround.onBoard && Climate.instance.isHeavyRain)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
            case LeafType.Leaf32:
                if (leafGround.onBoard && Climate.instance.isDrought)
                {
                    leafGround.fcFXVessel.transform.position = rootPos;
                }
                else
                    leafGround.fcFXVessel.transform.position = randPos;
                break;
        }
        #region old forecaster
        //if (leafGround.leafType == LeafType.Leaf22)
        //{
        //    if (leafGround.onBoard && Climate.instance.isGreenGas)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate greengas
        //if (leafGround.leafType == LeafType.Leaf18)
        //{
        //    if (leafGround.onBoard && Climate.instance.isRadiation)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }//anticipate radiation
        //}
        //if (leafGround.leafType == LeafType.Leaf26)
        //{
        //    if (leafGround.onBoard && Climate.instance.isBiohazard)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate biohazard
        //if (leafGround.leafType == LeafType.Leaf29)
        //{
        //    if (leafGround.onBoard && Climate.instance.isRain)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate heavy rain
        //if (leafGround.leafType == LeafType.Leaf12)
        //{
        //    if (leafGround.onBoard && Climate.instance.isSnow)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate snow
        //if (leafGround.leafType == LeafType.Leaf35)
        //{
        //    if (leafGround.onBoard && Climate.instance.isSandStorm)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate sandstorm
        //if (leafGround.leafType == LeafType.Leaf5)
        //{
        //    if (leafGround.onBoard && Climate.instance.isFire)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate fire
        //if (leafGround.leafType == LeafType.Leaf32)
        //{
        //    if (leafGround.onBoard && Climate.instance.isDrought)
        //    {
        //        leafGround.fcFXVessel.transform.position = leafGround.transform.position;
        //    }
        //}//anticipate drought
        #endregion
    }
}//CLASS
