using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterludeScript : MonoBehaviour
{
    public float delayBeforeInterlude;
    public int chatLineNumber;
    public List<string> chatLines;
    public GameObject chatWindowObject;
    public TextMeshProUGUI chatWindow;
    public GameObject nextButton;

    public GameObject promptText;
    public GameObject head;
    public GameObject mainCam;

    public bool interludeShown;
    void Start()
    {
        //delayBeforeInterlude = 5;
    }
    void Update()
    { if (!interludeShown && References.levelManager.chatEnded)
        {
            if (delayBeforeInterlude > 0)
            {
                delayBeforeInterlude -= Time.deltaTime;
            }
            else
            {
                ShowInterlude();
                interludeShown = true;
            }
        }
    }
    public void ShowInterlude()
    {
        Time.timeScale = 0.05f;                  //*stop time
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        head.SetActive(true);
        head.transform.LookAt(mainCam.transform.position);
        chatWindowObject.SetActive(true);        //show text "oh he's fast
        nextButton.SetActive(true);              //enable button "next"
        ShowNextLine();
    }
    public void EndInterlude()
    {
        head.SetActive(false);
        chatWindowObject.SetActive(false);        //hide text "oh he's fast
        nextButton.SetActive(false);              //disable button "next"
        promptText.SetActive(true);
        Time.timeScale = 1;    //resume time
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

    }
    public void HidePrompt()
    {
        promptText.SetActive(false);
    }
    public void ShowNextLine()
    {
        
        if (chatLineNumber < chatLines.Count)
        {
            chatWindow.text = chatLines[chatLineNumber];
        }
        if (chatLineNumber == chatLines.Count )
        {
            EndInterlude();
        }
        chatLineNumber++;

    }

}
