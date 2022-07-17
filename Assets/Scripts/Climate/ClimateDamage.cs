using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateDamage : MonoBehaviour
{
    private Climate climate;
    [SerializeField]private TypeDamage typeDamage;
    GameObject[] trees;
    [SerializeField] private float damage;
    private ParticleSystem FX;
    void Start()
    {
         
        climate = transform.parent.parent.GetComponent<Climate>();
         
        FX = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        trees = GameObject.FindGameObjectsWithTag("Tree");
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameplayController.instance.can_Damage_Trees = true;
        }
        if (FX.isPlaying)
        {
            if (GameplayController.instance.can_Damage_Trees) // trigger true in ObjectClicker
            {
                DealDamageToTree();
            }
            if (GameplayController.instance.can_Freeze_Trees)
            {
                print("Freeze Trees");
                FreezeTree();
            }
        }
    }
    public void DealDamageToTree()
    {
        foreach(GameObject tree in trees)
        {
            Tree t = tree.GetComponent<Tree>();
            #region get all resistance of tree
            float fireResis = t.fireResis;
            float radResis = t.radResis;
            float freezeResis = t.freezeResis;
            float drought = t.droughtResis;
            float gasResis = t.gasResis;
            float hurricaneResis = t.hurricaneResis;
            float sandstormResis = t.sandstormResis;
            float biohazardResis = t.biohazardResis;
            #endregion
            if (tree.activeInHierarchy)
            {
                switch (typeDamage)
                {
                    case TypeDamage.Sand:
                        t.ReduceHealth(climate.damage, sandstormResis);
                        t.sandstormAf = true;
                        break;
                    case TypeDamage.Fire:
                        t.ReduceHealth(climate.damage, fireResis);
                        t.fireAf = true;
                        break;
                    case TypeDamage.Radiation:
                        t.ReduceHealth(climate.damage, radResis);
                        t.radAf = true;
                        break;
                    case TypeDamage.Freeze:
                        t.ReduceHealth(climate.damage, freezeResis);
                        t.freezeAf = true;
                        break;
                    case TypeDamage.Drought:
                        t.ReduceHealth(climate.damage, drought);
                        t.droughtAf = true;
                        break;
                    case TypeDamage.Gas:
                        t.ReduceHealth(climate.damage, gasResis);
                        t.gasAf = true;
                        break;
                    case TypeDamage.Hurricane:
                        t.ReduceHealth(climate.damage, hurricaneResis);
                        t.hurricaneAf = true;
                        break;
                    case TypeDamage.Biohazard:
                        t.ReduceHealth(climate.damage, biohazardResis);
                        t.bioAf = true;
                        break;
                }
                #region old typeDamage
                //if (typeDamage == TypeDamage.Sand)
                //{
                //    t.ReduceHealth(climate.damage, sandstormResis);
                //    t.sandstormAf = true;
                //}
                //if (typeDamage == TypeDamage.Fire)
                //{
                //    t.ReduceHealth(climate.damage, fireResis);
                //    t.fireAf = true;
                //}
                //if (typeDamage == TypeDamage.Radiation)
                //{
                //    t.ReduceHealth(climate.damage, radResis);
                //    t.radAf = true;
                //}
                //if (typeDamage == TypeDamage.Freeze)
                //{
                //    t.ReduceHealth(climate.damage, freezeResis);
                //    t.freezeAf = true;
                //}
                //if (typeDamage == TypeDamage.Drought)
                //{
                //    t.ReduceHealth(climate.damage, drought);
                //    t.droughtAf = true;
                //}
                //if (typeDamage == TypeDamage.Gas)
                //{
                //    t.ReduceHealth(climate.damage, gasResis);
                //    t.gasAf = true;
                //}
                //if (typeDamage == TypeDamage.Hurricane)
                //{
                //    t.ReduceHealth(climate.damage, hurricaneResis);
                //    t.hurricaneAf = true;
                //}
                //if (typeDamage == TypeDamage.Biohazard)
                //{
                //    t.ReduceHealth(climate.damage, biohazardResis);
                //    t.bioAf = true;
                //}
                #endregion
            }
        }

    }
    public void FreezeTree()
    {
        foreach (GameObject tree in trees)
        {
            Tree t = tree.GetComponent<Tree>();
            float freezeResis = t.freezeResis;
            if (tree.activeInHierarchy)
            {
                t.ReduceHealth(climate.damage, freezeResis);
                t.freezeAf = true;
            }
        }

    }
}//CLASS











