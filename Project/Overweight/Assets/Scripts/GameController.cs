using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public ScaleZone player1Scale;
    public ScaleZone player2Scale;
    public ScaleZone player3Scale;
    public ScaleZone player4Scale;
    [SerializeField] private GameObject playerObject;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    [SerializeField] private GameObject player1SpawnLocation;
    [SerializeField] private GameObject player2SpawnLocation;
    [SerializeField] private GameObject player3SpawnLocation;
    [SerializeField] private GameObject player4SpawnLocation;

    public int numberOfPlayers;
    private float timer;
    private float newRoundTimer = 5;
    private float timerFrozen = 25;
    private int timerAsInt;
    private bool roundScoreCalculated = false;
    private bool playersSet = false;
    private int lastPlayerScoreUpdate=0;
    private int closestScoreValue = 1;

    private bool playersReady = true;
    private bool readyPlayer1 = false;
    private bool readyPlayer2 = false;
    private bool readyPlayer3 = false;
    private bool readyPlayer4 = false;

    public Text timerUI;
    public Text objectiveUI;

    public Text player1WinUI;
    public Text player2WinUI;
    public Text player3WinUI;
    public Text player4WinUI;

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

    private List<ScaleZone> currentPlayers = new List<ScaleZone>{};
    private List<ScaleZone> perfectPlayerScores = new List<ScaleZone> {};
    private List<ScaleZone> closestPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> overweightPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> currentPerfectPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> PlayerScores = new List<ScaleZone> { };

    // Parcel Spawner game object reference
    [SerializeField] private GameObject parcel_spawner_object;

    // Parcel destruction setup
    parcel_manager parcel_man = new parcel_manager();

    // Parcel spawn setup
    //parcel_spawner spw_parcel;

    void Start()
    {
        //playerSetup();
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(playersReady)
        {
            CalculateTimer();
            if (timer > 0)
            {
                GetAndEvaluatePlayerScales();
                //GetAndEvaluateCurrentScores();
                //spawn parcels
            }
        }
        else
        {
            playerSetup();
        }

        
    }

    void CalculateTimer()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(timer);
            timerUI.text = "Timer is: " + timerAsInt;
        }
        else
        {
            timerUI.text = "Time is up mother lickers!";

            parcel_man.DestroyParcels("Parcel");

            if (roundScoreCalculated==false)
            {
                GetAndEvaluateFinalScores();
                UpdateTotalScores();
            }
            
            CalculateEndRoundTimer();
        }
    }

    void playerSetup()
    {
        //wait for players to become active by A press
        //when a player press A spawn the corresponding player object at the right player spawn point
        //wait for all players to press X?
        //once all players are ready call setup game
        if (Input.GetButtonDown("GrabDrop_P" + 1) && player1 == null)
        {
            player1 = Object.Instantiate(playerObject, player1SpawnLocation.transform.position, player1SpawnLocation.transform.rotation);
            player1.GetComponent<PlayerController>().PlayerIndex = 1;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 2) && player2 == null)
        {
            player2 = Object.Instantiate(playerObject, player2SpawnLocation.transform.position, player2SpawnLocation.transform.rotation);
            player2.GetComponent<PlayerController>().PlayerIndex = 2;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 3) && player3 == null)
        {
            player3 = Object.Instantiate(playerObject, player3SpawnLocation.transform.position, player3SpawnLocation.transform.rotation);
            player3.GetComponent<PlayerController>().PlayerIndex = 3;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 4) && player4 == null)
        {
            player4 = Object.Instantiate(playerObject, player4SpawnLocation.transform.position, player4SpawnLocation.transform.rotation);
            player4.GetComponent<PlayerController>().PlayerIndex = 4;
        }


        if (Input.GetButtonDown("Throw_P" + 1) && player1 == null && readyPlayer1 == false)
        {
            readyPlayer1 = true;
        }
        if (Input.GetButtonDown("Throw_P" + 2) && player2 == null && readyPlayer2 == false)
        {
            readyPlayer2 = true;
        }
        if (Input.GetButtonDown("Throw_P" + 3) && player3 == null && readyPlayer3 == false)
        {
            readyPlayer3 = true;
        }
        if (Input.GetButtonDown("Throw_P" + 4) && player4 == null && readyPlayer4 == false)
        {
            readyPlayer4 = true;
        }




        waitingForPlayersUI();
    }

    void waitingForPlayersUI()
    {
        //change timer to be waiting for players
        Color waitingCol = new Color(1f, 0f, 0f, 1f);
        timerUI.color = waitingCol;
        timerUI.text = "Waiting for players to be ready";
        //change objective to be "press A to join and X to ready up"
        objectiveUI.text = "Press A to join and X to ready up";
        //change scores to be blank then switch to joined then ready for each player
    }

    void CalculateEndRoundTimer()
    {
        if (newRoundTimer >= 0)
        {
            newRoundTimer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(newRoundTimer);
            Color countdownCol = new Color(1f, 0f, 0f, 1f);
            timerUI.color = countdownCol;
            timerUI.text = "New Round starts in: " + timerAsInt+"s";
        }
        else
        {
            timerUI.text = "START!";
            //ActivateAllPlayers();
            ClearGame();
            SetupGame();
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
        timer = timerFrozen;
        //ClearAllPlayerWeights();
        roundScoreCalculated = false;
        closestScoreValue = 1;
    }

    void SetupGame()
    {
        //set timers
        timer = timerFrozen;
        newRoundTimer = 5;
        Color countdownCol = new Color(0f, 0f, 0f, 1f);
        timerUI.color = countdownCol;

        //generate and set objectives
        objectiveWeightFrozen = Mathf.RoundToInt(Random.Range(1, 6));
        objectiveWeight = objectiveWeightFrozen;
        currentObjectiveWeight = objectiveWeightFrozen;
        objectiveUI.text = "GOAL: " + objectiveWeightFrozen + "KG";
        //get all active players?
        GetPlayersList();
        numberOfPlayers = currentPlayers.Count;
        //set player spawns
        //set item spawns

        // Setup parcel spawners here
        parcel_man.BeginParcelSpawning();

        UpdateTotalScores();
    }

    void GetAndEvaluateFinalScores()
    {
        int playerScore;
        perfectPlayerScores = new List<ScaleZone> { };
        foreach (ScaleZone playerScale in currentPlayers)
        {
            playerScore = playerScale.CurrentWeight;
            playerScore = objectiveWeight - playerScore;
            if (playerScore < 0)
            {
                playerScore = playerScore * -1;
            }
            if (playerScore == 0)
            {
                perfectPlayerScores.Add(playerScale);
            }
            Debug.Log("Score: " + playerScore);
            playerScale.SetScore(playerScore);
            //Debug.Log("Bedson Test: " + playerScore);
        }
        if (perfectPlayerScores.Count > 0)
        {
            foreach (ScaleZone playerScale in perfectPlayerScores)
            {
                if (objectiveWeight == objectiveWeightFrozen)
                {
                    Color winCol = new Color(0f, 1f, 0f, 1f);
                    playerScale.SetEvaluationTextColour(winCol);
                    playerScale.SetEvaluationText("PERFECT WINNER!!!");
                    if(lastPlayerScoreUpdate != playerScale.PlayerIndex)
                    {
                        lastPlayerScoreUpdate = playerScale.PlayerIndex;
                        //roundScoreCalculated = true;
                        playerScale.AddWins(1);
                    }
                    
                    Debug.Log("Bedson TestX: " + playerScale.GetWinCount());
                }
                else
                {
                    Color neutCol = new Color(1f, 1f, 1f, 1f);
                    playerScale.SetEvaluationTextColour(neutCol);
                    playerScale.SetEvaluationText("WINNER!!!");
                    if (lastPlayerScoreUpdate != playerScale.PlayerIndex)
                    {
                        lastPlayerScoreUpdate = playerScale.PlayerIndex;
                        //roundScoreCalculated = true;
                        playerScale.AddWins(1);
                    }
                    Debug.Log("Bedson Test: " + playerScale.GetWinCount() + " objective weight: "+objectiveWeight+" roundScoreCalc: "+roundScoreCalculated);
                }

            }
            roundScoreCalculated = true;
        }
        else
        {
            //no perfect score so we get the closest
            //objectiveWeight--;
            CalculateNearestScore();
        }
    }

    void CalculateNearestScore()
    {
        int playerScore;
        closestPlayerScores = new List<ScaleZone> { };
        foreach (ScaleZone playerScale in currentPlayers)
        {
            playerScore = playerScale.CurrentWeight;
            playerScore = objectiveWeight - playerScore;
            if (playerScore < 0)
            {
                //playerScore = playerScore * -1;
                overweightPlayerScores.Add(playerScale);
            }
            if (playerScore == closestScoreValue)
            {
                closestPlayerScores.Add(playerScale);
            }
            //Debug.Log("Score: " + playerScore);
            playerScale.SetScore(playerScore);
            //Debug.Log("Bedson Test: " + playerScore);
        }


        if (closestPlayerScores.Count > 0)
        {
            foreach (ScaleZone playerScale in closestPlayerScores)
            {
                Color neutCol = new Color(1f, 1f, 1f, 1f);
                playerScale.SetEvaluationTextColour(neutCol);
                playerScale.SetEvaluationText("WINNER!!!");
                if (lastPlayerScoreUpdate != playerScale.PlayerIndex)
                {
                    lastPlayerScoreUpdate = playerScale.PlayerIndex;
                    //roundScoreCalculated = true;
                    playerScale.AddWins(1);
                }
                //Debug.Log("Bedson Test: " + playerScale.GetWinCount() + " objective weight: " + objectiveWeight + " roundScoreCalc: " + roundScoreCalculated);

            }
            roundScoreCalculated = true;
        }
        else if(overweightPlayerScores.Count >= numberOfPlayers)
        {
            foreach (ScaleZone playerScale in overweightPlayerScores)
            {
                Color overCol = new Color(1f, 0f, 1f, 1f);
                playerScale.SetEvaluationTextColour(overCol);
                playerScale.SetEvaluationText("ALL OVERWEIGHT!");
            }
        }
        else
        {
            //no perfect score so we get the closest
            closestScoreValue++;
            CalculateNearestScore();
        }

    }

    //void GetAndEvaluateCurrentScores()
    //{
    //    Debug.Log("Bedson Test1: " + currentObjectiveWeight);
    //    int playerScore;
    //    currentPerfectPlayerScores = new List<ScaleZone> { };
    //    foreach (ScaleZone playerScale in currentPlayers)
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
    //        foreach (ScaleZone playerScale in currentPerfectPlayerScores)
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


    void UpdateTotalScores()
    {
        Debug.Log("Here we are!");
        //for each current player
        foreach (ScaleZone playerScale in currentPlayers)
        {
            int playerNum = playerScale.PlayerIndex;
            if(playerNum == 1)
            {
                player1WinCount = playerScale.GetWinCount();
            }else if (playerNum == 2)
            {
                player2WinCount = playerScale.GetWinCount();
            }
            else if (playerNum == 3)
            {
                player3WinCount = playerScale.GetWinCount();
            }
            else if (playerNum == 4)
            {
                player4WinCount = playerScale.GetWinCount();
            }
        }
        UpdateTotalScoreText();
    }

    void UpdateTotalScoreText()
    {
        string scoreText = "Score ";
        player1WinUI.text = scoreText + player1WinCount;
        player2WinUI.text = scoreText + player2WinCount;
        player3WinUI.text = scoreText + player3WinCount;
        player4WinUI.text = scoreText + player4WinCount;
    }

    void GetAndEvaluatePlayerScales()
    {
        //for each player get the current score 
        foreach(ScaleZone playerScale in currentPlayers)
        {
            int weight = playerScale.CurrentWeight;
            EvaluatePlayerScore(weight, playerScale);
        }
    }

    //void DeactivateAllPlayers()
    //{
    //    foreach (ScaleZone playerScale in currentPlayers)
    //    {
    //        playerScale.Deactivate();
    //    }
    //}

    //void ActivateAllPlayers()
    //{
    //    foreach (ScaleZone playerScale in currentPlayers)
    //    {
    //        playerScale.Activate();
    //    }
    //}

    //void ClearAllPlayerWeights()
    //{
    //    foreach (ScaleZone playerScale in currentPlayers)
    //    {
    //        playerScale.SetWeight(0);
    //    }
    //}

    void UpdatePlayerWins()
    {
        foreach (ScaleZone playerScale in currentPlayers)
        {
            playerScale.GetWinCount();
        }
    }


    void EvaluatePlayerScore(int playerScore, ScaleZone playerScale)
    {
        if (playerScore == objectiveWeight)
        {
            Color winCol = new Color(0f, 0.8f, 0.1f, 1f);
            playerScale.SetEvaluationTextColour(winCol);
            playerScale.SetEvaluationText("PERFECT WEIGHT!");
        }
        else if (playerScore > objectiveWeight)
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


    //void AssignPlayerNumbers()
    //{
    //    int num = 1;
    //    foreach (ScaleZone playerScale in currentPlayers)
    //    {
    //        playerScale.SetPlayerNumber(num);
    //        Debug.Log("Assigned: "+playerScale.name+" the number of "+num);
    //        num++;
    //    }
    //}

    void GetPlayersList()
    {
        if(playersSet == false)
        {
            playersSet = true;
            if (player1Scale != null)
            {
                currentPlayers.Add(player1Scale);
                //player1Scale.SetPlayerNumber(1);
            }
            if (player2Scale != null)
            {
                currentPlayers.Add(player2Scale);
                //player2Scale.SetPlayerNumber(2);
            }
            if (player3Scale != null)
            {
                currentPlayers.Add(player3Scale);
                //player3Scale.SetPlayerNumber(3);
            }
            if (player4Scale != null)
            {
                currentPlayers.Add(player4Scale);
                //player4Scale.SetPlayerNumber(4);
            }
        }
    }
}
