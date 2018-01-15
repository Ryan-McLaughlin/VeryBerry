using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;

    public float health;
    public float experience;

    void Awake()

    {
        if (gameControl == null)
        {
            DontDestroyOnLoad(gameObject);
            gameControl = this;
        }
        else if (gameControl != this)
        {
            Destroy(gameObject);
        }
        
    }

    void OnGUI()
    {
        // OnGUI.Lable(new Rect(10, 10, 100, 30), "Health: " + health);
        // OnGUI.Lable(new Rect(10, 40, 150, 30), "Experience: " + experience);
    }
}
