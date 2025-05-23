using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    public float upForce;
    public float forwardForce;
    Rigidbody myRigidbody;
    public float speed;
    public float turnSpeed;
    public float turnSpeedMultiplier;

    public GameObject myBody;
    public GameObject blueBody;
    public NavMeshAgent agent;
    public bool canPromote;  //����� ������������ ��� inConversation
    //public bool leaping;
    //public float leapingTimer;

    private void Awake()
    {
        References.thePlayer = this;
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        canPromote = true;

        agent = GetComponent<NavMeshAgent>(); //��� ������
        agent.enabled = false;
    }

    void Update()
    {
        //Debug.Log("First blues: " + References.blueFighters.Count);
        //Debug.Log("First reds: " + References.redFighters.Count);

        //MOVEMENT
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (!agent.enabled)
        {
            if (inputVector.magnitude > 0)
            {
                myRigidbody.velocity = inputVector * speed;
            }
        }

        //PROMOTION TARGET
        ElectorBehaviour nearestElectorSoFar = null;
        float nearestDistance = 1.5f; //max effect distance

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

        if (nearestElectorSoFar != null) //����� ����� �������� ��������� UI ��������
        {
            if (canPromote && Input.GetButtonDown("Use"))
            {
                nearestElectorSoFar.JoinTalk(gameObject);
            }
        }

        if (canPromote == false) //������� ��� �������
        {
            //Rotate
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            turnSpeedMultiplier += Time.deltaTime; //��������
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }

        if (References.fightEnded) //�������� � ������ ����������
        {
            agent.enabled = true;
        }

        //Shooting
        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.forward, 0);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

        /*if (!leaping && Input.GetButtonDown("Fire1")) //!leaping && 
        {
            leaping = true;
            //myRigidbody.isKinematic = false;
            myRigidbody.constraints = RigidbodyConstraints.None;
            myRigidbody.AddForce((cursorPosition * forwardForce) + transform.up * upForce);
            transform.LookAt(cursorPosition);
        }
         if (leaping)
         {
             leapingTimer += Time.deltaTime;

             if(leapingTimer >= 1f)
             {
                 leaping = false;
                 leapingTimer = 0;
                 myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                 //myRigidbody.isKinematic = true;
                 //myRigidbody.constraints = RigidbodyConstraints.None;
             }

         }*/
    }
    private void OnCollisionEnter(Collision collision)
    {

        /*if (collision.gameObject.name == "Ground")
        {
            leaping = false;
            leapingTimer = 0;
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }*/
    }
    private IEnumerator PlayerLeap()
    {
        yield return null;
    }
}
