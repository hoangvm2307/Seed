using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum WeatherState
{
    Fire,
    Hot,
    Normal,
    Fog,
    Rain,
    Snow,
    Freeze
}
public enum TypeDamage
{
    Fire,
    Radiation,
    Freeze,
    Drought,
    LowTemp,
    HighTemp,
    Gas,
    Water,
    Insect,
    Hurricane,
    Sand,
    Biohazard
}
public class Climate : MonoBehaviour
{
    #region singleton
    public static Climate instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    private TypeDamage typeDamage;
    public bool can_Start_Climating;
    public bool isRain, isHeavyRain, isSnow, isFog, isSandStorm, isGreenGas, isRadiation, isFire, isBiohazard, isDrought;
    public  ParticleSystem rainFX, heavyRainFX, snowFX, fogFX, sandStormFX, greenGasFX, radiationFX, fireFX, biohazardFX;
    GameplayController gameplayController;
    int rand_Climate, rand_Toxic;
    public bool start_Count_Down;
    private IEnumerator level_1_Climate_Cor, level_1_Toxic_Cor, level_2_Climate_Cor;
    private IEnumerator level_2_Toxic_Cor, level_3_Toxic_Cor, level_4_Toxic_Cor;
    private GameObject[] trees;
    GameObject[] grounds;
    public bool controlledByClimate; //adjusted by bool isFire, isSnow,... and stopped in StopClimate();
    [Header("Climate")]
    public float damage;
    public float damageRate;
    private int time_For_Climate;
    [SerializeField] private int climateTimer;
    [SerializeField] private int climateDuration;
    public int cliNum;
    public int quantityCliCounter;
    private bool canStopClimate;
     
    private float i, j, k, l;
    [Header("Weather")]
    public WeatherState weatherState;
    public bool canRainSound, canSnowSound, canFogSound;
    private AudioSource audioSource;
    public AudioClip[] weatherClip;
    public bool canHumidify;
    private bool canStopWeather;
    public int weatherStateNumber;
    public int weatherTimer;
    public int weatherDuration;
    public int norRate;
    public int rainRate;
    public int snowRate;
    public int fogRate;
    public Text TypeDays;


