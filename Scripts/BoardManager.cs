using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{

    public static BoardManager instance = null;
    public GameObject go;
    public int columns, rows;
    public float seasonLength;
    public enum Season { Red, Blue, Green, Purple };
    public Season season;
    public int freeTier, cheapTier, mediumTier, expensiveTier;
    public int freeConversion, cheapConversion, mediumConversion, expensiveConversion;
    public int redPrice, bluePrice, greenPrice, purplePrice;
    public bool debug;

    private Transform tileHolder;
    private Text currencyText, berriesText, seasonText;
    private GameObject playerScript;
    private int redBerries, blueBerries, greenBerries, purpleBerries;
    private float seasonTimer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        playerScript = GameObject.Find("Player");
        tileHolder = new GameObject("Tiles").transform;

        SetBoard();
        season = Season.Purple;
        ChangeSeason();

        currencyText = GameObject.Find("Currency").GetComponent<Text>();
        berriesText = GameObject.Find("Berries").GetComponent<Text>();
        seasonText = GameObject.Find("Season").GetComponent<Text>();
    }

    void Update()
    {
        seasonTimer += Time.deltaTime;
        if (seasonTimer >= seasonLength)
            ChangeSeason();

        redBerries = playerScript.GetComponent<Player>().redBerries;
        blueBerries = playerScript.GetComponent<Player>().blueBerries;
        greenBerries = playerScript.GetComponent<Player>().greenBerries;
        purpleBerries = playerScript.GetComponent<Player>().purpleBerries;

        currencyText.text = "Hoorays: " + playerScript.GetComponent<Player>().hoorays;// + "\nGems: " + playerScript.GetComponent<Player>().gems;
        berriesText.text = "R : " + redBerries + "\nB : " + blueBerries + "\nG : " + greenBerries + "\nP : " + purpleBerries;
        seasonText.text = season + " Season";

        if (debug)
        {
            if (Input.GetKeyUp(KeyCode.G))
                playerScript.GetComponent<Player>().gems += 10;
            if (Input.GetKeyUp(KeyCode.H))
                playerScript.GetComponent<Player>().hoorays += 10;
            if (Input.GetKeyUp(KeyCode.S))
                ChangeSeason();
            if (Input.GetKeyUp(KeyCode.R))
                playerScript.GetComponent<Player>().redBerries += 10;
            if (Input.GetKeyUp(KeyCode.B))
                playerScript.GetComponent<Player>().blueBerries += 10;
            if (Input.GetKeyUp(KeyCode.G))
                playerScript.GetComponent<Player>().greenBerries += 10;
            if (Input.GetKeyUp(KeyCode.P))
                playerScript.GetComponent<Player>().purpleBerries += 10;
        }
    }

    void ChangeSeason()
    {
        seasonTimer = 0;
        int tmp = 0;
        if (season == Season.Red)
        {
            redPrice = freeTier;
            bluePrice = cheapTier;
            greenPrice = mediumTier;
            purplePrice = expensiveTier;

            tmp += (int)playerScript.GetComponent<Player>().redBerries / mediumConversion;
            tmp += (int)playerScript.GetComponent<Player>().blueBerries / cheapConversion;
            tmp += (int)playerScript.GetComponent<Player>().greenBerries / expensiveConversion;
            tmp += (int)playerScript.GetComponent<Player>().purpleBerries / freeConversion;
            ResetBerries();
            playerScript.GetComponent<Player>().hoorays += tmp;

            season = Season.Blue;
        }
        else if (season == Season.Blue)
        {
            redPrice = expensiveTier;
            bluePrice = freeTier;
            greenPrice = cheapTier;
            purplePrice = mediumTier;

            tmp += (int)playerScript.GetComponent<Player>().redBerries / freeConversion;
            tmp += (int)playerScript.GetComponent<Player>().blueBerries / mediumConversion;
            tmp += (int)playerScript.GetComponent<Player>().greenBerries / cheapConversion;
            tmp += (int)playerScript.GetComponent<Player>().purpleBerries / expensiveConversion;
            ResetBerries();
            playerScript.GetComponent<Player>().hoorays += tmp;

            season = Season.Green;
        }
        else if (season == Season.Green)
        {
            redPrice = mediumTier;
            bluePrice = expensiveTier;
            greenPrice = freeTier;
            purplePrice = cheapTier;

            tmp += (int)playerScript.GetComponent<Player>().redBerries / expensiveConversion;
            tmp += (int)playerScript.GetComponent<Player>().blueBerries / freeConversion;
            tmp += (int)playerScript.GetComponent<Player>().greenBerries / mediumConversion;
            tmp += (int)playerScript.GetComponent<Player>().purpleBerries / cheapConversion;
            ResetBerries();
            playerScript.GetComponent<Player>().hoorays += tmp;

            season = Season.Purple;
        }
        else if (season == Season.Purple)
        {
            redPrice = cheapTier;
            bluePrice = mediumTier;
            greenPrice = expensiveTier;
            purplePrice = freeTier;

            tmp += (int)playerScript.GetComponent<Player>().redBerries / cheapConversion;
            tmp += (int)playerScript.GetComponent<Player>().blueBerries / expensiveConversion;
            tmp += (int)playerScript.GetComponent<Player>().greenBerries / freeConversion;
            tmp += (int)playerScript.GetComponent<Player>().purpleBerries / mediumConversion;
            ResetBerries();
            playerScript.GetComponent<Player>().hoorays += tmp;

            season = Season.Red;
        }
        Debug.Log(season + " season\n");
        Debug.Log(tmp + "\n");
    }

    void ResetBerries()
    {
        playerScript.GetComponent<Player>().redBerries = 0;
        playerScript.GetComponent<Player>().blueBerries = 0;
        playerScript.GetComponent<Player>().greenBerries = 0;
        playerScript.GetComponent<Player>().purpleBerries = 0;
    }

    void SetBoard()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiate = go;
                GameObject instance = Instantiate(toInstantiate, new Vector2(x, y), Quaternion.identity) as GameObject;
                instance.name = "go[" + x + "][" + y + "]";
                instance.transform.SetParent(tileHolder);
                GO script = go.GetComponent<GO>();
                // script.x = x;
                // script.y = y;
            }
        }
    }
}
