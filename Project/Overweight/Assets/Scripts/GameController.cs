using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerZoneControllerTemp player1Scale;
    public PlayerZoneControllerTemp player2Scale;
    public PlayerZoneControllerTemp player3Scale;
    public PlayerZoneControllerTemp player4Scale;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public int numberOfPlayers;
    private float timer = 5;
    private float newRoundTimer = 5;
    private int timerAsInt;
    public Text timerUI;
    public Text objectiveUI;
    private int objectiveWeightFrozen = 5;
    private int objectiveWeight;
    private int currentObjectiveWeight;

    private int player1Score;
    private int player2Score;
    private int player3Score;
    private int player4Score;

    private int player1WinCount;
    private int player2WinCount;
    private int player3WinCount;
    private int player4WinCount;

    private List<PlayerZoneControllerTemp> currentPlayers = new List<PlayerZoneControllerTemp>{};
    private List<PlayerZoneControllerTemp> perfectPlayerScores = new List<PlayerZoneControllerTemp> {};
    private List<PlayerZoneControllerTemp> currentPerfectPlayerScores = new List<PlayerZoneControllerTemp> { };
    private List<PlayerZoneControllerTemp> PlayerScores = new List<PlayerZoneControllerTemp> { };

    // Parcel Spawner game object reference
    [SerializeField] private GameObject parcel_spawner_object;

    void Start()
    {
        //this is where some game controller things will happen
        objectiveWeight = objectiveWeightFrozen;
        currentObjectiveWeight = objectiveWeightFrozen;
        GetPlayersList();
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTimer();
        if(timer > 0)
        {
           GetAndEvaluatePlayerScales();
           //GetAndEvaluateCurrentScores();
           //spawn parcels
        }
        
    }

    void CalculateTimer()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(timer);
            timerUI.text = "Timer is: " + timerAsInt;
            //Debug.Log("Time is: " + timerAsInt);
        }
        else
        {
            timerUI.text = "Time is up mother lickers!";
            DeactivateAllPlayers();
            GetAndEvaluateFinalScores();
        }
    }

    void CalculateEndRoundTimer()
    {
        if (newRoundTimer >= 0)
        {
            newRoundTimer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(newRoundTimer);
            timerUI.text = "New Round starts in: " + timerAsInt+"s";
            //Debug.Log("Time is: " + timerAsInt);
        }
        else
        {
            timerUI.text = "START!";
            ActivateAllPlayers();
            if (objectiveWeight > 0)
            {
                GetAndEvaluateFinalScores();
            }
        }
    }

    void ClearGame()
    {
        //wipe scores
        player1Score = 0;
        player2Score = 0;
        player3Score = 0;
        player4Score = 0;
        //wipe timers
    }

    void SetupGame()
    {
        //set timers
        timer = 5;
        newRoundTimer = 5;

        //generate and set objectives
        objectiveWeightFrozen = Mathf.RoundToInt(Random.Range(1, 20));
        objectiveWeight = objectiveWeightFrozen;
        currentObjectiveWeight = objectiveWeightFrozen;
        objectiveUI.text = "GOAL: " + objectiveWeightFrozen + "KG";
        //get all active players?
        GetPlayersList();
        //set player spawns
        //set item spawns

        // Setup parcel spawners here
        Vector3 parcelPosition_1 = new Vector3(0, 50, 0);
        var parcelSpawn_1 = (GameObject)Instantiate(parcel_spawner_object, parcelPosition_1, transform.rotation);
        parcelSpawn_1.GetComponent<parcel_spawner>().StartSpawning(7, 1, 0, 0, 0);
    }

    void GetAndEvaluateFinalScores()
    {
        if(objectiveWeight > 0)
        {
            int playerScore;
            perfectPlayerScores = new List<PlayerZoneControllerTemp> { };
            foreach (PlayerZoneControllerTemp playerScale in currentPlayers)
            {
                playerScore = playerScale.GetWeight();
                playerScore = objectiveWeight - playerScore;
                if (playerScore < 0)
                {
                    playerScore = playerScore * -1;
                }
                if (playerScore == 0)
                {
                    perfectPlayerScores.Add(playerScale);
                }
                playerScale.SetScore(playerScore);
                Debug.Log("Bedson Test: " + playerScore);
            }
            if (perfectPlayerScores.Count > 0)
            {
                foreach (PlayerZoneControllerTemp playerScale in perfectPlayerScores)
                {
                    if (objectiveWeight == objectiveWeightFrozen)
                    {
                        Color winCol = new Color(0f, 1f, 0f, 1f);
                        playerScale.SetEvaluationTextColour(winCol);
                        playerScale.SetEvaluationText("PERFECT WINNER!!!");
                    }
                    else
                    {
                        playerScale.SetEvaluationText("WINNER!!!");
                    }

                }
            }
            else
            {
                //no perfect score so we get the closest
                objectiveWeight--;
                GetAndEvaluateFinalScores();
            }
        }
        
    }

    //void GetAndEvaluateCurrentScores()
    //{
    //    Debug.Log("Bedson Test1: " + currentObjectiveWeight);
    //    int playerScore;
    //    currentPerfectPlayerScores = new List<PlayerZoneControllerTemp> { };
    //    foreach (PlayerZoneControllerTemp playerScale in currentPlayers)
    //    {
    //        playerScore = playerScale.GetWeight();
    //        playerScore = currentObjectiveWeight - playerScore;
    //        if (playerScore < 0)
    //        {
    //            playerScore = playerScore * -1;
    //        }
    //        if (playerScore == 0)
    //        {
    //            currentPerfectPlayerScores.Add(playerScale);
    //        }
    //        playerScale.SetScore(playerScore);
    //    }
    //    if (currentPerfectPlayerScores.Count > 0)
    //    {
    //        foreach (PlayerZoneControllerTemp playerScale in currentPerfectPlayerScores)
    //        {
    //            if (currentObjectiveWeight == objectiveWeightFrozen)
    //            {
    //                Color winCol = new Color(0f, 1f, 0f, 1f);
    //                playerScale.SetEvaluationTextColour(winCol);
    //                playerScale.SetEvaluationText("Lead Player!");
                    
    //            }
    //            else
    //            {
    //                playerScale.SetEvaluationText("Joint Lead Player!");
    //            }
                
    //        }
    //        currentObjectiveWeight = objectiveWeightFrozen;
    //        //GetAndEvaluateCurrentScores();
    //    }
    //    else
    //    {
    //        //no perfect score so we get the closest
    //        Debug.Log("Bedson Test: "+ currentObjectiveWeight);
    //        currentObjectiveWeight--;
    //        GetAndEvaluateCurrentScores();
    //    }
    //}

    void SortScoreOutFam(PlayerZoneControllerTemp player, int score)
    {
        if (player1Scale != null)
        {
            player1Score = player1Scale.GetWeight();
            player1Score = objectiveWeight - player1Score;
            if (player1Score < 0)
            {
                //its a minus, so lets normalise it?
                player1Score = player1Score * -1;
            }
        }
    }

    void GetAndEvaluatePlayerScales()
    {
        //for each player get the current score 
        foreach(PlayerZoneControllerTemp playerScale in currentPlayers)
        {
            int weight = playerScale.GetWeight();
            EvaluatePlayerScore(weight, playerScale);
        }
    }

    void DeactivateAllPlayers()
    {
        foreach (PlayerZoneControllerTemp playerScale in currentPlayers)
        {
            playerScale.Deactivate();
        }
    }

    void ActivateAllPlayers()
    {
        foreach (PlayerZoneControllerTemp playerScale in currentPlayers)
        {
            playerScale.Activate();
        }
    }


    void EvaluatePlayerScore(int playerScore, PlayerZoneControllerTemp playerScale)
    {
        //if (playerScore == objectiveWeight)
        //{
        //    playerScale.SetEvaluationText("PERFECT WEIGHT!");
        //}
        //else 
        
        if(playerScore > objectiveWeight)
        {
            Color overCol = new Color(1f, 0f, 1f, 1f);
            playerScale.SetEvaluationTextColour(overCol);
            playerScale.SetEvaluationText("OVERWEIGHT!");
        }
        else if(playerScore < objectiveWeight)
        {
            Color neutCol = new Color(1f, 1f, 1f, 1f);
            playerScale.SetEvaluationTextColour(neutCol);
            playerScale.SetEvaluationText("UNDERWEIGHT!");
        }
    }

    public int getObjectiveWeight()
    {
        return objectiveWeight;
    }

    void GetPlayersList()
    {
        if(player1Scale != null)
        {
            currentPlayers.Add(player1Scale);
        }
        if (player2Scale != null)
        {
            currentPlayers.Add(player2Scale);
        }
        if (player3Scale != null)
        {
            currentPlayers.Add(player3Scale);
        }
        if (player4Scale != null)
        {
            currentPlayers.Add(player4Scale);
        }
    }
}
