using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Tool
{
    Mouse,
    Shovel
}
public class GameplayController : MonoBehaviour
{
    #region singleton
    public static GameplayController instance;
    void MakeInstance()
    {
        if (instance == null) instance = this;
    }
    private void Awake()
    {
        MakeInstance();
    }
    #endregion
    [Header("Game Speed")]
    public int clicker;
    public Text speedText;
    public bool isPausing;
    public int timeMultiple; // change speed if player choose 
    [Header("Shovel")]
    public Tool tool;
    public int toolClick;
    [SerializeField] GameObject shovel;
    public Sprite dayBG;
    public Sprite nightBG;
    [Header("Instruction")]
    public bool onInstruction;
    public Text instructionCountdownText;
    public Text instructionText;
    private int instructionCountdown;
    public GameObject Details1, Details2;

    GameObject[] trees;
    [SerializeField] GameObject geneModifyTab;
    [SerializeField] GameObject FormulaPanel;
    [SerializeField] GameObject[] Ground;
    [SerializeField] Image fillOxygen;
    [SerializeField] List<GameObject> leafList = new List<GameObject>();

    [SerializeField] public bool oneTime; // start climate in update function once only
    public float reduceO2Rate;
    [Header("Climate")]
    public bool can_Start_Climating; 
    public bool can_Damage_Trees, can_Freeze_Trees;
    public int Leaf_Counter = 2; // To increase game difficulty
    private float i,j;
    public Image leaf;
    public Sprite brownGround, blueGround, greenGround, purpleGround, degradedGround, bluedroughtGround, purpledroughtGround;
    public Text germinateWarningText;
     

