using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    Rigidbody myRigidbody;
    public float speed;

    public GameObject myBody;
    public GameObject hostileBody;

    public bool pantsOnFire; //must run a lap
    public bool canPromote;  //useable on electors disabled

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //MOVEMENT
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (inputVector.magnitude > 0)
        {
            myRigidbody.velocity = inputVector * speed;
        }

        //use the nearest usable
        Useable nearestUseableSoFar = null;
        float nearestDistance = 3; //max pickup distance
        foreach (Useable thisUseable in References.useables)
        {
            //how far is this one from the player?
            float thisDistance = Vector3.Distance(transform.position, thisUseable.transform.position);
            //is it closer than anything else we've found?
            if (thisDistance <= nearestDistance)
            {
                //if it's THIS now it's the closest one
                nearestUseableSoFar = thisUseable;
                nearestDistance = thisDistance;
            }
            //+++ challenge - check if it's in front of us
        }

        if (nearestUseableSoFar != null)
        {
            //show USE prompt
            //References.canvas.usePromptSignal = true;
            if (Input.GetButtonDown("Use"))
            {
                nearestUseableSoFar.Use();
            }
        }
    }
}
