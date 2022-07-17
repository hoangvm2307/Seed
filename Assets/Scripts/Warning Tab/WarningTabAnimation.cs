using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningTabAnimation : MonoBehaviour
{
    #region singleton
    public static WarningTabAnimation instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public Image colorPanel;

    public Material poisonMaterial, radioactiveMaterial, biohazardMaterial, rainMaterial, 
        fogMaterial, sandMaterial, snowMaterial, fireMaterial, droughtMaterial;
    public Sprite poisonSymbol, radioactiveSymbol, biohazardSymbol, rainSymbol, fogSymbol, 
        sandSymbol, snowSymbol, fireSymbol, droughtSymbol;

    public Image symbol;
    public bool canPlayAnimation;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.Play("WarningSlideLeft");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.Play("WarningSlideRight");
        }
        if (canPlayAnimation)
        {
            anim.Play("WarningSlideLeft");
        }
        else
        {
            anim.Play("WarningSlideRight");
        }
    }
}//CLASS






























