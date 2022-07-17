using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Where
{
    GENEPANEL,
    MAIN
}
public enum LeafFunction
{
    FUNCTION,
    FORECASTER,
    HYBRID,
    RESISTANCE,
}
public enum Gene
{
    NONE,
    FIRE_RESIS,
    INCR_SOIL_TEMP,
    INCR_DAY,
    INCR_RAIN,
    DECR_SOIL_TEMP,
    RAD_RESIS,
    GAS_RESIS,
    BIO_RESIS,
    HUR_RESIS,
    SAND_RESIS,
    PURPLE_GROUND,
    INCR_CHANCE_SURVIVAL,
    GREATLY_INCR_CHANCE_SURVIVAL,
    INCR_O2,
    CHOOSE_TEMP,
    REVIVE_TREES
}
public class LeafList : MonoBehaviour
{
    public Where where;
    public LeafFunction leafFunc;
    public GameObject leafGround;
    LeafGround lg;
    public int leafQuantity;
    public Vector2Int leafTemper;
    [TextArea(5, 20)]
    public string effectText;
    public string leafQuantityText;
    public string temperatureText;
    public string detailText;
    public bool isUnlocked;
    public bool isOnPanel;
    public int index;
    public int orderIndex;
    public bool canModify;
    public bool isUsed; //defined in LeafGround(Update)
    private bool oneTime;
    void Start()
    {
        //isUnlocked = true;
        //transform.GetChild(0).gameObject.SetActive(false);
        //leafQuantity = 2;
        lg = leafGround.GetComponent<LeafGround>();
        temperatureText = lg.leafTemper.x + " to " + lg.leafTemper.y + " degree";
        if (leafFunc == LeafFunction.FORECASTER || leafFunc == LeafFunction.HYBRID)
        {
            leafQuantity = 2;
        }
        else
        {
            leafQuantity = 1;
        }
    }

    void Update()
    {
        if (isUnlocked && oneTime)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            oneTime = false;
            //SaveSystem.SaveLeafList(this);
        }
    }
    public void SavePlayer()
    {
        SaveSystem.SaveLeafList(this);
    }
    public void LoadLeaf()
    {
        LeafData data = SaveSystem.loadLeaf();

        isUnlocked = data.isUnlocked;
        isUsed = data.isUsed;
    }
    public void SubtractOrAddQuantity(bool subtract)
    {
        if (subtract) leafQuantity -= 1;
        else leafQuantity += 1;
    }
    public bool HowManyLeft()
    {
        if (leafQuantity > 0) return true;
        else return false;
    }
    public string Effect()
    {
        return effectText;
    }
}//CLASS
























