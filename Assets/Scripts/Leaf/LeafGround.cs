using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CurrentlyStandOn
{
    Brown,
    Blue,
    Green,
    Purple
}
public enum LeafType
{
    Leaf1,
    Leaf2,
    Leaf3,
    Leaf4,
    Leaf5,
    Leaf6,
    Leaf7,
    Leaf8,
    Leaf9,
    Leaf10,
    Leaf11,
    Leaf12,
    Leaf13,
    Leaf14,
    Leaf15,
    Leaf16,
    Leaf17,
    Leaf18,
    Leaf19,
    Leaf20,
    Leaf21,
    Leaf22,
    Leaf23,
    Leaf24,
    Leaf25,
    Leaf26,
    Leaf27,
    Leaf28,
    Leaf29,
    Leaf30,
    Leaf31,
    Leaf32,
    Leaf33,
    Leaf34,
    Leaf35,
    Leaf36,
    Leaf37,
    Leaf38,
    Leaf39,
    Leaf40,
    Leaf41,
    Leaf42,
    Leaf43,
    Leaf44,
    Leaf45,
    Leaf46,
    Leaf47,
    Leaf48
}
public class LeafGround : MonoBehaviour
{
    public LeafFunction leafFunc;
    public Vector2Int leafTemper;
    [TextArea(5, 20)]
    public string effectText;
    public string temperatureText;
    public string detailText;
    public GameObject ForecasterFX;
    public GameObject fcFXVessel;
    public GameObject inputField;

    [Header("Leaf Stats")]
    public LeafType leafType;
    public CurrentlyStandOn currently_Stand_On;
    public bool onBoard, onGerminated, cancelGerminate, noMoreGermination;
    public bool onGerminating, onCountdown;// used in GroundController
    public bool canCountDown;
    public bool readyToGerminate;// already true
    public bool deactivateEffect;
    public float rate;
    public float  opm ,hp, fire_Resis, rad_Resis, freeze_Resis, drought_Resis, 
        low_Temp_Resis, high_Temp_Resis, gas_Resis, water_Resis, insect_Resis, 
        hurricane_Resis, sandstorm_Resis,biohazard_Resis, germination_Time,spawnForecasterTime;
    public int leafQuantity;
    public bool readyToUse; //Used in ObjectClicker => GetLeafGround and LeafRoot
    GameplayController gcontr;
    #region GET SET EFFECT VALUE
    public float OPM
    {
        get { return opm * rate; }
        set { opm = value; }
    }
    public float HP
    {
        get { return hp * rate; }
        set { hp = value; }
    }
    public float Fire_Resis
    {
        get { return fire_Resis * rate; }
        set { fire_Resis = value; }
    }
    public float Rad_Resis
    {
        get { return rad_Resis * rate; }
        set { rad_Resis = value; }
    }
    public float Freeze_Resis
    {
        get { return freeze_Resis * rate; }
        set { freeze_Resis = value; }
    }
    public float Drought_Resis
    {
        get { return drought_Resis * rate; }
        set { drought_Resis = value; }
    }
    public float Low_Temp_Resis
    {
        get { return low_Temp_Resis * rate; }
        set { low_Temp_Resis = value; }
    }
    public float High_Temp_Resis
    {
        get { return high_Temp_Resis * rate; }
        set { low_Temp_Resis = value; }
    }
    public float Gas_Resis
    {
        get { return gas_Resis * rate; }
        set { gas_Resis = value; }
    }
    public float Water_Resis
    {
        get { return water_Resis * rate; }
        set { water_Resis = value; }
    }
    public float Insect_Resis
    {
        get { return insect_Resis * rate; }
        set { insect_Resis = value; }
    }
    public float Hurricane_Resis
    {
        get { return hurricane_Resis * rate; }
        set { hurricane_Resis = value; }
    }
    public float SandStorm_Resis
    {
        get { return sandstorm_Resis * rate; }
        set { sandstorm_Resis = value; }
    }
    public float Biohazard_Resis
    {
        get { return biohazard_Resis * rate; }
        set { biohazard_Resis = value; }
    }
    #endregion
    private GameObject[] leaf_List;

