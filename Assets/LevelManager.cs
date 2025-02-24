using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Меню
    public GameObject menuGameObject;
    public GameObject tutorialText1; //*
    public GameObject tutorialText2; //Recomended
    //Финальный бой
    public float timeBeforeShowdown;
    public GameObject RedTeamFighter;
    public GameObject BlueTeamFighter;
    public bool fightersSpawned;
    public List<GameObject> blueFightersSpawns;
    public List<GameObject> redFightersSpawns;

    public float graceTime;

    public GameObject endScreenText;
    public GameObject endScreenButton;
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

    private void Awake()
    {
        References.levelManager = this;
    }
    private void Start()
    {
        //В начале уровня сбрасываем изменения в навигации по этапам
        References.electionsEnded = false;
        graceTime = 1;
        References.fightEnded = false;

        References.pointsForPlayerCandidate = 0;
        References.pointsForOppositeCandidate = 0;

        Debug.Log("current scene name: " + SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("currentLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        levelNameForRestartButton = SceneManager.GetActiveScene().name;
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
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1;
        References.gamesCount++;
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
    public void Map()
    {
        SceneManager.LoadScene("Map");
        Time.timeScale = 1;
    }
    private void Update()
    {
        //SCORE UPDATE
        if(blueScore != null)
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

        //Переход на Этап 2 - голосование завершилось
        if (References.electors.Count == 0 && !fightersSpawned)
        {
            Debug.Log("Election Results: " + References.pointsForPlayerCandidate + ":" + References.pointsForOppositeCandidate);
            References.electionsEnded = true;
            if (!endScreenText.activeInHierarchy)
            {
                timerBeforeShowingMenu -= Time.deltaTime;
                if(timerBeforeShowingMenu < 0)
                {
                    endScreenText.SetActive(true);
                    endScreenButton.SetActive(true);
                }
            }

            /*if (timeBeforeShowdown <= 0)
            {
                //+++Player Body Change
                SpawnFighters();
                fightersSpawned = true;
            }*/
        }

        //Переход на Финальный экран
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
    }
    public void StartTheFight() //Запускается кнопкой NEXT
    {
        //Agitator body switch
        agitator.GetComponent<FighterScript>().redBody.SetActive(true);
        agitator.GetComponent<FighterScript>().checkMyTeam();
        agitator.GetComponent<AgitatorBehaviour>().myBody.SetActive(false);
        agitator.GetComponent<FighterScript>().targetAcquired = false;
        agitator.GetComponent<Rigidbody>().isKinematic = false;


        //Player body switch
        References.thePlayer.GetComponent<PlayerBehaviour>().blueBody.SetActive(true);
        References.thePlayer.GetComponent<FighterScript>().checkMyTeam();
        References.thePlayer.GetComponent<PlayerBehaviour>().myBody.SetActive(false);
        //References.blueFighters.Add(References.thePlayer.GetComponent<FighterScript>());

        //Disabling the active text
        endScreenText.SetActive(false);
        endScreenButton.SetActive(false);
        //timeBeforeShowdown = 0; //triggers SpawnFighters
        theCage.SetActive(true);
        //timerBeforeShowingMenu = 55; //необходимый шаг, чтобы не включилось снова
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
