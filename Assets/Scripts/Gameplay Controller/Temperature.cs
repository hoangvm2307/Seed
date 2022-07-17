using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Temperature : MonoBehaviour
{
    #region
    public static Temperature instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    Climate climate;
    [SerializeField] Text temperatureText;
    public GameObject inputField;
    public int temperature;
    [HideInInspector] public int predefinedTemper; //adjusted in Climate
    public int inputTemper;
    public Text inputFieldText;
    public bool canVaryTemper;
    public int varyTimeCounter;
    private float i,j;
    public bool somethingcool;
    void Start()
    {
        inputTemper = 0;
        i = 1f;
        j = 1f;
        climate = Climate.instance;
        temperature = 28;
        varyTimeCounter = 60;
        predefinedTemper = 23;
    }
    void Update()
    {
        //EnterTemperature();
        AdjustTemperature();
        j -= Time.deltaTime;
        if(j <= 0)
        {
            varyTimeCounter -= 1; j = 1;
        }
        if(varyTimeCounter <= 0)
        {
            if (Climate.instance.weatherState == WeatherState.Fire)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(55, 61);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(50, 55);
            }
            if (Climate.instance.weatherState == WeatherState.Hot)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(42, 50);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(36, 42);
            }
            if (Climate.instance.weatherState == WeatherState.Normal)
            {
                if(DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(30, 36);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(23, 30);
            }
            if(Climate.instance.weatherState == WeatherState.Fog)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(20, 23);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(17, 20);
            }
            if(Climate.instance.weatherState == WeatherState.Rain)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(17, 23);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(14, 17);
            }
            if(Climate.instance.weatherState == WeatherState.Snow)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(8, 14);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(1, 8);

            }
            if(Climate.instance.weatherState == WeatherState.Freeze)
            {
                if (DayNight.instance.session == Session.Day)
                    predefinedTemper = Random.Range(-10, 1);
                if (DayNight.instance.session == Session.Night)
                    predefinedTemper = Random.Range(-20,-10);

            }
            varyTimeCounter = 180;
        }
    }
    public void ReadTemperInput()
    {
        int inputTemper = int.Parse(inputFieldText.text);
        Climate.instance.controlledByClimate = true;
        if (inputTemper < 62 && inputTemper >= 50)
        {
            Climate.instance.weatherState = WeatherState.Fire;
            Climate.instance.weatherStateNumber = 5;
        }
        else if (inputTemper < 50 && inputTemper >= 36)
        {
            Climate.instance.weatherState = WeatherState.Hot;
            Climate.instance.weatherStateNumber = 5;
        }
        else if (inputTemper < 36 && inputTemper >= 23)
        {
            Climate.instance.weatherState = WeatherState.Normal;
            Climate.instance.weatherStateNumber = 1;
        }
        else if (inputTemper < 23 && inputTemper >= 17)
        {
            Climate.instance.weatherState = WeatherState.Rain;
            Climate.instance.weatherStateNumber = 8;
        }
        else if (inputTemper < 17 && inputTemper >= 14)
        {
            Climate.instance.weatherState = WeatherState.Fog;
            Climate.instance.weatherStateNumber = 17;
        }
        else if (inputTemper < 14 && inputTemper >= 1)
        {
            Climate.instance.weatherState = WeatherState.Snow;
            Climate.instance.weatherStateNumber = 23;
        }
        else if (inputTemper < 1 && inputTemper >= -20)
        {
            Climate.instance.weatherState = WeatherState.Freeze;
            Climate.instance.weatherStateNumber = 27;
        }
        predefinedTemper = inputTemper; //I use predefinedTemper, inputTemper is just temporary
        inputField.SetActive(false);
        print(inputTemper);
    }
    void AdjustTemperature()
    {
        if (climate.isRain)
        {
            i -= (Time.deltaTime * 1.5f);
            if(i <= 0)
            {
                temperature -= 1;
                i = Random.Range(0.8f, 1);
                if (temperature <= 14) temperature = predefinedTemper;
            }
        }
        else
        {
            i -= (Time.deltaTime * 1.5f);
            if(i <= 0)
            {
                TranslateTemperature(predefinedTemper);
                i = Random.Range(0.7f,1);
            }
        }
        if(Climate.instance.weatherState == WeatherState.Normal)
        {
            if(canVaryTemper)
            {
                TranslateTemperature(30);
            }
        }
        temperatureText.text = temperature.ToString();
    }//adjust temperature
    void TranslateTemperature(int preTemper)
    {
        if (temperature < preTemper) temperature++;
        else if (temperature > preTemper) temperature--;
    }

}//CLASS













































