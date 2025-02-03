using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    Rigidbody myRigidbody;
    public float speed;
    public float turnSpeed;
    public float turnSpeedMultiplier;

    public GameObject myBody;
    public NavMeshAgent agent;
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
        canPromote = true;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

    }

    void Update()
    {

        //MOVEMENT
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (!agent.enabled)
        {
            if (inputVector.magnitude > 0)
            {
                myRigidbody.velocity = inputVector * speed;
            }
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
            if (canPromote && Input.GetButtonDown("Use"))
            {
                nearestElectorSoFar.JoinTalk(GetComponent<PlayerBehaviour>());
            }
        }

        if (canPromote == false)
        {
            //Rotate
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            turnSpeedMultiplier += Time.deltaTime; //ускоряем
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }

        if (References.fightEnded)
        {
            agent.enabled = true;
        }
    }
}
