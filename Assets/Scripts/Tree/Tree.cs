using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    Overwatered,
    OnFire,
    Drought,
    Normal
}
public class Tree : MonoBehaviour
{
    public bool onBoard;
    public Sprite treeRootSprite;
    public Sprite treeSprite;
    public SpriteRenderer sr;
    private GameplayController gcontr;
    public int treeQuantity;
    public string effectText;
    public bool isUnlocked;
    public bool isOnPanel, isAffected, canReleaseOxygen;
    public bool isOnFire;
    public int index;
    public float opm, hp, fireResis, radResis,freezeResis, droughtResis, 
        lowTempResis, highTempResis, gasResis, waterResis,insectResis, 
        hurricaneResis, sandstormResis,biohazardResis,germinationTime;
    public float groundRate;
    public float leafRate;
    [Header("Affected")]
    public bool fireAf;
    public bool radAf;
    public bool freezeAf;
    public bool droughtAf;
    public bool gasAf;
    public bool bioAf;
    public bool allAf;
    public bool hurricaneAf;
    public bool sandstormAf;
    public bool isTreeUI;
    public bool touchedLeaf,oneTime;
    [Header("Status Threshold")]
    [SerializeField] public float overwateredThreshold;
    [SerializeField] public float droughtThreshold;
    [SerializeField] public float onFireThreshold;
    public float chanceOfSurvival;
    private float i,j,k,l,m1,m2,n1,n2;
    [Header("Status")]
    public Status status;
    void Start()
    {
        status = Status.Normal;
        i = j = k = l = m1 = m2 = n1 = n2 = 1;
        onFireThreshold = 50;
        overwateredThreshold = 50;
        droughtThreshold = 50;
        chanceOfSurvival = 0;
        oneTime = true;
        gcontr = GameplayController.instance;
        sr = GetComponent<SpriteRenderer>();

        if (!isTreeUI)
        {
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - 0.8f), Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            LeafGround Leaf = hitObject.GetComponent<LeafGround>();
            if (hitObject.CompareTag("Leaf"))
            {
                if(Leaf.leafType == LeafType.Leaf42)
                {
                    print("Leaf 42");
                    hp = 100;
                    hitObject.SetActive(false);
                }
            }
        }
        Release_Oxygen();

        if (hp <= 0)
        {
            hp = 0;
            if(status == Status.OnFire)
            {
                //change sprite => burnt tree
            }
            if(status == Status.Drought)
            {
                //change sprite => drought tree
            }
            if(status == Status.Overwatered)
            {
                //change sprite => drought tree
            }
        } //dead tree
    }
    public void DayEndSpawnNewLeaf()
    {
        int rand = Random.Range(0, 85);
        GameObject Hybrid = GameObject.Find("Leaf 2").gameObject;
        Hybrid.GetComponent<LeafList>().isUsed = false;
        if (rand <= hp)
        {
            Hybrid.GetComponent<LeafList>().leafQuantity = 2;

            hp = 100;
            if (gasAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.gas); gasAf = false;
            }
            if (fireAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.fire); fireAf = false;
            }
            if (radAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.rad); radAf = false;
            }
            if (bioAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.bio); bioAf = false;
            }
            if (hurricaneAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.hurr); hurricaneAf = false;
            }
            if (sandstormAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.sand); sandstormAf = false;
            }
            if (allAf)
            {
                GameplayController.instance.UnlockLeaf(gcontr.rare); allAf = false;
            }

        }
        else
        {
            sr.sprite = treeRootSprite;
            //change sprite 
            transform.tag = "Dead Tree";
            //change tag
        }

    }
    private void Release_Oxygen()
    {
        l -= Time.deltaTime;
        if(l <= 0)
        {
            if (DayNight.instance.session == Session.Day)
                GameplayController.instance.ToFillOxygen(opm * leafRate * groundRate);
            if (DayNight.instance.session == Session.Night)
                GameplayController.instance.ToFillOxygen(-opm * leafRate * groundRate);
            l = 1;
        }        
    }
    public void SubtractOrAddQuantity(bool subtract)
    {
        if (subtract) treeQuantity -= 1;
        else treeQuantity += 1;
    }
    public bool HowManyLeft()
    {
        if (treeQuantity > 0) return true;
        else return false;
    }
    public string Effect()
    {
        return effectText;
    }
    public void ReduceHealth(float _damage, float _resistance)
    {
        i -= Time.deltaTime;
        if(i <= 0)
        {
            print("reduce health");
            if(_damage > 0)
            hp -= (_damage - _resistance - chanceOfSurvival);
            i = 1;
        }
        if(hp <= 0)
        {
            sr.sprite = treeRootSprite;
            //change sprite 
            transform.tag = "Dead Tree";
            //change tag
        }       
    }
    public void ReduceOnfireThreshold()
    {
        if (Climate.instance.isFire)
        {
            j -= Time.deltaTime;
            if (j <= 0)
            {
                onFireThreshold -= 5 / fireResis;
                //change sprite => hot tree
                j = 1;
            }         
        }
        if (onFireThreshold <= 0)
        {
            status = Status.OnFire;
            //change sprite => onfire animation
            onFireThreshold = 0;
            k -= Time.deltaTime;
            if (k <= 0)
            {
                ReduceHealth(1,0);
                k = 1;
            }
        }
    }
    public void ReduceOverwateredThreshold()
    {
        if (Climate.instance.isRain)
        {
            m1 -= Time.deltaTime;
            if (m1 <= 0)
            {
                overwateredThreshold -= 5;
                //change sprite => overwatered tree
                m1 = 1;
            }
        }
        if (overwateredThreshold <= 0)
        {
            status = Status.Overwatered;
            //change sprite => onfire animation
            overwateredThreshold = 0;
            m2 -= Time.deltaTime;
            if (m2 <= 0)
            {
                ReduceHealth(1,0);
                m2 = 1;
            }
        }
    }
    public void ReduceDroughtThreshold()
    {
        if (Climate.instance.weatherState == WeatherState.Hot)
        {
            n1 -= Time.deltaTime;
            if (n1 <= 0)
            {
                droughtThreshold -= 5 / highTempResis;
                //change sprite => drought tree
                n1 = 1;
            }
        }
        if (Climate.instance.weatherState == WeatherState.Fire)
        {
            n1 -= Time.deltaTime;
            if (n1 <= 0)
            {
                droughtThreshold -= 10 / highTempResis;
                //change sprite => drought tree
                n1 = 1;
            }
        }
        if (droughtThreshold <= 0)
        {
            status = Status.Drought;
            //change sprite => drought tree
            droughtThreshold = 0;
            n2 -= Time.deltaTime ;
            if (n2 <= 0)
            {
                hp -= 2;
                n2 = 1;
            }
        }
    }
}//CLASS
































