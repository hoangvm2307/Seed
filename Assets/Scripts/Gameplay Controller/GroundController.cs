using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeGround
{
    Blue,
    Brown,
    Green,
    Purple
}
public enum State
{
    Drought,
    Normal,
    Moist
}
public class GroundController : MonoBehaviour //Normalize Tree Status Here
{
    public bool hitLeaf; // ObjectClicker and GameplayController
    public TypeGround typeGround;
    public float degrading_Time;
    public float degradeRate;
    public State state;
    public float humidThreshold;
    private float i, j, k, l;
    public SpriteRenderer spriteRenderer;
    public int groundTemper;
    public int hp;
     
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        humidThreshold = 20;
        typeGround = TypeGround.Brown;
        i = 1;
        hp = 10;
        degrading_Time = 100;
        state = State.Normal;
    }
    private void Update()
    {
        if (degrading_Time <= 0)
        {
            degrading_Time = 0;
            transform.GetComponent<SpriteRenderer>().sprite = degradedGround;
            state = State.Drought;
        }
        RegenerateGround();
        groundTemper = Temperature.instance.temperature;
        GameObject[] LeafList = GameObject.FindGameObjectsWithTag("Leaf List");
        ReduceHumidThreshold();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            Tree Tree = hitObject.GetComponent<Tree>();
            LeafGround LeafGround = hitObject.GetComponent<LeafGround>();
             
            if (hitObject.CompareTag("Leaf"))
            {
                if(typeGround != TypeGround.Blue)
                DegradingGround(1);
            }
            if (hitObject.CompareTag("Tree"))
            {
                if (state == State.Drought)
                {
                    Tree.groundRate = 0.5f;
                    Tree.hp -= Time.deltaTime;
                }
                else if (state == State.Normal)
                {
                    Tree.groundRate = 1f;
                }
                else if (state == State.Moist)
                {
                    Tree.groundRate = 0.5f;
                    Tree.hp -= Time.deltaTime;
                }
            }
            if (hitObject.CompareTag("Leaf"))
            {
                hitLeaf = true; // In ObjectClicker
                if (typeGround == TypeGround.Blue)
                {
                    if(LeafGround.leafQuantity == 0)
                    transform.GetComponent<SpriteRenderer>().sprite = bluedroughtGround;
                    else
                    {
                        foreach (GameObject leaflist in LeafList)
                        {
                            if (LeafGround.name == leaflist.name + " Ground(Clone)")
                            {
                                leaflist.GetComponent<LeafList>().leafQuantity = 1;
                            }
                        }
                        LeafGround.gameObject.SetActive(false);
                        hitLeaf = false;
                    }
                }
                if(typeGround == TypeGround.Brown)
                {
                    if(LeafGround.leafQuantity == 1)
                        spriteRenderer.sprite = degradedGround;
                    else
                    {
                        foreach(GameObject leaflist in LeafList)
                        { 
                            if (LeafGround.name == leaflist.name + " Ground(Clone)")
                            {
                                leaflist.GetComponent<LeafList>().leafQuantity = 1; // To refill the leaf not ready        
                            }
                        }
                        LeafGround.gameObject.SetActive(false);
                        GameplayController.instance.germinateWarningText.text = "Use the blue soil to germinate your leaves";
                        hitLeaf = false;
                    }
                }
                if(typeGround == TypeGround.Purple)
                {
                    spriteRenderer.sprite = purpledroughtGround;
                }
            }
            else
            {
                //hitLeaf = false;
            }         
        }
    }
    public Sprite blueGround, brownGround, greenGround, purpleGround, degradedGround, humidGround, bluedroughtGround, purpledroughtGround;

    public void DegradingGround(float degradeTime)
    {
        i -= Time.deltaTime;
        if (i <= 0)
        {
            i = 1;
            degrading_Time -= (degradeTime * degradeRate);
        }
        if (degrading_Time <= 0)
        {
            degrading_Time = 0;
            transform.GetComponent<SpriteRenderer>().sprite = degradedGround;
            state = State.Drought;
        }
    }
    public void ReduceHP()
    {
        hp -= 1;
        if (hp == 0)
        {
            spriteRenderer.sprite = degradedGround;
            hp = -1;
            GameplayController.instance.UnlockFormula(GameplayController.instance.leafformula);
        }      
    }
    public void RegenerateGround()
    {
        l -= Time.deltaTime;
        if(l <= 0)
        {
            if(degrading_Time < 120)
            degrading_Time += 1;
            l = 4;
        }
        if(degrading_Time >= 120)
        {

        }
    }
    public void ReduceHumidThreshold()
    {
        if (Climate.instance.canHumidify)
        {
            if (state == State.Drought)
            {
                transform.GetComponent<SpriteRenderer>().sprite = brownGround;
                state = State.Normal;
            }
            print("humidify");
            j -= Time.deltaTime;
            if (j <= 0)
            {
                humidThreshold -= 3;
                j = 1;
            }
        }
        if (humidThreshold <= 0)
        {
            humidThreshold = 0;
            k -= Time.deltaTime;
            if (k <= 0)
            {
                state = State.Moist;
                spriteRenderer.sprite = humidGround; 
                k = 1;
            }
        }
    }
    public void ChangeToBlue()
    {
        ObjectClicker.instance.GroundChange(blueGround,TypeGround.Blue);
    }
    public void ChangeToBrown()
    {
        ObjectClicker.instance.GroundChange(brownGround, TypeGround.Brown);
    }
    public void ChangeToGreen()
    {
        ObjectClicker.instance.GroundChange(greenGround, TypeGround.Green);
    }
    public void ChangeToPurple()
    {
        ObjectClicker.instance.GroundChange(purpleGround, TypeGround.Purple);
    }
    public void BecomeDrought()
    {

    }
}//CLASS

















