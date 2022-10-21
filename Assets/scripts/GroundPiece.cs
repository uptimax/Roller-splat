using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    private int score = 1000;
    public bool isColored = false;

    public void ChangeColor(Color color){
        GameManager gameManager = GameManager.singleton; 
        ++gameManager.scoreMultiplier;
        int multiplier = (gameManager.scoreMultiplier <= 0)? 1 : gameManager.scoreMultiplier;
        gameManager.score = (multiplier * score) + gameManager.score;
        gameManager.UpdateScoreText();
        GetComponent<MeshRenderer> ().material.color = color;
        isColored = true;
        GameManager.singleton.CheckComplete();
    }

}
