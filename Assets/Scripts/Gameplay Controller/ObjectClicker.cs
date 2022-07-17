using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
public class ObjectClicker : MonoBehaviour
{
    #region singleton
    public static ObjectClicker instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion 
    Transform BoardManager;
    public TypeGround typeGround;
    [SerializeField] GameObject greenLayer;
    [SerializeField] GameObject groundPanel;
    [HideInInspector] public GameObject curGround;
    Transform LeafGroundParent;
    [SerializeField]public List<GameObject> _leafGround, _leafPanel;
    private bool canAdd, canReturnLeaf, groundPanelOn, canEmpty, canAddTree, canReturnTree;
    public bool[] isFull;
    public GameObject[] slots;
    Transform mouse;
    GameObject  onHoldingTree, holdingLeafGr;
    public AudioClip placeChessClip;
    public AudioSource placeLeafSound, digSound;
    [SerializeField] Text leafEffectText, leafQuantityText, leafEffectOnPanelText, leafEffFormula;
    [SerializeField] Text detailsText1, detailsText2;
    [SerializeField] Transform newLeaf;
    public LayerMask leafMask;
    GameObject[] Ground;
    float waitForPanel = 0.4f;
    int waitPanelCounter;
    GameplayController gameplayController;
    private string curLeaf, curTree;
    private void Start()
    {
        gameplayController = GameplayController.instance;
         
        BoardManager = GameObject.Find("Board Manager").transform;
        LeafGroundParent = GameObject.Find("Leaf Ground").transform;
        mouse = GameObject.Find("Mouse").transform;
        Ground = GameObject.FindGameObjectsWithTag("Row");
        waitForPanel = 0.4f;
    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        mouse.position = mousePos2D;

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider == null)
        {
            if (groundPanelOn)
            {
                groundPanel.SetActive(false);
                groundPanelOn = false;
            }
        }
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            GetBoardPosition(hitObject);
            GetShovel(hitObject);
            if (!gameplayController.onInstruction)
            {       
                GetLeafGround(hitObject, mousePos2D);
                RemoveLeafGround(hitObject);
                GetLeafListEffect(hitObject);
                GetLeafPanel(hitObject);
                RemoveNewLeaf(hitObject);
            }
            else
            {
                GetTree(hitObject, mousePos2D);
            }
        }
        else
        {
            greenLayer.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0) && !greenLayer.activeInHierarchy)
        {
            if (holdingLeafGr != null)
            {
                if (!holdingLeafGr.GetComponent<LeafGround>().onBoard)
                {
                    holdingLeafGr.SetActive(false);
                    canAdd = false;
                    if (curLeaf != null && canReturnLeaf)
                    {
                        GameObject leaf = GameObject.Find(curLeaf).gameObject;
                        if (leaf.GetComponent<LeafList>().leafQuantity != 2)
                        {
                            leaf.GetComponent<LeafList>().SubtractOrAddQuantity(false);
                        }
                        canReturnLeaf = false;
                    }
                }
            }
            if (onHoldingTree != null)
            {
                if (!onHoldingTree.GetComponent<Tree>().onBoard)
                {
                    onHoldingTree.SetActive(false);
                    canAddTree = false;
                    if (canReturnTree)
                    {
                        GameObject tree = GameObject.Find("Tree list").gameObject;
                        tree.GetComponent<Tree>().SubtractOrAddQuantity(false);
                        canReturnTree = false;
                    }
                }
            }
        }
        GetNewLeaf();
    }// update

    #region Get green layer pos
    private void GetBoardPosition(GameObject _hitObject)
    {
        waitForPanel -= Time.deltaTime;
        print(waitForPanel);
        if (waitForPanel <= 0)
        {
            print("Reset");
            waitPanelCounter = 0;
        }
        foreach (Transform pos in BoardManager)
        {
            if (_hitObject.name == pos.name)
            {
                if (!_hitObject.GetComponent<GroundController>().hitLeaf) //If that ground didn't have any leaf on
                {
                    greenLayer.SetActive(true);
                    greenLayer.transform.position = new Vector3(pos.position.x, pos.position.y);
                }
                if (Input.GetMouseButtonDown(1))
                {
                     
                    if (!_hitObject.GetComponent<GroundController>().hitLeaf)
                    {
                        groundPanel.SetActive(true);
                        groundPanel.transform.position = new Vector3(mouse. position.x - 2.5f, mouse.position.y);
                        groundPanelOn = true;
                        curGround = _hitObject;
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    waitPanelCounter++;
                    waitForPanel = 0.4f;
                     
                     
                    if (waitPanelCounter == 2)
                    {
                        if (!_hitObject.GetComponent<GroundController>().hitLeaf)
                        {
                            groundPanel.SetActive(true);
                            groundPanel.transform.position = new Vector3(mouse.position.x - 2.5f, mouse.position.y);
                            groundPanelOn = true;
                            curGround = _hitObject;
                        }
                        waitPanelCounter = 0;
                        waitForPanel = 0.4f;
                    }
                   
                     
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameplayController.instance.tool == Tool.Shovel)
                    {
                        _hitObject.GetComponent<GroundController>().ReduceHP();
                        digSound.Play();
                        GameObject shovel = GameObject.Find("Shovel").gameObject;
                        shovel.GetComponent<Animator>().SetTrigger("Dig");
                    }
                }
            }
        }
    }//get board position
    private void GetShovel(GameObject _hitObject)
    {
        if(_hitObject.name.Contains("Shovel Button") && Input.GetMouseButtonDown(0))
        {
            print("click shovel");
            GameplayController.instance.toolClick++;
        }
    }
    private void GetGroundPanel(GameObject _hitObject)
    {
        foreach (GameObject ground in Ground) // take Row1, Row2, Row3,...
        {
            for (int i = 0; i < ground.transform.childCount; i++) // take children in Row1, Row2, Row3,...
            {
                GameObject groundChildren = ground.transform.GetChild(i).gameObject;
            }
        }
    }
    #endregion
    #region Get leaf ground
    private void GetLeafGround(GameObject _hitObject, Vector2 mousePos2D)
    {
        LeafList leafList = _hitObject.GetComponent<LeafList>();
        foreach(GameObject leafGround in _leafGround)
        {
            if(leafGround.name == _hitObject.name +" Ground")
            {
                if (leafList.isUnlocked)
                {
                    if(Input.GetMouseButtonDown(0) && leafList.leafQuantity > 0)
                    {
                        if (!leafList.isUsed)
                        {
                            leafList.SubtractOrAddQuantity(true);
                            holdingLeafGr = SmartPool.instance.SpawnFromPool
                                    (leafGround.name, new Vector3(100, 100), Quaternion.identity);
                            holdingLeafGr.GetComponent<PolygonCollider2D>().enabled = false;
                            holdingLeafGr.transform.position = mousePos2D;
                            holdingLeafGr.transform.SetParent(mouse);
                            holdingLeafGr.GetComponent<LeafGround>().readyToUse = true;

                            ChildrenSetActive(holdingLeafGr, false);
                            //set leafEffect active true, to avoid using the effect when not on board

                            canReturnLeaf = true; // can return the amount of leaves when on mouse button up without adding to ground
                            curLeaf = _hitObject.name;

                            canAdd = true;
                        }
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) && canAdd && greenLayer.activeInHierarchy)
            {
                holdingLeafGr.transform.position = greenLayer.transform.position;
                holdingLeafGr.GetComponent<PolygonCollider2D>().enabled = true;
                holdingLeafGr.GetComponent<LeafGround>().onBoard = true;
                holdingLeafGr.GetComponent<SpriteRenderer>().sortingLayerName = "Leaf";
                holdingLeafGr.GetComponent<SpriteRenderer>().sortingOrder = 0;
                holdingLeafGr.transform.SetParent(LeafGroundParent);

                placeLeafSound.Play();
                CameraShaker.Instance.ShakeOnce(.5f, 1.5f, .1f, 1f);
                ChildrenSetActive(holdingLeafGr, true);//set leafEffect active true, to use the effect when on board

                canAdd = false;
            }//add leaf ground to tiles
        }
    }// get leaf ground object

    private void GetTree(GameObject _hitObject, Vector2 mousePos2D)
    {
        if (_hitObject.name == "Tree list")
        {
            if (Input.GetMouseButtonDown(0) && _hitObject.GetComponent<Tree>().HowManyLeft())
            {
                _hitObject.GetComponent<Tree>().SubtractOrAddQuantity(true);
                onHoldingTree = SmartPool.instance.SpawnFromPool("Tree", new Vector3(100, 100), Quaternion.identity);
                onHoldingTree.transform.SetParent(mouse);
                onHoldingTree.transform.position = new Vector3(mousePos2D.x, mousePos2D.y + 0.8f);
                onHoldingTree.GetComponent<BoxCollider2D>().enabled = false;
                canAddTree = true;
                canReturnTree = true;
                ChildrenSetActive(onHoldingTree, false);
                placeLeafSound.Play();
            }
        }
        if (Input.GetMouseButtonUp(0) && canAddTree && greenLayer.activeInHierarchy)
        {
            onHoldingTree.transform.position = new Vector3(greenLayer.transform.position.x, greenLayer.transform.position.y + 1.1f);
            onHoldingTree.transform.SetParent(LeafGroundParent);
            onHoldingTree.GetComponent<BoxCollider2D>().enabled = true;
            onHoldingTree.GetComponent<SpriteRenderer>().sortingLayerName = "Leaf";
            onHoldingTree.GetComponent<SpriteRenderer>().sortingOrder = 2;
            onHoldingTree.GetComponent<Tree>().onBoard = true;
            onHoldingTree.GetComponent<Tree>().canReleaseOxygen = true;

            CameraShaker.Instance.ShakeOnce(.5f, 1.5f, .1f, 1f);
            placeLeafSound.Play();
            ChildrenSetActive(onHoldingTree, true);
            canAddTree = false;
        }
    }
    #endregion
    #region Remove leaf ground
    private void RemoveLeafGround(GameObject _hitObject)
    {
        for (int i = 0; i < _leafGround.Count; i++)
        {
            if (_leafGround[i].name + "(Clone)" == _hitObject.name && Input.GetMouseButtonDown(1))
            {
                if (!_hitObject.GetComponent<LeafGround>().onGerminating)
                {
                    _hitObject.GetComponent<PolygonCollider2D>().enabled = false;
                    _hitObject.GetComponent<LeafGround>().onBoard = false;
                    _hitObject.SetActive(false);
                }
            }
        }
        if (gameplayController.onInstruction)
        {
            if (_hitObject.name == "Tree(Clone)" && Input.GetMouseButtonDown(1))
            {
                _hitObject.SetActive(false);
                _hitObject.GetComponent<BoxCollider2D>().enabled = false;
                GameObject treeList = GameObject.Find("Tree list").gameObject;
                treeList.GetComponent<Tree>().SubtractOrAddQuantity(false);
            }
        }
    }
    #endregion
    #region Get leaf list effect
    private void GetLeafListEffect(GameObject _hitObject)
    {
        LeafGround LeafGround = _hitObject.GetComponent<LeafGround>();
        LeafList LeafList = _hitObject.GetComponent<LeafList>();
        GroundController Ground = _hitObject.GetComponent<GroundController>();
        Tree Tree = _hitObject.GetComponent<Tree>();

        foreach(GameObject leafGround in _leafGround)
        {
            if (leafGround.name == _hitObject.name + " Ground")
            {
                if (LeafList.isUnlocked)
                {
                    leafEffectText.text = LeafList.Effect();
                    if (LeafList.leafQuantity == 2)
                        leafQuantityText.text = "Ready to use";
                    else if (LeafList.isUsed) leafQuantityText.text = "Used";
                    else leafQuantityText.text = "Not ready to use";
                }
                else
                {
                    leafEffectText.text = "Unknown: Try using gene modify tab";
                    leafQuantityText.text = "Not ready to use";
                }
            }
        }
        foreach(GameObject leafPanel in _leafPanel)
        {
            if (leafPanel.name == _hitObject.name)
            {
                if (LeafList.isUnlocked)
                {
                    leafEffectOnPanelText.text = LeafList.Effect();
                }
                else
                {
                    leafEffectOnPanelText.text = "Unknown: Try using gene modify tab";
                }
            }
        }
        if(_hitObject.CompareTag("Leaf List"))
        {
            leafEffFormula.text = LeafList.Effect();
            detailsText1.text = LeafList.temperatureText;
            detailsText2.text = LeafList.detailText;
        }
        if (_hitObject.name == "Tree list")
        {
            leafEffectText.text = "Release Oxygen";
            leafQuantityText.text = "Amount: " + _hitObject.GetComponent<Tree>().treeQuantity;
        }
        if (_hitObject.CompareTag("Leaf"))
        {
            detailsText1.text = LeafGround.temperatureText;
            detailsText2.text = LeafGround.detailText;
            if (LeafGround.currently_Stand_On == CurrentlyStandOn.Brown)
            {
                leafEffectText.text = LeafGround.effectText;
                leafQuantityText.text = "Duration: " + LeafGround.effectDuration;
            }
            else if(LeafGround.currently_Stand_On == CurrentlyStandOn.Blue || 
                LeafGround.currently_Stand_On == CurrentlyStandOn.Purple)
            {
                leafEffectText.text = LeafGround.effectText;
                leafQuantityText.text = "Time left: " + LeafGround.germination_Time;
            }
        }
        if (_hitObject.CompareTag("Tree"))
        {
            leafEffectText.text = "Chance of survival: " + Tree.hp + "%" + "\n"+
                                  "Fire : " + Tree.onFireThreshold + "\n"
                                 +"Drought : "+ Tree.droughtThreshold + "\n" 
                                 +"Overwatered : "+ Tree.overwateredThreshold;
            leafQuantityText.text = "Oxygen release: " + Tree.opm * Tree.leafRate * Tree.groundRate;
            detailsText1.text = "Fire: " + Tree.fireResis + "\n"
                              + "Radiation: " + Tree.radResis + "\n"
                              + "Freeze: " + Tree.freezeResis + "\n"
                              + "Drought: " + Tree.droughtResis + "\n"
                              + "Gas: " + Tree.gasResis + "\n"
                              + "Water: " + Tree.waterResis + "\n";

            detailsText2.text = "Insect: " + Tree.insectResis + "\n"
                              + "Hurricane: " + Tree.hurricaneResis + "\n"
                              + "Sandstorm: " + Tree.sandstormResis + "\n"
                              + "Soil Rate: " + Tree.groundRate + "\n"
                              + "Leaf Rate: " + Tree.leafRate + "\n";
        }
        if (_hitObject.CompareTag("Ground"))
        {
            leafEffectText.text = "drought threshold: " + Ground.degrading_Time + "\n" +
                                  "moist threshold: " + Ground.humidThreshold;
            leafQuantityText.text = "";
        }
    }
    #endregion
    public void GroundChange(Sprite groundColor, TypeGround type_Ground)
    {
        curGround.GetComponent<SpriteRenderer>().sprite = groundColor;
        curGround.GetComponent<GroundController>().typeGround = type_Ground;
    }
    public void GetLeafPanel(GameObject _hitObject)
    {
        foreach(GameObject nleafPanel in _leafPanel)
        {
            if (nleafPanel.name == _hitObject.name)
            {
                for (int j = 0; j < slots.Length; j++)
                {
                    if (!isFull[j])
                    {
                        if (Input.GetMouseButtonDown(0) && nleafPanel.GetComponent<LeafList>().canModify)
                        {
                            GameObject leafPanel = SmartPool.instance.SpawnFromPool
                                 (_hitObject.name, new Vector2(100, 100), Quaternion.identity);
                            leafPanel.transform.position = slots[j].transform.position;

                            GameObject fuseParent = GameObject.Find("Holder");
                            leafPanel.transform.SetParent(fuseParent.transform);
                            isFull[j] = true;
                            break;
                        }
                    }
                    if (canEmpty) // when clicked modify, all slots must be empty to add more leaf
                    {
                        foreach(GameObject leafPan in _leafPanel)
                        {
                            leafPan.GetComponent<LeafList>().canModify = false;
                        }
                        isFull[j] = false;
                        if (j == slots.Length - 1)
                        {
                            canEmpty = false;
                             
                        }
                    }
                }
            }
        }
    }
    public void GetNewLeaf()
    {
        if (GameplayController.instance.isPausing)
        {
            if (Modify.instance.isModified)
            {
                print("smoethingnice");
                switch (Modify.instance.newLeaf)
                {
                    case 6:
                        SpawnNewLeaf(3);
                        break;
                    case 11:
                        SpawnNewLeaf(8);
                        break;
                    case 13:
                        SpawnNewLeaf(10);
                        break;
                    case 27:
                        SpawnNewLeaf(9);
                        break;
                    case 25:
                        SpawnNewLeaf(15);
                        break;
                    case 26:
                        SpawnNewLeaf(16);
                        break;
                    case 21:
                        SpawnNewLeaf(17);
                        break;
                    case 23:
                        SpawnNewLeaf(20);
                        break;
                    case 51:
                        SpawnNewLeaf(24);
                        break;
                    case 30:
                        SpawnNewLeaf(27);
                        break;
                    case 36:
                        SpawnNewLeaf(33);
                        break;
                    case 68:
                        SpawnNewLeaf(36);
                        break;
                    case 53:
                        SpawnNewLeaf(37);
                        break;
                    case 45:
                        SpawnNewLeaf(38);
                        break;
                    case 93:
                        SpawnNewLeaf(39);
                        break;
                    case 127:
                        SpawnNewLeaf(40);
                        break;
                    case 79:
                        SpawnNewLeaf(41);
                        break;
                    case 81:
                        SpawnNewLeaf(42);
                        break;
                    case 74:
                        SpawnNewLeaf(44);
                        break;
                    case 89:
                        SpawnNewLeaf(45);
                        break;
                    case 37:
                        SpawnNewLeaf(47);
                        break;
                    case 211:
                        SpawnNewLeaf(48);
                        break;
                }
                #region randomly modify
                //if (newLeafIndex >= 1 && newLeafIndex <= 10) SpawnNewLeaf(leaf, leafPanel);
                //if (10 < newLeafIndex && newLeafIndex <= 20)
                //{
                //    if (ranIndex == 0 || ranIndex == 1 || ranIndex == 2
                //        || ranIndex == 3 || ranIndex == 4 || ranIndex == 5 || ranIndex == 6 || ranIndex == 7)
                //    {
                //        SpawnNewLeaf(leaf, leafPanel);
                //    }
                //    else if (ranIndex == 8) SpawnLeaf1();
                //    else if (ranIndex == 9) SpawnLeaf2();
                //}
                //if (20 < newLeafIndex && newLeafIndex <= 30)
                //{
                //    if (ranIndex == 0 || ranIndex == 1 || ranIndex == 2 || ranIndex == 3
                //        || ranIndex == 4 || ranIndex == 5 || ranIndex == 6)
                //    {
                //        SpawnNewLeaf(leaf, leafPanel);
                //    }
                //    else if (ranIndex == 8) SpawnLeaf1();
                //    else if (ranIndex == 9) SpawnLeaf2();
                //}
                //if (30 < newLeafIndex && newLeafIndex <= 40)
                //{
                //    if (ranIndex == 0 || ranIndex == 1 || ranIndex == 2 || ranIndex == 3 || ranIndex == 4 || ranIndex == 5)
                //    {
                //        SpawnNewLeaf(leaf, leafPanel);
                //    }
                //    else if (ranIndex == 8) SpawnLeaf1();
                //    else if (ranIndex == 9) SpawnLeaf2();
                //}
                //if (40 < newLeafIndex && newLeafIndex <= 43)
                //{
                //    if (ranIndex == 0 || ranIndex == 1 || ranIndex == 2)
                //    {
                //        SpawnNewLeaf(leaf, leafPanel);
                //    }
                //    else if (ranIndex == 8) SpawnLeaf1();
                //    else if (ranIndex == 9) SpawnLeaf2();
                //}
                //if (43 < newLeafIndex && newLeafIndex <= 48)
                //{
                //    if (ranIndex == 0 || ranIndex == 1)
                //    {
                //        SpawnNewLeaf(leaf, leafPanel);
                //    }
                //    else if (ranIndex == 8) SpawnLeaf1();
                //    else if (ranIndex == 9) SpawnLeaf2();
                //}
                #endregion
                Modify.instance.isModified = false;
                canEmpty = true;
            }
        }
    }
    #region DON'T REPEAT YOURSELF FUNCTIONS
    private void SpawnNewLeaf(int index)
    {
        
        GameObject leafSlot5 = SmartPool.instance.SpawnFromPool
                        ("Leaf " + index + " Panel", newLeaf.position, Quaternion.identity);
        GameObject leaf = GameObject.Find("Leaf " + index).gameObject;
        GameObject leafPanel = GameObject.Find("Leaf " + index + " Panel").gameObject;

        leafSlot5.transform.SetParent(newLeaf);
        leafSlot5.GetComponent<SpriteRenderer>().sortingOrder = 0;

        leaf.GetComponent<LeafList>().isUnlocked = true;
        leaf.transform.GetChild(0).gameObject.SetActive(false);
        leaf.GetComponent<LeafList>().leafQuantity++;

        leafPanel.GetComponent<LeafList>().isUnlocked = true;
        leafPanel.transform.GetChild(0).gameObject.SetActive(false);
        leafPanel.GetComponent<LeafList>().leafQuantity++;

        gameplayController.Leaf_Counter++;
    }
    private void SpawnLeaf1()
    {
        GameObject leafSlot5 = SmartPool.instance.SpawnFromPool
               ("Leaf 1 Panel", newLeaf.position, Quaternion.identity);
        leafSlot5.transform.SetParent(newLeaf);
        GameObject leaf1 = GameObject.Find("Leaf 1").gameObject;
        leaf1.GetComponent<LeafList>().leafQuantity++;
    }
    private void SpawnLeaf2()
    {
        GameObject leafSlot5 = SmartPool.instance.SpawnFromPool
               ("Leaf 2 Panel", newLeaf.position, Quaternion.identity);
        leafSlot5.transform.SetParent(newLeaf);
        GameObject leaf2 = GameObject.Find("Leaf 2").gameObject;
        leaf2.GetComponent<LeafList>().leafQuantity++;
    }
    private void RemoveNewLeaf(GameObject _hitObject) // when clicked to new Leaf on Panel, it will disappear
    {
        if (_hitObject.name == "Slot 5")
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChildrenSetActive(_hitObject, false);
            }
        }
    }
    private void ChildrenSetActive(GameObject parent, bool activate)
    {
        for (int j = 0; j < parent.transform.childCount; j++)
        {
            GameObject child = parent.transform.GetChild(j).gameObject;
            child.SetActive(activate);
            //set leafEffect active true, to use the effect when on board
        }
    }
    #endregion


}//CLASS

























