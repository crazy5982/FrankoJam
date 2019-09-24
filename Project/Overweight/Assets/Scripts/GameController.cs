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
    [SerializeField] private GameObject player1Object;
    [SerializeField] private GameObject player2Object;
    [SerializeField] private GameObject player3Object;
    [SerializeField] private GameObject player4Object;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    [SerializeField] private GameObject player1SpawnLocation;
    [SerializeField] private GameObject player2SpawnLocation;
    [SerializeField] private GameObject player3SpawnLocation;
    [SerializeField] private GameObject player4SpawnLocation;

    private int numberOfPlayers = 0;
    [SerializeField] private int minPlayerCount = 2;
    private int zeroWeightCount = 0;


    private float startGameTimer = 5;
	[SerializeField] private float timerFrozen = 15;

    private float timer;
    private float newRoundTimer = 5;
    private int timerAsInt;
    private bool roundScoreCalculated = false;
    private bool playersSet = false;
    private int lastPlayerScoreUpdate=0;
    private int closestScoreValue = 1;
    [SerializeField] private int maxObjective = 15;
    [SerializeField] private int minObjective = 3;

    private bool playersReady = false;
    private bool gameStarting = false;
    private bool readyPlayer1 = false;
    private bool readyPlayer2 = false;
    private bool readyPlayer3 = false;
    private bool readyPlayer4 = false;
    private int readyCount = 0;

    private string player1State = "A to join";
    private string player2State = "A to join";
    private string player3State = "A to join";
    private string player4State = "A to join";

    public Text timerUI;
    public Text objectiveUILeft;
    public Text objectiveUIRight;

    public Text player1WinUI;
    public Text player2WinUI;
    public Text player3WinUI;
    public Text player4WinUI;

    public Canvas objectiveCanvasLeft;
    public Canvas objectiveCanvasRight;
    public Canvas timerCanvas;
    public Canvas winnerCanvas;
    public Canvas scoresCanvas;

    private int objectiveWeightFrozen = 5;
    private int objectiveWeight;
    private int currentObjectiveWeight;

    [SerializeField] private int winsObjective = 7;
    private bool gameWon = false;
    public Text winnerText;

    private int player1Score;
    private int player2Score;
    private int player3Score;
    private int player4Score;

    private int player1WinCount;
    private int player2WinCount;
    private int player3WinCount;
    private int player4WinCount;

    private List<ScaleZone> currentZones = new List<ScaleZone>{};
    private List<GameObject> currentPlayers = new List<GameObject>{};
    private List<ScaleZone> perfectPlayerScores = new List<ScaleZone> {};
    private List<ScaleZone> closestPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> overweightPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> currentPerfectPlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> PlayerScores = new List<ScaleZone> { };
    private List<ScaleZone> winners = new List<ScaleZone> { };
    private List<ScaleZone> zeroWeight = new List<ScaleZone> { };

    // Parcel Spawner game object reference
    [SerializeField] private GameObject parcel_spawner_object;

    // Parcel destruction setup
    parcel_manager parcel_man = new parcel_manager();

    // Winner delay timer setup
    [SerializeField] private float winnerDelay = 3f;

    [SerializeField] protected AudioClip m_PerfectClip;
	[SerializeField] protected AudioClip m_OverweightClip;
	protected AudioSource m_AudioSource;

    void Start()
    {
        playerSetup();
        winnerText.text ="";
        winnerCanvas.enabled = false;
		//SetupGame();

		m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStarting)
        {
            if(!gameWon)
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
                //GAME IS WON!
                EndOfGameWait();
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
            player1 = Object.Instantiate(player1Object, player1SpawnLocation.transform.position, player1SpawnLocation.transform.rotation);
            player1.GetComponent<PlayerController>().PlayerIndex = 1;
            currentPlayers.Add(player1);
            currentZones.Add(player1Scale);
            player1Scale.Enabled = true;
            player1State = "X to ready";
            numberOfPlayers++;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 2) && player2 == null)
        {
            player2 = Object.Instantiate(player2Object, player2SpawnLocation.transform.position, player2SpawnLocation.transform.rotation);
            player2.GetComponent<PlayerController>().PlayerIndex = 2;
            currentPlayers.Add(player2);
            currentZones.Add(player2Scale);
            player2Scale.Enabled = true;
            player2State = "X to ready";
            numberOfPlayers++;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 3) && player3 == null)
        {
            player3 = Object.Instantiate(player3Object, player3SpawnLocation.transform.position, player3SpawnLocation.transform.rotation);
            player3.GetComponent<PlayerController>().PlayerIndex = 3;
            currentPlayers.Add(player3);
            currentZones.Add(player3Scale);
            player3Scale.Enabled = true;
            player3State = "X to ready";
            numberOfPlayers++;
        }
        if (Input.GetButtonDown("GrabDrop_P" + 4) && player4 == null)
        {
            player4 = Object.Instantiate(player4Object, player4SpawnLocation.transform.position, player4SpawnLocation.transform.rotation);
            player4.GetComponent<PlayerController>().PlayerIndex = 4;
            currentPlayers.Add(player4);
            currentZones.Add(player4Scale);
            player4Scale.Enabled = true;
            player4State = "X to ready";
            numberOfPlayers++;
        }

        if (Input.GetButtonDown("Throw_P" + 1) && player1 != null && readyPlayer1 == false)
        {
            readyPlayer1 = true;
            player1.GetComponent<PlayerController>().ready = true;
            player1State = "Ready";
            readyCount++;
        }
        if (Input.GetButtonDown("Throw_P" + 2) && player2 != null && readyPlayer2 == false)
        {
            readyPlayer2 = true;
            player2.GetComponent<PlayerController>().ready = true;
            player2State = "Ready";
            readyCount++;
        }
        if (Input.GetButtonDown("Throw_P" + 3) && player3 != null && readyPlayer3 == false)
        {
            readyPlayer3 = true;
            player3.GetComponent<PlayerController>().ready = true;
            player3State = "Ready";
            readyCount++;
        }
        if (Input.GetButtonDown("Throw_P" + 4) && player4 != null && readyPlayer4 == false)
        {
            readyPlayer4 = true;
            player4.GetComponent<PlayerController>().ready = true;
            player4State = "Ready";
            readyCount++;
        }

        if(numberOfPlayers >= minPlayerCount && (readyCount == numberOfPlayers))
        {
            playersReady = true;
        }
        //else if(numberOfPlayers > 0 && (readyCount != numberOfPlayers))
        //{
        //    playersReady = false;
        //}
        waitingForPlayersUI();
    }

    void waitingForPlayersUI()
    {
        if(!playersReady)
        {
            //change timer to be waiting for players
            Color waitingCol = new Color(1f, 1f, 1f, 1f);
            timerUI.color = waitingCol;
            timerUI.text = "Ready up!";
            //change objective to be "press A to join and X to ready up"
            objectiveUILeft.text = "GOAL:";
            objectiveUIRight.text = "GOAL:     ";
            //change scores to be blank then switch to joined then ready for each player
            player1WinUI.text = "" + player1State;
            player2WinUI.text = "" + player2State;
            player3WinUI.text = "" + player3State;
            player4WinUI.text = "" + player4State;
        }
        else
        {
            //do some timer stuff here
            if (startGameTimer >= 0)
            {
                startGameTimer -= Time.deltaTime;
                timerAsInt = Mathf.RoundToInt(startGameTimer);
                Color countdownCol = new Color(1f, 1f, 1f, 1f);
                timerUI.color = countdownCol;
                timerUI.text = "Game starts in: " + timerAsInt + "s";
                player1WinUI.text = "" + player1State;
                player2WinUI.text = "" + player2State;
                player3WinUI.text = "" + player3State;
                player4WinUI.text = "" + player4State;
            }
            else
            {
                timerUI.text = "START!";
                //ActivateAllPlayers();
                ClearGame();
                SetupGame();
            }
        }

    }

    void CalculateEndRoundTimer()
    {
        if (newRoundTimer >= 0)
        {
            newRoundTimer -= Time.deltaTime;
            timerAsInt = Mathf.RoundToInt(newRoundTimer);
            Color countdownCol = new Color(1f, 1f, 1f, 1f);
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
		lastPlayerScoreUpdate = 0;
        zeroWeightCount = 0;
        zeroWeight = new List<ScaleZone> { };

        player1Scale.ResetScale();
		player2Scale.ResetScale();
		player3Scale.ResetScale();
		player4Scale.ResetScale();
	}

    void SetupGame()
    {
        //set timers
        gameStarting = true;
        timer = timerFrozen;
        newRoundTimer = 5;
        Color countdownCol = new Color(1f, 1f, 1f, 1f);
        timerUI.color = countdownCol;

        //generate and set objectives
        objectiveWeightFrozen = Mathf.RoundToInt(Random.Range(minObjective, maxObjective));
        objectiveWeight = objectiveWeightFrozen;
        currentObjectiveWeight = objectiveWeightFrozen;
        objectiveUILeft.text = "GOAL: " + objectiveWeightFrozen + " KG";
        objectiveUIRight.text = "GOAL: " + objectiveWeightFrozen + " KG";
        //get all active players?
        foreach (ScaleZone playerScale in currentZones)
        {
            Debug.Log("Player "+playerScale.name);
        }
        //GetPlayersList();
        //numberOfPlayers = currentZones.Count;
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
        foreach (ScaleZone playerScale in currentZones)
        {
            playerScore = playerScale.CurrentWeight;
            playerScore = objectiveWeight - playerScore;
            //if (playerScore < 0)
            //{
            //    playerScore = playerScore * -1;
            //}
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
        overweightPlayerScores = new List<ScaleZone> { };
        foreach (ScaleZone playerScale in currentZones)
        {
			if (playerScale.CurrentWeight > 0)
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
            else
            {
                if (zeroWeight.Contains(playerScale) == false)
                {
                    zeroWeightCount++;
                    zeroWeight.Add(playerScale);
                }
                
            }
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
            if (zeroWeightCount >= numberOfPlayers)
            {
                //all players have zero weight, this is a win for no one
                foreach (ScaleZone playerScale in currentZones)
                {
                    Color overCol = new Color(1f, 0f, 1f, 1f);
                    playerScale.SetEvaluationTextColour(overCol);
                    playerScale.SetEvaluationText("ALL ZEROWEIGHT!");
                }
                    
            }
            // If the closestScoreValue becomes > objectiveWeight any remaining players must have no weight,
            // so no-one gets any more points, and we break out of this loop
            else if (closestScoreValue < objectiveWeight)
            {
                closestScoreValue++;
                CalculateNearestScore();
            }
            
        }

    }

    //void GetAndEvaluateCurrentScores()
    //{
    //    Debug.Log("Bedson Test1: " + currentObjectiveWeight);
    //    int playerScore;
    //    currentPerfectPlayerScores = new List<ScaleZone> { };
    //    foreach (ScaleZone playerScale in currentZones)
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

    void EndOfGameWait()
    {
        //wait for the players to do something
        StartCoroutine(NextScene(winnerDelay));        
    }
    void UpdateTotalScores()
    {
        //for each current player
        foreach (ScaleZone playerScale in currentZones)
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
        CheckForWinner();
    }
    void CheckForWinner()
    {
        foreach (ScaleZone playerScale in currentZones)
        {
            if (playerScale.GetWinCount() >= winsObjective)
            {
                winners.Add(playerScale);
            }
        }

        if (winners.Count > 0)
        {
            //set a winner found variable to prevent other things from happening
            gameWon = true;
            string winnerNames = "WINNER!: ";
            foreach (ScaleZone playerScale in winners)
            {
                string scaleS = playerScale.name;
                string playerN = scaleS.Replace("ScaleZone_", "");
                Debug.Log("Your winner: "+playerScale.name);
                winnerNames = winnerNames + " Player " + playerN;
            }
            //do some winning function
            winnerCanvas.enabled = true;
            winnerText.text = winnerNames;
        }
    }

    void HideAllUI()
    {
        objectiveCanvasLeft.enabled = false;
        objectiveCanvasRight.enabled = false;
        timerCanvas.enabled = false;
        winnerCanvas.enabled = false;
        scoresCanvas.enabled = false;
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
        foreach(ScaleZone playerScale in currentZones)
        {
			if (playerScale.WeightChangedSinceLastChecked)
			{
				int weight = playerScale.CurrentWeight;
				EvaluatePlayerScore(weight, playerScale);
			}
        }
    }

    //void DeactivateAllPlayers()
    //{
    //    foreach (ScaleZone playerScale in currentZones)
    //    {
    //        playerScale.Deactivate();
    //    }
    //}

    //void ActivateAllPlayers()
    //{
    //    foreach (ScaleZone playerScale in currentZones)
    //    {
    //        playerScale.Activate();
    //    }
    //}

    //void ClearAllPlayerWeights()
    //{
    //    foreach (ScaleZone playerScale in currentZones)
    //    {
    //        playerScale.SetWeight(0);
    //    }
    //}

    void UpdatePlayerWins()
    {
        foreach (ScaleZone playerScale in currentZones)
        {
            playerScale.GetWinCount();
        }
    }


    void EvaluatePlayerScore(int playerScore, ScaleZone playerScale)
    {
		AudioClip clipToUse = null;

        if (playerScore == objectiveWeight)
        {
            Color winCol = new Color(0f, 0.8f, 0.1f, 1f);
            playerScale.SetEvaluationTextColour(winCol);
            playerScale.SetEvaluationText("PERFECT WEIGHT!");

			clipToUse = m_PerfectClip;
        }
        else if (playerScore > objectiveWeight)
        {
            Color overCol = new Color(1f, 0f, 1f, 1f);
            playerScale.SetEvaluationTextColour(overCol);
            playerScale.SetEvaluationText("OVERWEIGHT!");

			clipToUse = m_OverweightClip;
        }
        else if(playerScore < objectiveWeight)
        {
            Color neutCol = new Color(1f, 1f, 1f, 1f);
            playerScale.SetEvaluationTextColour(neutCol);
            playerScale.SetEvaluationText("");
        }

		if (clipToUse != null && m_AudioSource != null)
		{
			m_AudioSource.clip = clipToUse;
			m_AudioSource.Play();
		}
    }


    //void AssignPlayerNumbers()
    //{
    //    int num = 1;
    //    foreach (ScaleZone playerScale in currentZones)
    //    {
    //        playerScale.SetPlayerNumber(num);
    //        Debug.Log("Assigned: "+playerScale.name+" the number of "+num);
    //        num++;
    //    }
    //}

    //void GetPlayersList()
    //{
    //    if(playersSet == false)
    //    {
    //        playersSet = true;
    //        if (player1Scale != null)
    //        {
    //            currentZones.Add(player1Scale);
    //            //player1Scale.SetPlayerNumber(1);
    //        }
    //        if (player2Scale != null)
    //        {
    //            currentZones.Add(player2Scale);
    //            //player2Scale.SetPlayerNumber(2);
    //        }
    //        if (player3Scale != null)
    //        {
    //            currentZones.Add(player3Scale);
    //            //player3Scale.SetPlayerNumber(3);
    //        }
    //        if (player4Scale != null)
    //        {
    //            currentZones.Add(player4Scale);
    //            //player4Scale.SetPlayerNumber(4);
    //        }
    //    }
    //}

    IEnumerator NextScene(float winnerDelay)
    {
        yield return new WaitForSeconds(winnerDelay);
        if (Input.GetButtonDown("GrabDrop_P" + 1) || Input.GetButtonDown("GrabDrop_P" + 2) || Input.GetButtonDown("GrabDrop_P" + 3) || Input.GetButtonDown("GrabDrop_P" + 4))
        {
			if (gameObject.scene.buildIndex == 3)
			{
				SceneLoader.LoadStartScene();
			}
			else
			{
				SceneLoader.LoadNextScene();
			}
        }
        else if (Input.GetButtonDown("Throw_P" + 1) || Input.GetButtonDown("Throw_P" + 2) || Input.GetButtonDown("Throw_P" + 3) || Input.GetButtonDown("Throw_P" + 4))
        {
            SceneLoader.LoadStartScene();
        }
    }
}