    public List<string> fire = new List<string>();
    public List<string> snow = new List<string>();
    public List<string> gas  = new List<string>();
    public List<string> rad  = new List<string>();
    public List<string> bio = new List<string>();
    public List<string> hurr = new List<string>();
    public List<string> drought = new List<string>();
    public List<string> sand = new List<string>();
    public List<string> rare = new List<string>(); 
    public List<string> forecaster = new List<string>();
    public List<string> leafformula = new List<string>();
    void Start()
    {
        timeMultiple = 3;
        clicker = 1;
        instructionCountdown = 10;
        Leaf_Counter = 2;
        Ground = GameObject.FindGameObjectsWithTag("Row");
        onInstruction = true;
        Details1.SetActive(false);
        Details2.SetActive(false);
        i = j = 1;   
    }
    public void ToFillOxygen(float amount)
    {
        fillOxygen.fillAmount += amount / 100f;
    }
    void Update()
    {
        if (toolClick == 1)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            shovel.SetActive(true);
            shovel.transform.position = mousePos2D;
            tool = Tool.Shovel;
        }
        else
        {
            toolClick = 0;
            shovel.SetActive(false);
            tool = Tool.Mouse;
        }
        Time.timeScale = 3f;
        trees = GameObject.FindGameObjectsWithTag("Tree");
        PlantYourTreeInLimitedTime();
        if (onInstruction == false)
        {
            //Time.timeScale = 2f;
            Details1.SetActive(true);
            Details2.SetActive(true);
            instructionCountdownText.gameObject.SetActive(false);
            instructionText.gameObject.SetActive(false);
            if (trees.Length == 0)
            {
                ResetGameplay();
            }
        }
        GameSpeedUpdate();
        Reduce_Oxygen();
        // disable collider when open modify tab
        foreach (GameObject ground in Ground) // take Row1, Row2, Row3,...
        {
            for(int i = 0; i < ground.transform.childCount; i++) // take children in Row1, Row2, Row3,...
            {
                GameObject groundChildren = ground.transform.GetChild(i).gameObject;
                if (isPausing)
                {
                    groundChildren.GetComponent<PolygonCollider2D>().enabled = false;
                }
                if (!isPausing)
                {
                    //if (!groundChildren.GetComponent<GroundController>().hitLeaf)
                    groundChildren.GetComponent<PolygonCollider2D>().enabled = true;
                }
            }
        }
    }
 
    public void ResetGameplay()
    {
        instructionCountdown = 10;
        Details1.SetActive(false);
        Details2.SetActive(false);
        instructionCountdownText.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(true);
        DayNight.instance.ResetDayAndNight();
        Climate.instance.ResetClimate();
        onInstruction = true;

        GameObject forecaster = GameObject.Find("Leaf 2").gameObject;
        forecaster.GetComponent<LeafList>().leafQuantity = 2;

        GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
        foreach(GameObject gr in ground)
        {
            GroundController g = gr.GetComponent<GroundController>();
            g.hitLeaf = false;
            g.spriteRenderer.sprite = brownGround;
            g.typeGround = TypeGround.Brown; g.state = State.Normal;
            g.degrading_Time = 120; g.humidThreshold = 150;
        }

        GameObject[] leafList = GameObject.FindGameObjectsWithTag("Leaf List");
        foreach(GameObject leaf in leafList)
        {
            LeafList l = leaf.GetComponent<LeafList>();
            if (l.leafFunc != LeafFunction.HYBRID) 
            { 
                l.leafQuantity = 1;
                l.isUsed = false;
            }
        }
    }
    #region Unlock Leaf
    public void UnlockLeaf(List<string> group)
    {
        int i = Random.Range(1, group.Count);
        if (group.Count > 1)
        {
            GameObject leaf = GameObject.Find(group[0]).gameObject;
            leaf.GetComponent<LeafList>().isUnlocked = true;
            leaf.GetComponent<LeafList>().leafQuantity = 2;
            leaf.transform.GetChild(0).gameObject.SetActive(false);
            GameObject[] hiddenObject = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject leafPanel in hiddenObject)
            {
                if (leafPanel.name == group[0] + " Panel")
                {
                    leafPanel.GetComponent<LeafList>().leafQuantity = 2;
                    leafPanel.GetComponent<LeafList>().isUnlocked = true;
                    leafPanel.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            group.Remove(group[0]);
        }
    }
    public void UnlockFormula(List<string> group)
    {
        if (group.Count > 1)
        {
            int i = Random.Range(1, group.Count);
            GameObject[] hiddenObject = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject leafPanel in hiddenObject)
            {
                if (leafPanel.name == group[i] + " Formula")
                {
                    leafPanel.GetComponent<LeafList>().leafQuantity = 2;
                    leafPanel.GetComponent<LeafList>().isUnlocked = true;
                    leafPanel.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            group.Remove(group[0]);
        }
    }
    #endregion
    private void PlantYourTreeInLimitedTime()
    {
        j -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            instructionCountdown -= 1;
            instructionCountdownText.text = instructionCountdown.ToString();
        }
        if (j <= 0)
        {
            instructionCountdown -= 1;
            instructionCountdownText.text = instructionCountdown.ToString();
            j = 1;
        }
        if(instructionCountdown <= 0)
        {
            onInstruction = false;
        }
    }
    public void Reduce_Oxygen()
    {
        i -= Time.deltaTime;
        if (i <= 0)
        {
            ToFillOxygen(-1 * reduceO2Rate); i = 1;
        }
    }
    public void OpenFormulaPanel()
    {
        FormulaPanel.SetActive(true);
        isPausing = true;
    }
    public void CloseFormulaPanel()
    {
        FormulaPanel.SetActive(false);
        isPausing = false;
    }
    public void OpenGeneModifyTab()
    {
        geneModifyTab.SetActive(true);
        foreach (GameObject leafList in leafList)
        {
            LeafList llist = leafList.GetComponent<LeafList>();
            foreach(GameObject leafPan in ObjectClicker.instance._leafPanel)
            if (leafPan.name == leafList.name + " Panel" && llist.leafQuantity == 2 && llist.where == Where.MAIN)
            {
                    leafPan.GetComponent<LeafList>().canModify = true;
                    leafPan.GetComponent<LeafList>().effectText = leafList.GetComponent<LeafList>().effectText;
            }
        }
        isPausing = true;
    }
    public void GameSpeedUpdate()
    {
        if (clicker == 1)
        {
            speedText.text = "x1";
            Time.timeScale = 1f;
            timeMultiple = 1;
        }
        else if(clicker == 2)
        {
            speedText.text = "x2";
            Time.timeScale = 2f;
            timeMultiple = 2;
        }
        else if (clicker == 3)
        {
            speedText.text = "x3";
            Time.timeScale = 3f;
            timeMultiple = 3;
        }
        if (clicker > 3)
        {
            clicker = 1; 
        }
    }
    public void GameSpeedButton()
    {
        clicker++;
    }

    public void CloseGeneModifyTab()
    {
        geneModifyTab.SetActive(false);
        isPausing = false;
    }
}//CLASS



























