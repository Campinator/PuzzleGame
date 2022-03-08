/**
 * Date Created: March 7. 2022
 * Created By: Camp Steiner
 * 
 * Date Modified: March 7, 2022
 * Modified by: Camp Steiner
 * 
 * Description: Simple tutorial that guides the user through the game mechanics
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int step = 0;
    public Text tutorialText;
    public Button tutorialButton;
    public Text tutorialButtonText;
    public GameObject goalArrow;
    public GameObject canvasHUD;
    public GameObject viewArrow;
    public GameObject buildArea;
    public GameObject fakeBuildArea;
    public GameObject blocksArrow;
    GameManager gm; //reference to game manager

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
        FollowCam.POI = GameObject.Find("ViewBoth");
    }

    // Update is called once per frame
    public void Skip()
    {
        gm.NextLevel();
    }
    public void AdvanceTutorial(){
        switch(++step){
            case 1:
                GameObject.Find("SkipButton").SetActive(false);
                FollowCam.POI = GameObject.FindGameObjectWithTag("Slingshot");
                tutorialText.text = "This is the mysterious <color=\"#8b0000\">Slingshot</color> that is plaguing the kingdom.\nIt appeared out of nowhere, and began launching rocks at our castle.";
                tutorialButtonText.text = "Oh No!";
            break;
            case 2:
                FollowCam.POI = GameObject.FindGameObjectWithTag("Goal");
                tutorialText.text = "This is our castle. As you can see, it's looking a litle worse for the wear.";
                tutorialButtonText.text = "Next";
            break;
            case 3:
                tutorialText.text = "Your <color=\"#03588C\">Mission</color> is to <color=\"#03588C\">Construct</color> a new castle to protect the heart of the kingdom from being attacked.";
                goalArrow.SetActive(true);
                tutorialButtonText.text = "Got It";
            break;
            case 4:
                goalArrow.SetActive(false);
                tutorialText.text = "Let's take a look at how:";
                tutorialButtonText.text = "Next";
            break;
            case 5:
                canvasHUD.SetActive(true);
                tutorialText.text = "This is the information shown on each level. You can click <color=\"#8b0000\">at the top</color> to change what you are looking at.";
                tutorialButtonText.text = "Next";
                viewArrow.SetActive(true);
            break;
            case 6:
                FollowCam.POI = GameObject.FindGameObjectWithTag("Goal");
                viewArrow.SetActive(false);
                fakeBuildArea.SetActive(true);
                tutorialText.text = "The <color=\"#03588C\">blue</color> area behind this message is where you can build your castle. The pictures on the right represent the blocks you have to work with.";
                tutorialButtonText.text = "Next";
                blocksArrow.SetActive(true);
            break;
            case 7:
                blocksArrow.SetActive(false);
                tutorialText.text = "Click to place a block and build a castle. You can click on other block types to switch between construction materials.";
                tutorialButtonText.text = "Got it";
            break;
            case 8:
                blocksArrow.SetActive(false);
                tutorialText.text = "When you are done, press the green button in the left to test your castle against the slingshot. Good luck!";
                tutorialButtonText.text = "I'm Ready";
            break;
            case 9:
                fakeBuildArea.SetActive(false);
                buildArea.SetActive(true);
                GameObject.Find("CanvasTutorial").SetActive(false);
            break;
        }
    }
}
