using GLTF.Schema;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    public float upForce;
    public float forwardForce;
    public Rigidbody myRigidbody;
    public float speed;
    public float turnSpeed;
    public float turnSpeedMultiplier;

    public GameObject myBody;
    public GameObject blueBody;
    public NavMeshAgent agent;
    public bool canPromote;  //также используется как inConversation
    //public bool leaping;
    //public float leapingTimer;

    //
    public GameObject aimAssist;
    public GameObject myPushTarget;

    AudioSource pushAudioSource;

    private void Awake()
    {
        References.thePlayer = this;
    }
    void Start()
    {
        pushAudioSource = GetComponent<AudioSource>();

        myRigidbody = GetComponent<Rigidbody>();
        canPromote = true;

        agent = GetComponent<NavMeshAgent>(); //для финала
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
            if (thisDistance <= nearestDistance && !thisElector.GetComponent<ElectorBehaviour>().playerCandidateBody.activeInHierarchy) //BIG CHANGE
            {
                //if it's THIS now it's the closest one
                nearestElectorSoFar = thisElector;
                nearestDistance = thisDistance;
            }
        }

        if (nearestElectorSoFar != null) //ЗДЕСЬ можно добавить включение UI кружочка
        {
            if (canPromote && Input.GetButtonDown("Use"))
            {
                nearestElectorSoFar.JoinTalk(gameObject);
            }
        }

        if (canPromote == false) //вращаем при диалоге
        {
            //Rotate
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            turnSpeedMultiplier += Time.deltaTime; //ускоряем
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }

        if (References.fightEnded) //отбираем у игрока управление
        {
            agent.enabled = true;
        }
        /*if (References.levelManager.agitator != null)
        {
            if (Input.GetButtonDown("Fire1") && Vector3.Distance(transform.position, References.levelManager.agitator.transform.position) <= 3)
            {

                References.levelManager.agitator.GetComponent<AgitatorBehaviour>().GetPushed();

            }
        }*/

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

        //Pushing
        GameObject nearestPushable = null;
        float nearestDistanceForPushable = 3;

        foreach (GameObject pushable in References.pushables)
        {
            if(References.thePlayer != null) //проверка ради кнопки RESTART
            {
                //how far is this one from the player?
                float thisDistance = Vector3.Distance(transform.position, pushable.transform.position);
                //is it closer than anything else we've found?
                if (thisDistance <= nearestDistanceForPushable)
                {
                    //if it's THIS now it's the closest one
                    nearestPushable = pushable;
                    nearestDistanceForPushable = thisDistance;
                }
            }
        }

        if (nearestPushable != null)
        {
            SwitchMyPushTarget(nearestPushable);
            //Толкание
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space) && Vector3.Distance(transform.position, nearestPushable.transform.position) <= 3)
            {
                if (nearestPushable.GetComponent<AgitatorBehaviour>() != null)
                {
                    nearestPushable.GetComponent<AgitatorBehaviour>().GetPushed();
                }
                if (nearestPushable.GetComponent<FriendlyAgitatorBehaviour>() != null)
                {
                    nearestPushable.GetComponent<FriendlyAgitatorBehaviour>().GetPushed();
                }
                pushAudioSource.Play();

            }
        }
        if (myPushTarget != null && Vector3.Distance(transform.position, myPushTarget.transform.position) > 3)
        {
            if (myPushTarget.GetComponent<AgitatorBehaviour>() != null)
            {
                myPushTarget.GetComponent<AgitatorBehaviour>().myOutline.SetActive(false);            //Подсветка ВЫКЛ
            }
            if (myPushTarget.GetComponent<FriendlyAgitatorBehaviour>() != null)
            {
                myPushTarget.GetComponent<FriendlyAgitatorBehaviour>().myOutline.SetActive(false);            //Подсветка ВЫКЛ
            }
        }
    }
    public void SwitchMyPushTarget(GameObject nearestTarget)
    {

        if (myPushTarget != null)
        {
            if (myPushTarget.GetComponent<AgitatorBehaviour>() != null)
            {
                myPushTarget.GetComponent<AgitatorBehaviour>().myOutline.SetActive(false);            //Подсветка ВЫКЛ
            }
            if (myPushTarget.GetComponent<FriendlyAgitatorBehaviour>() != null)
            {
                myPushTarget.GetComponent<FriendlyAgitatorBehaviour>().myOutline.SetActive(false);            //Подсветка ВЫКЛ
            }
        }

        myPushTarget = nearestTarget;

        if (aimAssist != null)
        {
            aimAssist.transform.position = myPushTarget.transform.position + Vector3.up;
        }
        if (myPushTarget.GetComponent<AgitatorBehaviour>() != null)
        {
            myPushTarget.GetComponent<AgitatorBehaviour>().myOutline.SetActive(true);            //Подсветка ВКЛ
        }
        if (myPushTarget.GetComponent<FriendlyAgitatorBehaviour>() != null)
        {
            myPushTarget.GetComponent<FriendlyAgitatorBehaviour>().myOutline.SetActive(true);            //Подсветка ВЫКЛ
        }
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
    private void OnDestroy()
    {
        References.thePlayer = null;
    }
    private IEnumerator PlayerLeap()
    {
        yield return null;
    }
}
