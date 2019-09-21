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
    private int objectiveWeight;
    public bool active = true;
    private int score;
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

    public void SetWeightObjective(int objective)
    {
        objectiveWeight = objective;
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

}
