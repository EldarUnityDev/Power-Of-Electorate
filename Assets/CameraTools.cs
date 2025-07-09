using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class CameraTools : MonoBehaviour
{
    public float desiredRotationXA;
    public float desiredRotationXB;

    public Vector3 desiredRotationA;
    public Vector3 desiredRotationB;

    public Vector3 pointA;
    public Vector3 pointB;

    public float maxMoveSpeed;
    public Vector3 cameraOffset;
    public bool moving;
    public float camTime;
    private void Awake()
    {
        References.cameraTools = this;
    }
    private void Start()
    {
        moving = false;
    }
    void Update()
    {
        if (moving)
        {
           // moveToPoint(pointA, desiredRotation);
        }
    }
    public void moveOn()
    {
        moving = true;
        StopAllCoroutines();
        StartCoroutine(SmoothLerp(camTime/2, pointA, desiredRotationA));  // 3f is the time in seconds the movement will take.
    }
    public void moveToCutscene()
    {
        moving = true;
        StopAllCoroutines();
        StartCoroutine(SmoothLerp(camTime, pointB, desiredRotationB));  // 3f is the time in seconds the movement will take.
    }

    private IEnumerator SmoothLerp(float time, Vector3 destination, Vector3 desiredRotation)
    {
        Vector3 startingPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            //transform.SetPositionAndRotation(Vector3.Lerp(startingPos, destination, (elapsedTime / time)), Quaternion.Lerp(transform.rotation, desiredRotation, (elapsedTime / time)));
            //transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            transform.eulerAngles = new Vector3(
    Mathf.LerpAngle(transform.eulerAngles.x, desiredRotation.x, (elapsedTime / time)),
    Mathf.LerpAngle(transform.eulerAngles.y, desiredRotation.y, (elapsedTime / time)),
    Mathf.LerpAngle(transform.eulerAngles.z, desiredRotation.z, (elapsedTime / time)));

            // transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        References.levelManager.chatEnded = true;
    }
    public void FaithfulLerp()
    {
        StartCoroutine(FaithfulLerp(camTime, pointA));
    }
    private IEnumerator FaithfulLerp(float time, Vector3 destination)
    {
        Vector3 startingPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}