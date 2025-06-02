using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPlayer : MonoBehaviour
{
    public GameObject playerModel;
    public GameObject currentPad;
    public GameObject mainCamera;
    public GameObject cameraForEnd;
    public List<GameObject> pads;
    public GameObject resultsButton;
    //int highestUnlockedLevel;
    private void Start()
    {
        //highestUnlockedLevel = 3;
        string currentPadName = PlayerPrefs.GetString("currentLevel");
        string nextPadName = PlayerPrefs.GetString("nextLevel");
        int resultsReady = PlayerPrefs.GetInt("resultsReady");

        if (resultsReady == 1) //Endgame Button to show cutscene
        {
            resultsButton.SetActive(true);
        }

        if (currentPadName != null)
        {
            Debug.Log("1 - current Pad " + currentPadName);
            for (int i = 0; i < References.pads.Count; i++)
            {

                if (References.pads[i].GetComponent<ClickPadScript>().levelToStartName == currentPadName)
                {
                    currentPad = References.pads[i];
                    transform.position = currentPad.transform.position + new Vector3(0, 0.25f, 0);
                }
            }
        }
        else
        {
            Debug.Log("0 - current Pad 0" + currentPadName);

            currentPad = References.pads[1];
        }
        if (nextPadName == "5 - Big City" || currentPad.GetComponentInParent<ClickPadScript>().levelToStartName == "5 - Big City")
        {
            mainCamera.SetActive(false);
            cameraForEnd.SetActive(true);
        }
        UnlockPads();
    }
    public void FinalScene()
    {
        SceneManager.LoadScene("6 - The Results");
        Time.timeScale = 1;
    }
    public void UnlockPads()
    {
        int highestUnlockedLevel = PlayerPrefs.GetInt("highestLevel");
        //my numba
        Debug.Log("current Number " + highestUnlockedLevel);

        if (highestUnlockedLevel != 0)
        {
            for (int i = 0; i < pads.Count; i++)
            {
                if (i <= highestUnlockedLevel)
                {
                    pads[i].GetComponent<ClickPadScript>().unlocked = true;
                }
                pads[i].GetComponent<ClickPadScript>().RefreshStatus();
            }
        }
    }
    public void MoveToPad(GameObject pad)
    {
        if (currentPad != pad)
        {
            playerModel.transform.LookAt(pad.transform.position);
            currentPad = pad;
            StopAllCoroutines();
            Debug.Log("level To start: " + currentPad.GetComponentInParent<ClickPadScript>().levelToStartName.ToString());
            StartCoroutine(SmoothLerp(1, pad));  // 3f is the time in seconds the movement will take.

            if (currentPad.GetComponentInParent<ClickPadScript>().levelToStartName.ToString() == "5 - Big City" || currentPad.GetComponentInParent<ClickPadScript>().levelToStartName.ToString() == "4 - Co-op Town")
            {
                mainCamera.SetActive(false);
                cameraForEnd.SetActive(true);
            }
            else
            {
                mainCamera.SetActive(true);
                cameraForEnd.SetActive(false);
            }
        }
    }
    public void StartALevel()
    {
        //Debug.Log("level To start: "+ References.currentPad.GetComponent<EventClick>().levelToStartName.ToString());
        SceneManager.LoadScene(currentPad.GetComponentInParent<ClickPadScript>().levelToStartName.ToString());
        Time.timeScale = 1;
    }
    private IEnumerator SmoothLerp(float time, GameObject destination)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = destination.transform.position + new Vector3(0, 0.3f, 0);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
