using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class GO : MonoBehaviour
{

    public GameObject icons;
    private GameObject iconInstance;

    public Sprite[] grass;
    public Sprite tilled;
    public Sprite[] growthStage;
    public Sprite[] utility;
    public Sprite[] ripeBush;
    public Sprite[] indicators;
    public int x, y;
    public float alarmToGrass, alarmToGrowing, alarmToBush, alarmToBud, alarmToFlower, alarmToRipe;

    // private Sprite tempSprite;
    private enum Type { Grass, Tilled, Planted, Growing, Bush, Bud, Flower, Ripe };
    private enum BushType { NoBush, RedBush, BlueBush, GreenBush, PurpleBush };
    private Type type;
    private BushType bushType;
    private float ripeTimer, timer, grassTimer, alarm2grass, alarm2growing, alarm2bush, alarm2bud, alarm2flower, alarm2ripe;
    private int numHarvested, bushHarvestLife, minHarvest, maxHarvest;
    private GameObject playerScript, boardScript;
    private bool planted, mulched, watered;

    public void OnMouseDown()
    {
        switch (type)
        {
            case Type.Grass:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().hoe)
                        TurnTilled();
                    break;
                }
            case Type.Tilled:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().plantSeed)
                    {
                        TurnPlanted();
                        // this make sure its right bush type
                        if (playerScript.GetComponent<Player>().seed == playerScript.GetComponent<Player>().redSeed)
                        {
                            playerScript.GetComponent<Player>().hoorays -= boardScript.GetComponent<BoardManager>().redPrice;
                            bushType = BushType.RedBush;
                        }
                        else if (playerScript.GetComponent<Player>().seed == playerScript.GetComponent<Player>().blueSeed)
                        {
                            playerScript.GetComponent<Player>().hoorays -= boardScript.GetComponent<BoardManager>().bluePrice;
                            bushType = BushType.BlueBush;
                        }
                        else if (playerScript.GetComponent<Player>().seed == playerScript.GetComponent<Player>().greenSeed)
                        {
                            playerScript.GetComponent<Player>().hoorays -= boardScript.GetComponent<BoardManager>().greenPrice;
                            bushType = BushType.GreenBush;
                        }
                        else if (playerScript.GetComponent<Player>().seed == playerScript.GetComponent<Player>().purpleSeed)
                        {
                            playerScript.GetComponent<Player>().hoorays -= boardScript.GetComponent<BoardManager>().purplePrice;
                            bushType = BushType.PurpleBush;
                        }
                    }
                    else if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().mulch)
                        Mulch();
                    break;
                }
            case Type.Planted:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().mulch)
                        Mulch();
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().watercan)
                        Water();
                    break;
                }
            case Type.Growing:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().mulch)
                        Mulch();
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().watercan)
                        Water();
                    break;
                }
            case Type.Bush:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().watercan)
                        Water();
                    break;
                }
            case Type.Bud:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().watercan)
                        Water();
                    break;
                }
            case Type.Flower:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().watercan)
                        Water();
                    break;
                }
            case Type.Ripe:
                {
                    if (playerScript.GetComponent<Player>().tool == playerScript.GetComponent<Player>().hand)
                        TurnHarvest();
                    break;
                }
        }
        // Debug.Log ("Tile" + "[" + x + "][" + y + "] : "  + type + " " + bushType + "\n");
    }

    public void OnMouseUp()
    {
        switch (type)
        {
            case Type.Tilled:
                {
                    TurnTilled();
                    break;
                }
            case Type.Planted:
                {
                    this.GetComponent<SpriteRenderer>().sprite = growthStage[0];
                    break;
                }
            case Type.Growing:
                {
                    this.GetComponent<SpriteRenderer>().sprite = growthStage[1];
                    break;
                }
            case Type.Bush:
                {
                    this.GetComponent<SpriteRenderer>().sprite = growthStage[2];
                    break;
                }
            case Type.Bud:
                {
                    this.GetComponent<SpriteRenderer>().sprite = growthStage[3];
                    break;
                }
            case Type.Flower:
                {
                    this.GetComponent<SpriteRenderer>().sprite = growthStage[4];
                    break;
                }
        }
    }

    void Start()
    {
        playerScript = GameObject.Find("Player");
        boardScript = GameObject.Find("BoardManager");
        numHarvested = 0;
        ripeTimer = 0;
        ChangeMaxHarvest();
        type = Type.Grass;
        bushType = BushType.PurpleBush;
        TurnGrass();
        ResetAlarms();

        iconInstance = Instantiate(icons, new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        iconInstance.transform.parent = gameObject.transform;
        iconInstance.name = "icon[" + transform.position.x + "][" + transform.position.y + "]";
    }

    // update timer and check alarms
    void Update()
    {
        if (type == Type.Ripe)
        {
            if (ripeTimer > 2000000000)
                ripeTimer = 0;
            ripeTimer += Time.deltaTime * Random.Range(1, 3);
        }
        // Debug.Log (ripeTimer + "\n");

        // don't start growing unless watered
        if (planted)
            timer += Time.deltaTime;
        else if (type == Type.Tilled)
            grassTimer += Time.deltaTime;

        // turn back to grass from tilled dirt
        if (grassTimer >= alarm2grass)
        {
            if (type == Type.Tilled)
                TurnGrass();
        }

        // growth stages
        if (timer >= alarm2ripe)
        {
            if (type == Type.Ripe)
                ChangeRipeSprite();
            else if (type == Type.Flower)
                TurnRipe();
        }
        else if (timer >= alarm2flower)
        {
            if (type == Type.Bud)
                TurnFlower();
        }
        else if (timer >= alarm2bud)
        {
            if (type == Type.Bush)
                TurnBud();
        }
        else if (timer >= alarm2bush)
        {
            if (type == Type.Growing)
                TurnBush();
        }
        else if (timer >= alarm2growing)
        {
            if (type == Type.Planted)
                TurnGrowing();
        }

        CheckIndicators();
    }

    // resets the timer & alarms
    void ResetAlarms()
    {
        timer = 0;
        ripeTimer = 0;
        mulched = false;
        watered = false;

        alarm2grass = alarmToGrass;
        alarm2growing = alarmToGrowing;
        alarm2bush = alarmToBush;
        alarm2bud = alarmToBud;
        alarm2flower = alarmToFlower;
        alarm2ripe = alarmToRipe;

        /*/
		alarm2grass = Random.Range((alarmToGrass - alarmToGrass * .25f), (alarmToGrass + alarmToGrass * .25f));
		alarm2bush = Random.Range((alarmToBush - alarmToBush * .25f), (alarmToBush + alarmToBush * .25f));
		alarm2bud = Random.Range((alarmToBud - alarmToBud * .25f), (alarmToBud + alarmToBud * .25f));
		alarm2flower = Random.Range((alarmToFlower - alarmToFlower * .25f), (alarmToFlower + alarmToFlower * .25f));
		alarm2ripe = Random.Range((alarmToRipe - alarmToRipe * .25f), (alarmToRipe + alarmToRipe * .25f));
		*/
    }

    void ChangeMaxHarvest()
    {
        bushHarvestLife = Random.Range(1, 4);
    }

    // utility methods
    void Mulch()
    {
        if (!mulched)
        {
            this.GetComponent<SpriteRenderer>().sprite = utility[1];
            mulched = true;
        }
        else
            this.GetComponent<SpriteRenderer>().sprite = utility[0];
    }

    void Water()
    {
        if (!watered)
        {
            this.GetComponent<SpriteRenderer>().sprite = utility[2];
            watered = true;
        }
        else
            this.GetComponent<SpriteRenderer>().sprite = utility[0];
    }

    void CheckIndicators()
    {
        // Debug.Log("Go.cs CheckIndicators()");
        if (!mulched && !watered)
            iconInstance.GetComponent<SpriteRenderer>().enabled = false;
        else
        {
            iconInstance.GetComponent<SpriteRenderer>().enabled = true;
            if (mulched)
                iconInstance.GetComponent<SpriteRenderer>().sprite = indicators[1];
            if (watered)
                iconInstance.GetComponent<SpriteRenderer>().sprite = indicators[0];
            if (mulched && watered)
                iconInstance.GetComponent<SpriteRenderer>().sprite = indicators[2];
        }
    }

    // Turn methods ~ updates game tiles
    void TurnGrass()
    {
        type = Type.Grass;
        mulched = false;
        watered = false;
        this.GetComponent<SpriteRenderer>().sprite = grass[Random.Range(0, 3)];
    }

    void TurnTilled()
    {
        grassTimer = 0;
        type = Type.Tilled;
        this.GetComponent<SpriteRenderer>().sprite = tilled;
    }

    void TurnPlanted()
    {
        // watered = false;
        type = Type.Planted;
        planted = true;
        this.GetComponent<SpriteRenderer>().sprite = growthStage[0];
    }

    void TurnGrowing()
    {
        // watered = false;
        type = Type.Growing;
        this.GetComponent<SpriteRenderer>().sprite = growthStage[1];
    }

    void TurnBush()
    {
        watered = false;
        type = Type.Bush;
        this.GetComponent<SpriteRenderer>().sprite = growthStage[2];
    }

    void TurnBud()
    {
        // watered = false;
        type = Type.Bud;
        this.GetComponent<SpriteRenderer>().sprite = growthStage[3];
    }

    void TurnFlower()
    {
        // watered = false;
        type = Type.Flower;
        this.GetComponent<SpriteRenderer>().sprite = growthStage[4];
    }

    void TurnRipe()
    {
        type = Type.Ripe;
        ChangeRipeSprite();
    }

    void ChangeRipeSprite()
    {
        switch (bushType)
        {
            case BushType.RedBush:
                {
                    if ((int)ripeTimer % 7 == 0)
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[1];
                    else
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[0];
                    break;
                }
            case BushType.BlueBush:
                {
                    if ((int)ripeTimer % 7 == 0)
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[2];
                    else
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[3];
                    break;
                }
            case BushType.GreenBush:
                {
                    if ((int)ripeTimer % 7 == 0)
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[4];
                    else
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[5];
                    break;
                }
            case BushType.PurpleBush:
                {
                    if ((int)ripeTimer % 7 == 0)
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[6];
                    else
                        this.GetComponent<SpriteRenderer>().sprite = ripeBush[7];
                    break;
                }
        }
    }

    void TurnHarvest()
    {
        TurnBush();
        timer = alarm2bush;

        numHarvested++;

        if (numHarvested >= bushHarvestLife)
        {
            TurnTilled();
            numHarvested = 0;
            ChangeMaxHarvest();
            ResetAlarms();
            planted = false;
        }

        // mulch only affects first harvest until replanted
        if (mulched)
        {
            minHarvest = 10;
            maxHarvest = 18;
        }
        else
        {
            minHarvest = 7;
            maxHarvest = 12;
        }

        switch (bushType)
        {
            case BushType.RedBush:
                {
                    playerScript.GetComponent<Player>().redBerries += Random.Range(minHarvest, maxHarvest);
                    // Debug.Log ("Tile" + "[" + x + "][" + y + "] : " /* + bushType + " score = " */ + purpleScore /* + " " + numHarvested */ + "\n");
                    break;
                }
            case BushType.BlueBush:
                {
                    playerScript.GetComponent<Player>().blueBerries += Random.Range(minHarvest, maxHarvest);
                    break;
                }
            case BushType.GreenBush:
                {
                    playerScript.GetComponent<Player>().greenBerries += Random.Range(minHarvest, maxHarvest);
                    break;
                }
            case BushType.PurpleBush:
                {
                    playerScript.GetComponent<Player>().purpleBerries += Random.Range(minHarvest, maxHarvest);
                    break;
                }
        }
    }
}