    [Header("Effect Duration")]
    [SerializeField] private float inspect_Effect_Duration;
    public float effectDuration;
    public float effectDurationRate;
    private float i,j,n;
    private void Start()
    {
        rate = 1; i = 1; j = 1; n = 1;
        readyToGerminate = true;
        leaf_List = GameObject.FindGameObjectsWithTag("Leaf List");
        canCountDown = true;
        effectDuration = inspect_Effect_Duration;
        noMoreGermination = false;
        gcontr = GameplayController.instance;

        if (leafFunc == LeafFunction.FORECASTER)
        {
            fcFXVessel = GameObject.Instantiate(ForecasterFX);
            fcFXVessel.transform.position = new Vector2(100, 100);
        }
    }
    private void Update()
    {
        foreach(GameObject leafList in leaf_List)
        {
            if (transform.gameObject.name == leafList.name + " Ground(Clone)") 
            {
                leafQuantity = leafList.GetComponent<LeafList>().leafQuantity;
                effectText = leafList.GetComponent<LeafList>().effectText;
                temperatureText = leafList.GetComponent<LeafList>().temperatureText;
                detailText = leafList.GetComponent<LeafList>().detailText;
                if (onBoard && leafQuantity == 1)
                {
                    leafList.GetComponent<LeafList>().isUsed = true;
                }
            }
        }
        if (onGerminating && !noMoreGermination) // onGerminating <=== GroundController
        {
            Germination();
        }
        if (onBoard && onCountdown && currently_Stand_On == CurrentlyStandOn.Brown) // onBoard <=== Objectclicker
        {
            CountdownDurationEffect();
            CheckingTemperatureThreshold();
        }
    }
    private void OnDisable()
    {
        if (onGerminating)
        {
            germination_Time = 120;
            onGerminating = false;
            effectDuration = inspect_Effect_Duration;
        }
        if(leafType == LeafType.Leaf36)
        {
            DayNight.instance.rate = 1f;
        }
        if(leafType == LeafType.Leaf15)
        {
            Climate.instance.rainRate = 6;
        }
        if(leafType == LeafType.Leaf23)
        {
            GameplayController.instance.reduceO2Rate = 1f;
        }
        if(leafType == LeafType.Leaf46)
        {
            Climate.instance.damageRate = 1;
        }
        if(leafType == LeafType.Leaf30)
        {
            GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
            foreach(GameObject gr in grounds)
            {
                gr.GetComponent<GroundController>().degradeRate = 1;
            }
        }
    }
    public void Germination() 
    {
        i -= Time.deltaTime;
        if(i <= 0)
        {
            germination_Time -= 1;
            i = 1;
        }
        if(germination_Time <= 0)
        {
            germination_Time = 0;
            noMoreGermination = true;
            IncreaseAmount();
            onGerminating = false;
        }
        print("Germinating");
    }
    public void SpawnForecaster()
    {
        n -= Time.deltaTime;
        if(n <= 0)
        {
            spawnForecasterTime -= 1;
            n = 1;
        }
        if (spawnForecasterTime <= 0)
        { 
            int i = Random.Range(0, gcontr.forecaster.Count);
            GameObject leaf = GameObject.Find(gcontr.forecaster[i]).gameObject;
            leaf.GetComponent<LeafList>().isUnlocked = true;
            leaf.GetComponent<LeafList>().leafQuantity = 2;
            leaf.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.SetActive(false);

            GameObject[] hiddenObject = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject leafPanel in hiddenObject)
            {
                if (leafPanel.name == gcontr.forecaster[i] + " Panel")
                {
                    leafPanel.GetComponent<LeafList>().leafQuantity = 2;
                    leafPanel.GetComponent<LeafList>().isUnlocked = true;
                    leafPanel.transform.GetChild(0).gameObject.SetActive(false);
                }
                if (leafPanel.name == gcontr.forecaster[i] + " Panel")
                {
                    leafPanel.GetComponent<LeafList>().leafQuantity = 2;
                    leafPanel.GetComponent<LeafList>().isUnlocked = true;
                    leafPanel.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            gcontr.forecaster.Remove(gcontr.forecaster[i]);
        }        
    }
    private void IncreaseAmount()
    {
        foreach (GameObject leafList in leaf_List)
        {
            if (transform.gameObject.name == leafList.name + " Ground(Clone)")
            {
                leafList.GetComponent<LeafList>().leafQuantity += 2;
                onGerminated = false;
                gameObject.SetActive(false);
            }
        }
    }// Increase amount after germinating
    private void CountdownDurationEffect()
    {
        j -= Time.deltaTime;
        if(j <= 0)
        {
            j = 1;
            if (leafQuantity == 1 && leafFunc != LeafFunction.FORECASTER)
            { // when placed ontable, the quantity reduce by 1
                effectDuration -= 1 * effectDurationRate;
            }
        }
        if(effectDuration <= 0)
        {
            deactivateEffect = true;
            effectDuration = 0;
            onBoard = false;
            foreach (GameObject leafList in leaf_List)
            {
                if (transform.gameObject.name == leafList.name + " Ground(Clone)")
                {
                    if (leafFunc == LeafFunction.FORECASTER || leafFunc == LeafFunction.HYBRID)
                    {
                        leafList.GetComponent<LeafList>().isUsed = false;
                    }
                }
            }              
            gameObject.SetActive(false);
        }
    }
    void CheckingTemperatureThreshold()
    {
        if(leafTemper.x > Temperature.instance.temperature || 
            leafTemper.y < Temperature.instance.temperature)
        {
            effectDurationRate = 1.5f;
        }
        else
        {
            effectDurationRate = 1f;
        }
    }
}//CLASS




























