    private void Start()
    {
        weatherState = WeatherState.Normal;
        weatherDuration = 50;
        climateDuration = 25;
        weatherTimer = 40;
        climateTimer = 60;
        grounds = GameObject.FindGameObjectsWithTag("Ground");
        audioSource = GetComponent<AudioSource>();

        quantityCliCounter = 0;
        gameplayController = GameplayController.instance;
        level_1_Climate_Cor = Level_1_WaitForRandomClimateFX();
        level_2_Climate_Cor = Level_2_WaitForRandomClimateFX();
        level_1_Toxic_Cor = Level_1_WaitForRandomToxicFX();
        level_2_Toxic_Cor = Level_2_WaitForRandomToxicFX();
        level_3_Toxic_Cor = Level_3_WaitForRandomToxicFX();
        level_4_Toxic_Cor = Level_4_WaitForRandomToxicFX();
    }
    void Update()
    {
        if (!controlledByClimate) //Can vary state through isFire, isSnow,..
        {
            if (weatherStateNumber >= 0 && weatherStateNumber < 4) 
            {
                TypeDays.text = "NORMAL DAYS";
                weatherState = WeatherState.Normal;
            }//Normal days
            if (weatherStateNumber >= 4 && weatherStateNumber < 7)
            {
                TypeDays.text = "HOT DAYS";
                weatherState = WeatherState.Hot;
            }//Hot days
            if (weatherStateNumber >= 7 && weatherStateNumber < 11)
            {
                TypeDays.text = "RAINY DAYS";
                weatherState = WeatherState.Rain;
            }//Rainy days
            if (weatherStateNumber >= 11 && weatherStateNumber < 14)
            {
                TypeDays.text = "FOGGY DAYS";
                weatherState = WeatherState.Fog;
            }//Foggy days
            if (weatherStateNumber >= 14 && weatherStateNumber < 17)
            {
                TypeDays.text = "SNOWY DAYS";
                weatherState = WeatherState.Snow;
            }//Snowy days
            if (weatherStateNumber >= 17 && weatherStateNumber < 19)
            {
                TypeDays.text = "FREEZY DAYS";
                weatherState = WeatherState.Freeze;
            }//Freezy days
        }
        if(weatherStateNumber == 4 || weatherStateNumber == 7 || 
            weatherStateNumber == 11 || weatherStateNumber == 14 || weatherStateNumber == 17)
        {
            Temperature.instance.varyTimeCounter = 0;
            weatherStateNumber++;
        }

        ClimateCountdown(); StopClimate();
        WeatherCountDown(); StopWeather();
        if (isFire)
        {
            controlledByClimate = true;
            weatherState = WeatherState.Fire;
            DegradeGlobalGrounds(); // make grounds drought when fire
        }
        #region random climate and toxic
        if (!can_Start_Climating)
        {
            rand_Climate = Random.Range(0, 3);
            rand_Toxic = Random.Range(0, 9);
        }
        #endregion
        int random = Random.Range(0, 4);
        #region control climate
        if (gameplayController.can_Start_Climating)
        {
            if (gameplayController.oneTime) // need can_Start_Climating and oneTime as it need turning off oneTime 
            {
                //Forecast Climate

                if (gameplayController.Leaf_Counter == 6)
                {
                    StartCoroutine(level_1_Climate_Cor);
                }
                if (gameplayController.Leaf_Counter == 10)
                {
                    StartCoroutine(level_1_Climate_Cor);
                }
                if (gameplayController.Leaf_Counter == 14)
                {
                    int ranNum = Random.Range(0, 2);
                    if (ranNum == 0) StartCoroutine(level_1_Climate_Cor);
                    else StartCoroutine(level_1_Toxic_Cor);
                }
                if (gameplayController.Leaf_Counter == 18)
                {
                    StartCoroutine(level_1_Climate_Cor);
                    StartCoroutine(level_1_Toxic_Cor);
                }
                if (gameplayController.Leaf_Counter == 22)
                {
                    int ranNUm = Random.Range(0, 2);
                    if(ranNUm == 0) StartCoroutine(level_2_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_1_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 26)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_2_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_1_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 30)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_3_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_2_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 34)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_3_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_2_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 38)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_3_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_2_Climate_Cor);
                        StartCoroutine(level_2_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 42)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_4_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_2_Climate_Cor);
                        StartCoroutine(level_3_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 46)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_4_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_3_Toxic_Cor);
                    }
                }
                if (gameplayController.Leaf_Counter == 48)
                {
                    int ranNUm = Random.Range(0, 2);
                    if (ranNUm == 0) StartCoroutine(level_4_Toxic_Cor);
                    else
                    {
                        StartCoroutine(level_1_Climate_Cor);
                        StartCoroutine(level_3_Toxic_Cor);
                    }
                }
                 
                gameplayController.oneTime = false; // call coroutine once so that the function run smoothly
                start_Count_Down = true; // => countdown duration of climate

            }
        }
        #endregion

        if (!can_Start_Climating)
        {
            StopCoroutine(level_1_Climate_Cor);
            StopAllCoroutines();

        }
    }
    public void ResetClimate()
    {
        weatherState = WeatherState.Normal;
        weatherDuration = 50;
        weatherStateNumber = 0;
        climateDuration = 25;
        weatherTimer = 40;
        climateTimer = 60;
        quantityCliCounter = 0;
    }
    public void WeatherCountDown()
    {
        j -= Time.deltaTime;
        if (j <= 0)
        {
            weatherTimer -= 1; j = 1;
        } 
        if(weatherTimer == 20)
        {
            WeatherGenerator();
            weatherTimer = 19;
        }
        if(weatherTimer <= 0)
        {
            weatherDuration = 60;
            canStopWeather = true;

            if (isSnow)
            {
                gameplayController.can_Freeze_Trees = true;
                damage = 2;
                snowFX.Play();
                audioSource.clip = weatherClip[1];
                audioSource.Play();
            }
            if (isRain)
            {
                canHumidify = true;
                rainFX.Play();
                canRainSound = true;
                audioSource.clip = weatherClip[0];
                audioSource.Play();

            }
            if (isFog)
            {
                fogFX.Play();
            }

            if (weatherState != WeatherState.Normal)
            {
                int rand = Random.Range(0, 3);
                if (rand == 0) weatherTimer = 40 + weatherDuration;
                else if (rand == 1) weatherTimer = 50 + weatherDuration;
                else if (rand == 2) weatherTimer = 60 + weatherDuration;
            }
            else
            {
                int rand = Random.Range(0, 3);
                if (rand == 0) weatherTimer = 40;
                else if (rand == 1) weatherTimer = 50;
                else if (rand == 2) weatherTimer = 60;
            }
        }
        if (weatherStateNumber >= 28) weatherStateNumber = 0;
    }
    public void StopWeather()
    {
        if (canStopWeather)
        {
            k -= Time.deltaTime;
            if (k <= 0)
            {
                weatherDuration -= 1; k = 1;
            }
            if (weatherDuration <= 0)
            {
                isRain = isSnow = isFog = false;
                gameplayController.can_Freeze_Trees = false;
                damage = 0;
                rainFX.Stop(); fogFX.Stop(); snowFX.Stop();
                canStopWeather = false;
            }
        }
    }
    public void ClimateCountdown()
    {
        i -= Time.deltaTime;
        if(i <= 0)
        {
            climateTimer -= 1; i = 1;
        }
        if(climateTimer == 30)
        {
           ClimateGenerator();
            climateTimer = 29;
        }
        if (climateTimer <= 0)
        {
            if (isFire) fireFX.Play();
            if (isBiohazard) biohazardFX.Play();
            if (isGreenGas) greenGasFX.Play();
            if (isHeavyRain) heavyRainFX.Play();
            if (isSandStorm) sandStormFX.Play();
            if (isRadiation) radiationFX.Play();

            quantityCliCounter++;
            gameplayController.can_Damage_Trees = true;
            climateDuration = 25;
            canStopClimate = true; //Start countdown
            ClimateGenerator();
            int rand = Random.Range(0, 3);
            if (rand == 0) climateTimer = 60 + climateDuration;
            else if (rand == 1) climateTimer = 70 + climateDuration;
            else if (rand == 2) climateTimer = 75 + climateDuration;
        }
    }
    private void ClimateGenerator()
    {
        damage = (2 * damageRate);
        if (quantityCliCounter <= 3)
        {
            Level1Toxic(); 
        }
        else if(quantityCliCounter > 3  && quantityCliCounter <= 6)
        {
            Level2Toxic();
        }
        else if(quantityCliCounter > 6 && quantityCliCounter <= 9)
        {
            Level2Climate();
        }
        else if(quantityCliCounter > 9 && quantityCliCounter <= 12)
        {
            int ranNUm = Random.Range(0, 2);
            if (ranNUm == 0) Level3Toxic();
            else
            {
                Level2Climate(); Level1Toxic();
            }
        }
        else if(quantityCliCounter > 12)
        {
            int ranNUm = Random.Range(0, 1);
            if (ranNUm == 0) Level4Toxic();
            else if (ranNUm == 1)
            {
                Level2Toxic(); Level2Climate();
            }
        }
    }
    private void WeatherGenerator()
    {
        if (weatherState == WeatherState.Rain)
        {
            int chance = Random.Range(0, 10);
            print(chance);
            if (chance <= rainRate)
            {
                isRain = true; isSnow = isFog = isHeavyRain = false;
            }
            else
            {
                isRain = isSnow = isFog = false;
            }
        }
        if (weatherState == WeatherState.Snow)
        {
            int chance =  Random.Range(0, 10);
            if (chance <= snowRate)
            {
                isSnow = true; isRain = isFog = isHeavyRain = false;
            }
            else
            {
                isRain = isSnow = isFog = false;
            }
        }
        if (weatherState == WeatherState.Fog)
        {
            int chance = Random.Range(0, 10);
            if (chance <= fogRate)
            {
                isSnow = true; isRain = isFog = isHeavyRain = false;
            }
            else
            {
                isRain = isSnow = isFog = false;
            }
        }
        weatherStateNumber++;        
    }
    public void StopClimate()
    {
        if (canStopClimate)
        {
            l -= Time.deltaTime;
            if (l <= 0)
            {
                climateDuration -= 1; l = 1;
            }
            if (climateDuration <= 0)
            {
                gameplayController.can_Damage_Trees = false;
                canStopClimate = false;
                sandStormFX.Stop();fireFX.Stop();greenGasFX.Stop(); biohazardFX.Stop(); radiationFX.Stop();
                isBiohazard = isRadiation = isFire = isGreenGas = isHeavyRain = isSandStorm = false;
                damage = 0;
                controlledByClimate = false;

                GameObject[] leafGr = GameObject.FindGameObjectsWithTag("Leaf");
                foreach(GameObject lg in leafGr)
                {
                   LeafGround l =  lg.GetComponent<LeafGround>();
                    if(l.leafFunc == LeafFunction.FORECASTER)
                    {
                        l.ForecasterFX.GetComponent<ParticleSystem>().Stop();
                    }
                }
            }
        }
    }
    private void Level2Climate()
    {
        isHeavyRain = true; isFog = isSnow = isRain = false;
    }
    private void Level1Toxic()
    {
        int rand =  Random.Range(0, 5);
        switch (rand)
        {
            case 0:
                isGreenGas = true; isRadiation = isFire = isSandStorm = isBiohazard = false;
                break;
            case 1:
                isRadiation = true; isGreenGas = isFire = isSandStorm = isBiohazard = false;
                break;
            case 2:
                isFire = true; isRadiation = isGreenGas = isBiohazard = isSandStorm = false;
                break;
            case 3:
                isSandStorm = true; isRadiation = isFire = isGreenGas = isBiohazard = false;
                break;
            case 4:
                isBiohazard = true; isRadiation = isFire = isGreenGas = isSandStorm = false;
                break;
        }
    }
    private void Level2Toxic()
    {
        int rand = Random.Range(0, 6);
        switch (rand)
        {
            case 0:
                isGreenGas = isRadiation = true; isSandStorm = isFire = isBiohazard = false;
                break;
            case 1:
                isGreenGas = isFire = true; isSandStorm = isRadiation = isBiohazard = false;
                break;
            case 2:
                isGreenGas = isSandStorm = true; isFire = isRadiation = isBiohazard = false;
                break;
            case 3:
                isRadiation = isFire = true; isSandStorm = isGreenGas = isBiohazard = false;
                break;
            case 4:
                isSandStorm = isFire = true; isGreenGas = isRadiation = isBiohazard = false;
                break;
            case 5:
                isSandStorm = isRadiation = true; isGreenGas = isFire = isBiohazard = false;
                break;
        }
    }
    private void Level3Toxic()
    {
        int rand = Random.Range(0, 9);
        switch (rand)
        {
            case 0:
                isSandStorm = isRadiation = isGreenGas = true; isFire = isBiohazard = false;
                break;
            case 1:
                isSandStorm = isFire = isGreenGas = true; isRadiation = isBiohazard = false;
                break;
            case 2:
                isSandStorm = isRadiation = isFire = true; isGreenGas = isBiohazard = false;
                break;
            case 3:
                isGreenGas = isRadiation = isFire = true; isSandStorm = isBiohazard = false;
                break;
            case 4:
                isSandStorm = isRadiation = isBiohazard = true; isBiohazard = isFire = false;
                break;
            case 5:
                isSandStorm = isGreenGas = isBiohazard = true; isFire = isRadiation = false;
                break;
            case 6:
                isSandStorm = isFire = isBiohazard = true; isGreenGas = isRadiation = false;
                break;
            case 7:
                isFire = isGreenGas = isBiohazard = true; isRadiation = isSandStorm = false;
                break;
            case 8:
                isFire = isRadiation = isBiohazard = true; isGreenGas = isSandStorm = false;
                break;
        }
        #region old rand
        if (rand == 0)
        {
            isSandStorm = isRadiation = isGreenGas = true; isFire = isBiohazard = false;
        }
        else if (rand == 1)
        {
            isSandStorm = isFire = isGreenGas = true; isRadiation = isBiohazard = false;
        }
        else if (rand == 2)
        {
            isSandStorm = isRadiation = isFire = true; isGreenGas = isBiohazard = false;
        }
        else if (rand == 3)
        {
            isGreenGas = isRadiation = isFire = true; isSandStorm = isBiohazard = false;
        }
        else if (rand == 4)
        {
            isSandStorm = isRadiation = isBiohazard = true; isBiohazard = isFire = false;
        }
        else if(rand == 5)
        {
            isSandStorm = isGreenGas = isBiohazard = true; isFire = isRadiation = false;
        }
        else if(rand == 6)
        {
            isSandStorm = isFire = isBiohazard = true; isGreenGas = isRadiation = false;
        }
        else if(rand == 7)
        {
            isFire = isGreenGas = isBiohazard = true; isRadiation = isSandStorm = false;
        }
        else if(rand == 8)
        {
            isFire = isRadiation = isBiohazard = true; isGreenGas = isSandStorm = false;

        }
        #endregion
    }
    private void Level4Toxic()
    {
        isSandStorm = isRadiation = isGreenGas = isBiohazard = isFire = isSandStorm = true;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach(GameObject tr in trees)
        {
            Tree t = tr.GetComponent<Tree>();
            t.allAf = true;
        }
    }
    #region old climate generator
    IEnumerator Level_1_WaitForRandomClimateFX()
    {
        if (rand_Climate == 0)
        {
            isRain = true; isHeavyRain = isSnow = isFog = false;
            yield return new WaitForSeconds(seconds: time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            rainFX.Play();
        }
        if (rand_Climate == 1)
        {
            isFog = true; isHeavyRain = isSnow = isRain = false;
            yield return new WaitForSeconds(seconds: time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            fogFX.Play();
        }
        if(rand_Climate == 2)
        {
            isSnow = true; isHeavyRain = isFog = isRain = false;
            yield return new WaitForSeconds(seconds: time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            snowFX.Play();
        }
    }
    IEnumerator Level_2_WaitForRandomClimateFX()
    {
        if (rand_Climate == 0)
        {
            isHeavyRain = true; isFog = isSnow = isRain = false;
            yield return new WaitForSeconds(seconds: time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            heavyRainFX.Play();
        }
    }
    IEnumerator Level_1_WaitForRandomToxicFX()
    {
        gameplayController.can_Damage_Trees = true;
        if (rand_Toxic == 0)
        {
            isGreenGas = true; isRadiation = isFire = isSandStorm = isBiohazard =false;
            yield return new WaitForSeconds(time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            greenGasFX.Play();            
        }
        if (rand_Toxic == 1)
        {
            isRadiation = true; isRadiation = isFire = isSandStorm = false;
            yield return new WaitForSeconds(time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            radiationFX.Play();
        }
        if (rand_Toxic == 2)
        {
            isFire = true; isRadiation = isFire = isSandStorm = false;
            yield return new WaitForSeconds(time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            fireFX.Play();
            
        }
        if (rand_Toxic == 3)
        {
            isSandStorm = true; isRadiation = isFire = isGreenGas = false;
            yield return new WaitForSeconds(time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            sandStormFX.Play();
        }
        if (rand_Toxic == 4)
        {
            isSandStorm = true; isRadiation = isFire = isGreenGas = false;
            yield return new WaitForSeconds(time_For_Climate);
            InvokeRepeating("CountDown", 0, 1);
            damage = 10f;
            sandStormFX.Play();
        }
    }
    IEnumerator Level_2_WaitForRandomToxicFX()
    {
        if (rand_Toxic == 0)
        {
            isGreenGas = isRadiation = true; isSandStorm = isFire = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            greenGasFX.Play();
            radiationFX.Play();
        }
        if(rand_Toxic == 1)
        {
            isGreenGas = isFire = true; isSandStorm = isRadiation = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            greenGasFX.Play();
            fireFX.Play();
        }
        if(rand_Toxic == 2)
        {
            isGreenGas = isSandStorm = true; isFire = isRadiation = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            greenGasFX.Play();
            sandStormFX.Play();
        }
        if(rand_Toxic == 3)
        {
            isRadiation = isFire = true; isSandStorm = isGreenGas = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            fireFX.Play();
            radiationFX.Play();
        }
        if(rand_Toxic == 4)
        {
            isSandStorm = isFire = true; isGreenGas = isRadiation = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            fireFX.Play();
            sandStormFX.Play();
        }
        if(rand_Toxic == 5)
        {
            isSandStorm = isRadiation = true; isGreenGas = isFire = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 3));
            InvokeRepeating("CountDown", 0, 1);
            damage = 15f;
            radiationFX.Play();
            sandStormFX.Play();
        }
    }
    IEnumerator Level_3_WaitForRandomToxicFX()
    {
        if (rand_Toxic == 0)
        {
            isSandStorm = isRadiation = isGreenGas = true; isFire = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 7));
            InvokeRepeating("CountDown", 0, 1);
            damage = 20f;
            sandStormFX.Play();
            greenGasFX.Play();
            radiationFX.Play();
        }
        if (rand_Toxic == 1)
        {
            isSandStorm = isFire = isGreenGas = true; isRadiation = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 7));
            InvokeRepeating("CountDown", 0, 1);
            damage = 20f;
            sandStormFX.Play();
            greenGasFX.Play();
            fireFX.Play();
        }
        if (rand_Toxic == 2)
        {
            isSandStorm = isRadiation = isFire = true; isGreenGas = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 7));
            InvokeRepeating("CountDown", 0, 1);
            damage = 20f;
            sandStormFX.Play();
            radiationFX.Play();
            fireFX.Play();
        }
        if (rand_Toxic == 3)
        {
            isGreenGas = isRadiation = isFire = true; isSandStorm = false;
            yield return new WaitForSeconds(time_For_Climate + Random.Range(0, 7));
            InvokeRepeating("CountDown", 0, 1);
            damage = 20f;
            greenGasFX.Play();
            radiationFX.Play();
            fireFX.Play();
        }
    }
    IEnumerator Level_4_WaitForRandomToxicFX()
    {
        yield return new WaitForSeconds(time_For_Climate + Random.Range(0,10));
        InvokeRepeating("CountDown", 0, 1);
        damage = 25f;
        isSandStorm = isRadiation = isFire = isGreenGas = true; 
        greenGasFX.Play();
        radiationFX.Play();
        fireFX.Play();
        sandStormFX.Play();
    }
    #endregion
    private void DegradeGlobalGrounds()
    {
        foreach(GameObject ground in grounds)
        {
            ground.GetComponent<GroundController>().DegradingGround(2);
        }
    }
}//CLASS






































