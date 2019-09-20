using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    private float timer = 3;
    public int timeLimit = 20;
    private int timerAsInt;
    public Text timerUI;
    private int objectiveWeight = 10;
    private int player1Scale;
    private int player2Scale;
    private int player3Scale;
    private int player4Scale;



    void Start()
    {
        //this is where some game controller things will happen
    }

    // Update is called once per frame
    void Update()
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

    void CalculateTimer()
    {

    }
}
