using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private static Score instance;

    public static Score Instance
    {
        get
        {
            
            return instance;
        }
    }

    private TextMeshProUGUI tmp;
    private int currentScore;
    
    public int CurrentScore
    {
        get { return currentScore; }
        set
        { 
            currentScore = value;
            tmp.text = string.Format("{0:D8}", currentScore);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = int.Parse(tmp.text);
    }

    void Update()
    {
        
    }

    public void AddScore(int score)
    {
        this.CurrentScore += score;
    }
}
