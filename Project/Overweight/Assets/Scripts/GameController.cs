using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1Scale;
    public GameObject player2Scale;
    public GameObject player3Scale;
    public GameObject player4Scale;
    private float timer = 3;
    public int timeLimit = 20;
    private int timerAsInt;
    public Text timerUI;
    private int objectiveWeight = 10;
    private int player1Score;
    private int player2Score;
    private int player3Score;
    private int player4Score;



    void Start()
    {
        //this is where some game controller things will happen
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTimer();

    }

    void CalculateTimer()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(timer);
            timerUI.text = "Timer is: " + timerAsInt;
            Debug.Log("Time is: " + timerAsInt);
        }
        else
        {
            timerUI.text = "Time is up mother lickers!";
        }
    }

    void GetPlayerScales()
    {
        //for each player get the current score 
    }
}
