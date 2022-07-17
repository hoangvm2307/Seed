using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Session
{
    Day,
    Night
}
public class DayNight : MonoBehaviour
{
    #region singleton
    public static DayNight instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] Text CurrentSession;
    [SerializeField] Text CurrentDay;
    public Session session;
    public float dayTime;
    private float i, j;
    public float rate;
    private int currentDay;
    GameObject[] trees;
    public GameObject[] background;
    void Start()
    {
        dayTime = 50; i = j = 1;
        currentDay = 1;
    }
    void Update()
    {
        trees = GameObject.FindGameObjectsWithTag("Tree");
        CurrentDay.text = "Day " + currentDay;
        if (session == Session.Day && !GameplayController.instance.onInstruction)
        {
            Day();
            CurrentSession.text = dayTime + " until night";
        }
        else if (session == Session.Night && !GameplayController.instance.onInstruction)
        {
            Night();
            CurrentSession.text = dayTime + " until day";
        }
    }
    public void ResetDayAndNight()
    {
        dayTime = 50;
        session = Session.Day;
        CurrentSession.text = dayTime + " until night";
        foreach (GameObject bg in background)
        {
            bg.GetComponent<SpriteRenderer>().sprite = GameplayController.instance.dayBG;
        }
    }
    private void Day()
    {
        i -= Time.deltaTime;
        if (i <= 0)
        {
            dayTime -= 1 * rate; i = 1;
        }
        if (dayTime <= 0)
        {
            session = Session.Night;
            foreach(GameObject bg in background)
            {
                bg.GetComponent<SpriteRenderer>().sprite = GameplayController.instance.nightBG;
            }
            dayTime = 50;
        }
    }
    private void Night()
    {
        j -= Time.deltaTime;
        if (j <= 0)
        {
            dayTime -= 1; j = 1;
        }
        if (dayTime <= 0)
        {
            session = Session.Day;
            foreach(GameObject trees in trees)
            {
                trees.GetComponent<Tree>().DayEndSpawnNewLeaf();
            }
            foreach (GameObject bg in background)
            {
                bg.GetComponent<SpriteRenderer>().sprite = GameplayController.instance.dayBG;
            }
            currentDay++;
            dayTime = 50;
        }
    }

}//CLASS







































