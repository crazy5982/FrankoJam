using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneControllerTemp : MonoBehaviour
{
    // Start is called before the first frame update
    private int weight = 0;
    private TextMesh scaleText;
    private GameObject scaleTextHolder;
    private GameObject evaluationTextHolder;
    private TextMesh evaluationText;
    public GameController gameController;
    public bool active = true;
    private int score;
    private int winCount = 0;
    private int playerNumber;
    void Start()
    {
        scaleTextHolder = this.gameObject.transform.GetChild(0).gameObject;
        scaleText = scaleTextHolder.GetComponent<TextMesh>();
        evaluationTextHolder = this.gameObject.transform.GetChild(1).gameObject;
        evaluationText = evaluationTextHolder.GetComponent<TextMesh>();
        evaluationText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            scaleText.text = weight + "kg";
        }
        
    }

    void OnMouseDown()
    {
        if(active)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
            {
                //print("ZoneReverse: " + this.gameObject.name);
                AddWeight(-1);
            }
            else if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
            {
                //print("Zone: " + this.gameObject.name);
                AddWeight(1);
            }
        }

    }

    public int GetWeight()
    {
        return weight;
    }

    public void AddWeight(int addedWeight)
    {
        weight += addedWeight;
    }

    public void SetWeight(int setWeight)
    {
        weight = setWeight;
    }

    public void SetEvaluationText(string text)
    {
        evaluationText.text = text;
    }

    public void SetEvaluationTextColour(UnityEngine.Color colour)
    {
        evaluationText.color = colour;
    }

    public string GetEvaluationText()
    {
       return evaluationText.text;
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public bool GetActive()
    {
        return active;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int scoreVal)
    {
        score = scoreVal;
    }

    public int GetWinCount()
    {
        return winCount;
    }

    public void SetWinCount(int winVal)
    {
        winCount = winVal;
    }

    public void AddWins(int winVal)
    {
        winCount = winCount+winVal;
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    public void SetPlayerNumber(int pNum)
    {
        playerNumber = pNum;
    }


}
