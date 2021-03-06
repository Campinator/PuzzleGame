/**** 
 * Created by: Akram Taghavi-Burrs
 * Date Created: Feb 23, 2022
 * 
 * Last Edited by: Camp Steiner
 * Last Edited: March 7, 2022
 * 
 * Description: Basic GameManager Template
****/

/** Import Libraries **/
using System; //C# library for system properties
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //libraries for accessing scenes


public class GameManager : MonoBehaviour
{
    /*** VARIABLES ***/

    #region GameManager Singleton
    static private GameManager gm; //refence GameManager
    static public GameManager GM { get { return gm; } } //public access to read only gm 

    //Check to make sure only one gm of the GameManager is in the scene
    void CheckGameManagerIsInScene()
    {

        //Check if instnace is null
        if (gm == null)
        {
            gm = this; //set gm to this gm of the game object
            Debug.Log(gm);
        }
        else //else if gm is not null a Game Manager must already exsist
        {
            Destroy(this.gameObject); //In this case you need to delete this gm
        }
        DontDestroyOnLoad(this); //Do not delete the GameManager when scenes load
        Debug.Log(gm);
    }//end CheckGameManagerIsInScene()
    #endregion

    [Header("GENERAL SETTINGS")]
    public string gameTitle = "Mission Construction";  //name of the game
    public string gameCredits = "Camp Steiner"; //game creator(s)
    public string copyrightDate = "Copyright " + thisDay; //date cretaed

    [Header("GAME SETTINGS")]

    [Tooltip("Will the high score be recorded")]
    public bool recordHighScore = true; //is the High Score recorded

    [SerializeField] //Access to private variables in editor
    private int defaultHighScore = 1000;
    static public int highScore = 1000; // the default High Score
    public int HighScore { get { return highScore; } set { highScore = value; } }//access to private variable highScore [get/set methods]

    [Space(10)]

    //static variables can not be updated in the inspector, however private serialized fields can be
    [SerializeField] //Access to private variables in editor
    private int numberOfShots; //set number of shots in the inspector
    static public int shots; // number of shots to be fired 
    public int Shots { get { return shots; } set { shots = value; } }//access to private variable died [get/set methods]

    static public int score;  //score value
    public int Score { get { return score; } set { score = value; } }//access to private variable died [get/set methods]
    static public int totScore;  //score value
    public int TotScore { get { return totScore; } set { totScore = value; } }//access to private variable died [get/set methods]

    [SerializeField] //Access to private variables in editor
    [Tooltip("Check to test player lost the level")]
    private bool levelLost = false;//we have lost the level (ie. player died)
    public bool LevelLost { get { return levelLost; } set { levelLost = value; } } //access to private variable lostLevel [get/set methods]

    [Space(10)]
    public string defaultEndMessage = "Game Over";//the end screen message, depends on winning outcome
    [TextArea]
    public string loseMessage = "Your Castle was destroyed!\nTry again?"; //Message if player loses
    [TextArea]
    public string winMessage = "You Win! Total Score: " + totScore; //Message if player wins
    [HideInInspector] public string endMsg;//the end screen message, depends on winning outcome

    [Header("SCENE SETTINGS")]
    [Tooltip("Name of the start scene")]
    public string startScene;

    [Tooltip("Name of the game over scene")]
    public string gameOverScene;

    [Tooltip("Count and name of each Game Level (scene)")]
    public string[] gameLevels; //names of levels
    [HideInInspector]
    public int gameLevelsCount; //what level we are on
    private int loadLevel; //what level from the array to load

    public static string currentSceneName; //the current scene name;

    [Header("FOR TESTING")]
    public bool nextLevel = false; //test for next level

    //Game State Varaiables
    [HideInInspector] public enum gameStates { Idle, Playing, Death, GameOver, BeatLevel };//enum of game states
    [HideInInspector] public gameStates gameState = gameStates.Idle;//current game state

    //Timer Varaibles
    private float currentTime; //sets current time for timer
    private bool gameStarted = false; //test if games has started

    //Win/Loose conditon
    [SerializeField] //to test in inspector
    private bool playerWon = true;

    //reference to system time
    private static string thisDay = System.DateTime.Now.ToString("yyyy"); //today's date as string


    /*** MEHTODS ***/

    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        //runs the method to check for the GameManager
        CheckGameManagerIsInScene();

        //store the current scene
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //Get the saved high score
        GetHighScore();

    }//end Awake()


    // Update is called once per frame
    private void Update()
    {
        //if ESC is pressed , exit game
        if (Input.GetKeyDown(KeyCode.Escape)) { ExitGame(); }

        //Check for next level
        if (nextLevel) { NextLevel(); }

        //if we are playing the game
        if (gameState == gameStates.Playing)
        {
            //if we have lost the level, go to game over
            //if we have survived all the shots, go on
            if(shots == 0) { NextLevel(); }
            if (levelLost) { playerWon = false; GameOver(); }

        }//end if (gameState == gameStates.Playing)

    }//end Update


    //LOAD THE GAME FOR THE FIRST TIME OR RESTART
    public void StartGame()
    {
        //SET ALL GAME LEVEL VARIABLES FOR START OF GAME

        gameLevelsCount = 1; //set the count for the game levels
        loadLevel = gameLevelsCount - 1; //the level from the array
        SceneManager.LoadScene(gameLevels[loadLevel]); //load first game level

        gameState = gameStates.Playing; //set the game state to playing

        shots = numberOfShots; //set the number of shots
        score = 0; //set starting score
        GetHighScore();

        endMsg = defaultEndMessage; //set the end message default

        playerWon = true; //set player winning condition to false
        levelLost = false;
    }//end StartGame()



    //EXIT THE GAME
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exited Game");
    }//end ExitGame()


    //GO TO THE GAME OVER SCENE
    public void GameOver()
    {
        gameState = gameStates.GameOver; //set the game state to gameOver

        if (playerWon) { endMsg = winMessage; CheckScore(); } else { endMsg = loseMessage; } //set the end message

        SceneManager.LoadScene(gameOverScene); //load the game over scene
        Debug.Log("Gameover");
    }


    //GO TO THE NEXT LEVEL
    public void NextLevel()
    {
        TotScore += Score;
        nextLevel = false; //reset the next level

        //as long as our level count is not more than the amount of levels
        if (gameLevelsCount < gameLevels.Length)
        {
            gameLevelsCount++; //add to level count for next level
            loadLevel = gameLevelsCount - 1; //find the next level in the array
            shots = numberOfShots;
            SceneManager.LoadScene(gameLevels[loadLevel]); //load next level

        }
        else
        { //if we have run out of levels go to game over
            GameOver();
        } //end if (gameLevelsCount <=  gameLevels.Length)

    }//end NextLevel()

    void CheckScore()
    { //This method manages the score on update. Right now it just checks if we are greater than the high score.

        //if the score is more than the high score
        if (score > highScore)
        {
            highScore = score; //set the high score to the current score
            PlayerPrefs.SetInt("HighScore", highScore); //set the playerPref for the high score
        }//end if(score > highScore)

    }//end CheckScore()

    void GetHighScore()
    {//Get the saved highscore

        //if the PlayerPref alredy exists for the high score
        if (PlayerPrefs.HasKey("HighScore"))
        {
            Debug.Log("Has Key");
            highScore = PlayerPrefs.GetInt("HighScore"); //set the high score to the saved high score
        }//end if (PlayerPrefs.HasKey("HighScore"))

        PlayerPrefs.SetInt("HighScore", highScore); //set the playerPref for the high score
    }//end GetHighScore()

}
