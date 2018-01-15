using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public enum Tools { Hoe, WaterCan, Mulch, Hand, PlantSeed, SeedSelect };
    public enum Seeds { Red, Blue, Green, Purple };
    public Tools tool;
    public Seeds seed;

    // cant check enum in an expression in another script, so needed these TODO find better way
    public Tools hoe;
    public Tools watercan;
    public Tools mulch;
    public Tools hand;
    public Tools plantSeed;
    public Tools seedSelect;

    public Seeds redSeed;
    public Seeds blueSeed;
    public Seeds greenSeed;
    public Seeds purpleSeed;

    public int hoorays, gems;
    public int redBerries, blueBerries, greenBerries, purpleBerries;

    void Start()
    {
        tool = Tools.Hand;
        hoorays = 100;
        gems = 10;
        redBerries = blueBerries = greenBerries = purpleBerries = 0;
        setBerries();
        setTools();
    }

    void setBerries()
    {
        redSeed = Seeds.Red;
        blueSeed = Seeds.Blue;
        greenSeed = Seeds.Green;
        purpleSeed = Seeds.Purple;
    }

    void setTools()
    {
        hoe = Tools.Hoe;
        watercan = Tools.WaterCan;
        mulch = Tools.Mulch;
        hand = Tools.Hand;
        plantSeed = Tools.PlantSeed;
        seedSelect = Tools.SeedSelect;
    }
}
