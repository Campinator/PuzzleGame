/**** 
 * Created by: Akram Taghavi-Burrs
 * Date Created: Feb 23, 2022
 * 
 * Last Edited by: Camp Steiner
 * Last Edited: March 7, 2022
 * 
 * Description: Updates HUD canvas referecing game manager
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    /*** VARIABLES ***/

    GameManager gm; //reference to game manager

    [Header("Canvas SETTINGS")]
    public Text levelTextbox; //textbox for level count
    public Text shotsTextbox; //textbox for the shots
    public Text scoreTextbox; //textbox for the score
    public Text highScoreTextbox; //textbox for highscore
    public Text viewButtonTextbox; //textbox for change view
    public static Text viewButtonText;
    
    //GM Data
    private int level;
    private int totalLevels;
    private int shots;
    private int score;
    private int highscore;

    private void Start()
    {
        gm = GameManager.GM; //find the game manager
        viewButtonText = this.viewButtonTextbox;
        //reference to levle info
        level = gm.gameLevelsCount;
        totalLevels = gm.gameLevels.Length;

        SetHUD();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        GetGameStats();
        SetHUD();
    }//end Update()

    void GetGameStats()
    {
        shots = gm.Shots;
        score = gm.Score;
        highscore = gm.HighScore;
    }

    void SetHUD()
    {
        string shotsTxt = "";
        for(int i = 0; i < shots; i++){
            shotsTxt += "o";
        }
        //if texbox exsists update value
        if (levelTextbox) { levelTextbox.text = "Level " + level + "/" + totalLevels; }
        if (shotsTextbox) { shotsTextbox.text = "Shots Left " + shotsTxt; }
        if (scoreTextbox) { scoreTextbox.text = "Budget: $ " + score; }
        if (highScoreTextbox) { highScoreTextbox.text = "High Score " + highscore; }

    }//end SetHUD()

}
