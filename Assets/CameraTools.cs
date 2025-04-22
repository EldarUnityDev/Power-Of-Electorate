using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class CameraTools : MonoBehaviour
{
    Vector3 desiredPosition;
    public Quaternion desiredRotation;
    public Vector3 pointA;
    public Vector3 pointB;

    public float maxMoveSpeed;
    public Vector3 cameraOffset;
    public bool moving;
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
        StartCoroutine(SmoothLerp(1, pointA, desiredRotation));  // 3f is the time in seconds the movement will take.
    }
    public void moveToCutscene()
    {
        moving = true;
        StopAllCoroutines();
        StartCoroutine(SmoothLerp(1, pointB, desiredRotation));  // 3f is the time in seconds the movement will take.
    }
    //UNUSED
    public void moveToPoint(Vector3 desiredPosition, Quaternion desiredRotation)
    {
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, maxMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, maxMoveSpeed * Time.deltaTime);
    }
    private IEnumerator SmoothLerp(float time, Vector3 destination, Quaternion desiredRotation)
    {
        Vector3 startingPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            //transform.SetPositionAndRotation(Vector3.Lerp(startingPos, destination, (elapsedTime / time)), Quaternion.Lerp(transform.rotation, desiredRotation, (elapsedTime / time)));

            //transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            transform.eulerAngles = new Vector3(
    Mathf.LerpAngle(transform.eulerAngles.x, 42, (elapsedTime / time)),
    Mathf.LerpAngle(transform.eulerAngles.y, 0, (elapsedTime / time)),
    Mathf.LerpAngle(transform.eulerAngles.z, 0, (elapsedTime / time)));

            // transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        References.levelManager.chatEnded = true;
        Debug.Log("AYA  "+References.levelManager.chatEnded);
    }
}