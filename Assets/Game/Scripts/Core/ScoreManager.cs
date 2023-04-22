using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Inspector
    public TextMeshProUGUI ScoreText;
    public int BlockScore;
    
    private int _score=0;
    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            ScoreText.text = "Score: 0";
        }
    }

    private void UpdateScore()
    {
        ScoreText.text = "Score: "+_score;
    }

    public void BlockAddedToBoard()
    {
        _score += BlockScore;
        UpdateScore();
    }

    public void LineCleared(int lineNum)
    {
        switch (lineNum)
        {
            case 0:
                break;
            case 1:
                _score += 40;
                break;
            case 2:
                _score += 100;
                break;
            case 3:
                _score += 300;
                break;
            case 4 or _:
                _score += 1200;
                break;
        }
        UpdateScore();
    }
    
    
}
