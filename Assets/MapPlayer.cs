using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{
    public GameObject playerModel;
    public GameObject currentPad;
    private void Start()
    {
        //playerModel = GetComponent<GameObject>();

    }
    public void MoveToPad(GameObject pad)
    {
        if (References.currentPad != pad)
        {
            playerModel.transform.LookAt(pad.transform.position);
            References.currentPad = pad;
            StopAllCoroutines();
            StartCoroutine(SmoothLerp(1, pad));  // 3f is the time in seconds the movement will take.
        }
    }
    private IEnumerator SmoothLerp(float time, GameObject destination)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = destination.transform.position +new Vector3(0,0.3f,0);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
