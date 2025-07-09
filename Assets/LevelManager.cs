using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public bool gameLevel;
    //����
    public GameObject menuGameObject;
    public GameObject tutorialText1; //*
    public GameObject tutorialText2; //Recomended
    //������ � �����
    public GameObject chatWindowObject;
    public GameObject outroWindowObject;

    public TextMeshProUGUI chatWindow;
    public TextMeshProUGUI outroTextWindow;

    public List<string> chatLines;
    public List<string> endChatLines;

    public int endChatLineNumber;
    public int chatLineNumber;
    public bool chatEnded;
    public GameObject nextButton;
    public GameObject nextButtonSupport;
    public GameObject goButton;
    public GameObject welcomeTextButton;
    bool endScreenShown;
    //��������� ���
    public float timeBeforeShowdown;
    public GameObject RedTeamFighter;
    public GameObject BlueTeamFighter;
    public bool fightersSpawned;
    public List<GameObject> blueFightersSpawns;
    public List<GameObject> redFightersSpawns;

    public float graceTime;
    public GameObject endScreenText;

    public GameObject moveToCutButton;
    public GameObject outroLineButton;
    public GameObject toMapButton;

    public GameObject blueWinsText;
    public GameObject redWinsText;
    public TextMeshProUGUI blueScore;
    public TextMeshProUGUI redScore;

    public float timerBeforeShowingMenu;

    public GameObject agitator;

    public GameObject creditsText;
    public GameObject theCage;
    public string levelNameForRestartButton;

    public GameObject elector;
    public GameObject cutsceneElectorSpawnPoint;
    public bool cutsceneStarted;
    public float timeOfCutscene;
    public bool level1Scenario;
    public bool campAlignmentOn;
    public List<GameObject> finalSpawnPoints;

    public GameObject pressETutorial;
    public Button OneTapButton;

    public string nextLevelName;
    public int nextLevelNumber;
    public GameObject vanGameObj;
    public int isGameStarted;
    private void Awake()
    {
        References.levelManager = this;
    }
    private void Start()
    {
        isGameStarted = 0;
        Debug.Log("Number now " + PlayerPrefs.GetInt("highestLevel", nextLevelNumber));
        /*PlayerPrefs.GetInt("isGameStarted", isGameStarted);
        if(isGameStarted == 0)
        {
            PlayerPrefs.SetInt("isGameStarted", 1);
            ProgressReset();
        }*/
        //� ������ ������ ���������� ��������� � ��������� �� ������
        References.electionsEnded = false;
        graceTime = 1;
        References.fightEnded = false;

        References.pointsForPlayerCandidate = 0;
        References.pointsForOppositeCandidate = 0;
        if (gameLevel)
        {
            PlayerPrefs.SetString("currentLevel", SceneManager.GetActiveScene().name);
        }
        PlayerPrefs.Save();
        levelNameForRestartButton = SceneManager.GetActiveScene().name;

        chatEnded = false; //no update

    }
    public void GoIntoFormation()
    {
        if (campAlignmentOn)
        {
            pressETutorial.SetActive(true); //���������� ���������
            for (int i = References.electors.Count - 1; i >= 0; i--)
            {
                References.electors[i].GetComponent<ElectorBehaviour>().PickFormationPoint(i);
            }
            goButton.SetActive(false); //EITHER
            OneTapButton.interactable = false;//OR
            Debug.Log("I did it");
        }
    }

    public void AllLookAtPlayer()
    {
        for (int i = References.electors.Count - 1; i >= 0; i--)
        {
            References.electors[i].GetComponent<ElectorBehaviour>().LookAtPlayer();
        }
    }
    public void ShowNextLine()
    {
        chatLineNumber++;
        if (chatLineNumber < chatLines.Count)
        {
            chatWindow.text = chatLines[chatLineNumber];
        }
        if (chatLineNumber == chatLines.Count - 1)
        {
            if (nextButtonSupport != null)
            {
                nextButtonSupport.SetActive(false);
            }
            nextButton.SetActive(false);
            goButton.SetActive(true);
        }
    }
    public void ShowNextFarewellLine()                 //THIS
    {

        if (endScreenText.activeInHierarchy)
        {
            endScreenText.SetActive(false);
        }

        if (endChatLineNumber < endChatLines.Count)
        {
            outroTextWindow.text = endChatLines[endChatLineNumber];
        }

        endChatLineNumber++;
        if (endChatLineNumber == endChatLines.Count)
        {
            if (outroLineButton != null)
            {
                outroLineButton.SetActive(false);
            }
            toMapButton.SetActive(true);

            //Door Closing Animation
            if (vanGameObj != null)
            {
                vanGameObj.GetComponent<VanScript>().CloseDoor();
            }
        }
    }
    public void CreditsOnOff()
    {
        if (creditsText.activeInHierarchy)
        {
            creditsText.SetActive(false);
        }
        else
        {
            creditsText.SetActive(true);
        }
    }
    public void StartNewGame()
    {
        SceneManager.LoadScene("Map");
        Time.timeScale = 1;
        References.gamesCount++;
    }

    public void ProgressReset()
    {
        PlayerPrefs.SetString("currentLevel", "1 - Hometown");
        PlayerPrefs.SetString("nextLevel", "1.1 - Hometown");

        PlayerPrefs.SetInt("highestLevel", 1);
        PlayerPrefs.SetInt("resultsReady", 0);
    }
    public void StartTurorial()
    {
        SceneManager.LoadScene("Start Tutorial");
        Time.timeScale = 1;
    }
    public void Resume()
    {
        menuGameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(levelNameForRestartButton);
        graceTime = 1;

        Time.timeScale = 1;
        References.gamesCount++;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
    public void FinalScene()
    {
        SceneManager.LoadScene("6 - The Results");
        Time.timeScale = 1;
    }
    public void Map()
    {
        //��������� ������ ���� ����� ����� �� ������
        PlayerPrefs.SetString("nextLevel", nextLevelName);
        int highestLevel = PlayerPrefs.GetInt("highestLevel");
        if (highestLevel < nextLevelNumber)
        {
            PlayerPrefs.SetInt("highestLevel", nextLevelNumber);
        }
        PlayerPrefs.Save();

        Time.timeScale = 1;

        SceneManager.LoadScene("Map");
    }
    private void Update()
    {
        //  Debug.Log("ElectionersResults: " + References.electors.Count);

        //SCORE UPDATE
        if (blueScore != null)
        {
            blueScore.text = References.pointsForPlayerCandidate.ToString();
            redScore.text = References.pointsForOppositeCandidate.ToString();
        }

        if (Input.GetButtonDown("Menu"))
        {
            if (menuGameObject.activeInHierarchy)
            {
                menuGameObject.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                menuGameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }

        //������� �� ���� 2 - ����������� �����������
        if (References.electors.Count == 0)// && !fightersSpawned)
        {
            //Debug.Log("Election Results: " + References.pointsForPlayerCandidate + ":" + References.pointsForOppositeCandidate);
            References.electionsEnded = true; //��� ��������
            if (!endScreenText.activeInHierarchy && !endScreenShown)
            {
                endScreenShown = true;
                timerBeforeShowingMenu -= Time.deltaTime;
                if (timerBeforeShowingMenu < 0)
                {
                    endScreenText.SetActive(true);
                    moveToCutButton.SetActive(true);
                }
            }
            /*if (timeBeforeShowdown <= 0)
            {
                //+++Player Body Change
                SpawnFighters();
                fightersSpawned = true;
            }*/
        }

        //������� �� ��������� �����
        if (fightersSpawned && !References.fightEnded) //check for fight ended?
        {
            Debug.Log("blues: " + References.blueFighters.Count);
            Debug.Log("reds: " + References.redFighters.Count);
            Debug.Log("References.fightEnded???: " + References.fightEnded);
            graceTime -= Time.deltaTime;
            if (graceTime < 0)
            {
                if (References.blueFighters.Count == 0)
                {
                    References.fightEnded = true;
                    redWinsText.SetActive(true);
                }

                if (References.redFighters.Count == 0)
                {
                    References.fightEnded = true;
                    blueWinsText.SetActive(true);
                }

                toMapButton.SetActive(true);
            }
        }
        if (cutsceneStarted)
        {
            timeOfCutscene -= Time.deltaTime;
            if (timeOfCutscene < 0)
            {
                //endScreenText.SetActive(true);
                if (outroLineButton != null) //just for the last level where there is only 1 line
                {
                    outroLineButton.SetActive(true);
                }
                cutsceneStarted = false;
            }
        }

        if (Input.GetButtonDown("Use") && campAlignmentOn)
        {
            if (pressETutorial.activeInHierarchy) //camp only
            {
                endScreenText.SetActive(false);
                welcomeTextButton.SetActive(false);
                pressETutorial.SetActive(false); //������� ���������
                chatWindowObject.SetActive(true);
                nextButton.SetActive(true);

                References.cameraTools.GetComponent<CameraTools>().moveToCutscene();
                References.thePlayer.GetComponent<PlayerBehaviour>().enabled = false;
                References.thePlayer.GetComponent<PlayerBehaviour>().myRigidbody.isKinematic = true;
                References.playerSpot.SetActive(false);

            }
        }
    }
    public void CutsceneStart()
    {
        //��������� ������, ������� ������� �������
        moveToCutButton.SetActive(false);
        endScreenText.SetActive(false);
        //player loses control
        References.thePlayer.GetComponent<PlayerBehaviour>().enabled = false;
        //spawn electors
        if (level1Scenario)
        {
            for (int i = 0; i < finalSpawnPoints.Count; i++)
            {
                GameObject newElector = Instantiate(elector, finalSpawnPoints[i].transform.position, cutsceneElectorSpawnPoint.transform.rotation);
                newElector.GetComponent<ElectorBehaviour>().neutralMood = false; //���� �� �����
            }
            agitator.SetActive(true);
        }
        //camera pans
        References.cameraTools.moveToCutscene();
        //Agitator ON ?? 
        chatEnded = true;
        //Timer Start -> presentation done
        cutsceneStarted = true;
        ShowNextFarewellLine(); //����� �������� ������

        //Show text by kiosk guy "woa" "+time to move on to another location. Make sure to come buy to get paid before you hit the road"
        //(!) Maybe make another camera pointing at him
        //Go to map button ON
    }
    public void FarewellAfterLooking()
    {
        goButton.SetActive(false);
        chatWindowObject.SetActive(false);
        outroWindowObject.SetActive(true);
        outroLineButton.SetActive(true);
        ShowNextFarewellLine(); //����� �������� ������

    }
    public void StartTheFight() //����������� ������� NEXT
    {
        //Agitator body switch
        if (agitator != null)
        {
            agitator.GetComponent<FighterScript>().redBody.SetActive(true);
            agitator.GetComponent<FighterScript>().checkMyTeam();
            agitator.GetComponent<AgitatorBehaviour>().myBody.SetActive(false);
            agitator.GetComponent<FighterScript>().targetAcquired = false;
            agitator.GetComponent<Rigidbody>().isKinematic = false;
        }
        //Player body switch
        References.thePlayer.GetComponent<PlayerBehaviour>().blueBody.SetActive(true);
        References.thePlayer.GetComponent<FighterScript>().checkMyTeam();
        References.thePlayer.GetComponent<PlayerBehaviour>().myBody.SetActive(false);
        //References.blueFighters.Add(References.thePlayer.GetComponent<FighterScript>());

        //Disabling the active text
        endScreenText.SetActive(false);
        outroLineButton.SetActive(false);
        //timeBeforeShowdown = 0; //triggers SpawnFighters
        theCage.SetActive(true);
        //timerBeforeShowingMenu = 55; //����������� ���, ����� �� ���������� �����
        SpawnFighters();
        fightersSpawned = true;
    }
    public void SpawnFighters()
    {
        {
            for (int i = 0; i < References.pointsForPlayerCandidate; i++)
            {
                Instantiate(BlueTeamFighter, blueFightersSpawns[Random.Range(0, blueFightersSpawns.Count)].transform.position, Quaternion.identity);
            }

            for (int i = 0; i < References.pointsForOppositeCandidate; i++)
            {
                Instantiate(RedTeamFighter, redFightersSpawns[Random.Range(0, redFightersSpawns.Count)].transform.position, Quaternion.identity);
            }
        }
    }
}
