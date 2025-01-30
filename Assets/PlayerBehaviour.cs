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

    private void Awake()
    {
        References.thePlayer = this;
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        References.pointsForPlayerCandidate = 0;
        References.pointsForOppositeCandidate = 0;
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
        ElectorBehaviour nearestElectorSoFar = null;
        float nearestDistance = 1.5f; //max turn distance
        foreach (ElectorBehaviour thisElector in References.electors)
        {
            //how far is this one from the player?
            float thisDistance = Vector3.Distance(transform.position, thisElector.transform.position);
            //is it closer than anything else we've found?
            if (thisDistance <= nearestDistance)
            {
                //if it's THIS now it's the closest one
                nearestElectorSoFar = thisElector;
                nearestDistance = thisDistance;
            }
        }

        if (nearestElectorSoFar != null)
        {
            //show USE prompt
            //References.canvas.usePromptSignal = true;
            if (Input.GetButtonDown("Use"))
            {
                nearestElectorSoFar.JoinTalk(GetComponent<PlayerBehaviour>());
            }
        }
        if(References.electors.Count == 0)
        {
            Debug.Log("Election Results: " + References.pointsForPlayerCandidate + ":" + References.pointsForOppositeCandidate);
        }
    }
}
