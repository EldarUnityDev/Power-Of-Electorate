using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPlayer : MonoBehaviour
{
    public GameObject playerModel;
    public GameObject currentPad;
    private void Start()
    {
        string currentPadName = PlayerPrefs.GetString("currentLevel");
        if(currentPadName != null)
        {
            for (int i = 0; i < References.pads.Count; i++)
            {

                if (References.pads[i].GetComponent<EventClick>().levelToStartName == currentPadName)
                {
                    currentPad = References.pads[i];
                    transform.position = currentPad.transform.position + new Vector3(0,0.25f,0);
                }
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
            Debug.Log("level To start: " + currentPad.GetComponentInParent<EventClick>().levelToStartName.ToString());
            StartCoroutine(SmoothLerp(1, pad));  // 3f is the time in seconds the movement will take.

        }
    }
    public void StartALevel()
    {
        //Debug.Log("level To start: "+ References.currentPad.GetComponent<EventClick>().levelToStartName.ToString());
        SceneManager.LoadScene(currentPad.GetComponentInParent<EventClick>().levelToStartName.ToString());
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
