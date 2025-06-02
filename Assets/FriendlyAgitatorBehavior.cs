using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class FriendlyAgitatorBehaviour : MonoBehaviour
{
    Rigidbody myRigidbody;
    NavMeshAgent agent;
    public ElectorBehaviour myTarget;
    public bool targetAcquired;
    public bool talking;
    public float talkTime;
    public float currentTalkTime;
    public bool readyToWork;
    public float breakTimer;
    public float turnSpeed;
    public float turnSpeedMultiplier;
    public float flightSpeedMultiplier;

    public bool coinFlipped;
    public bool secretFlight;

    public GameObject myBody;

    AudioSource audioSource;
    public float knockBackForce;
    public float upForce;

    public bool knockable;
    public bool beingKnocked;
    public float knockbackTimer;
    public float knockbackTime;

    public GameObject myOutline;

    private void Awake()
    {
        //References.pushables.Add(gameObject); //moved to START
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        flightSpeedMultiplier = 1;
        myRigidbody = GetComponent<Rigidbody>();
        currentTalkTime = talkTime;
        if (knockable)
        {
            References.pushables.Add(gameObject);
        }
    }
    public void ChooseTarget()
    {
        ElectorBehaviour nearestElectorSoFar = null;
        float nearestDistance = 20; //max search distance
        foreach (ElectorBehaviour thisElector in References.targetElectors)
        {
            //how far is this one from the agitator?
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
            agent.destination = nearestElectorSoFar.transform.position;
            myTarget = nearestElectorSoFar;
            targetAcquired = true;
        }

    }

    void Update()
    {
        if (!beingKnocked && !readyToWork && References.levelManager.chatEnded) //�������
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer < 0)
            {
                readyToWork = true;
            }
        }

        if (!targetAcquired && readyToWork && References.targetElectors.Count > 0) //���� ��� ����
        {
            ChooseTarget(); //������
        }
        /*
        if (References.electors.Count == 0 && myBody.activeInHierarchy)
        {
            agent.destination = References.levelManager.redFightersSpawns[Random.Range(0, References.levelManager.redFightersSpawns.Count)].transform.position;
            //transform.position = References.levelManager.redFightersSpawns[Random.Range(0, References.levelManager.redFightersSpawns.Count)].transform.position;
        }
        */
        if (targetAcquired) //���� ���� ����
        {
            if (agent.enabled)
            {
                agent.destination = myTarget.transform.position; //��������� ����� ����������, ������ ��� ���� ��������
            }
            if (!talking && Vector3.Distance(transform.position, myTarget.transform.position) < 1.3f) //���� �� ������� � �� ���������� ������
            {
                talking = true;                     //��������� ������ �� ��������
                agent.enabled = false;              //�����������
                myTarget.JoinTalk(gameObject);      //���������� � �����������
            }
            if (talking)
            {
                //Rotate
                Vector3 lateralOffset = transform.right * Time.deltaTime;
                turnSpeedMultiplier += Time.deltaTime; //��������
                transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);

                
                currentTalkTime -= Time.deltaTime;
                if (currentTalkTime < 0)
                {
                    References.targetElectors.Remove(myTarget); //������ ��� �� ����
                    myTarget.TurnMe(gameObject);
                    myTarget.LeaveTalk();
                    myTarget = null;    //�������� ����

                    talking = false; // ��������� ��������
                    targetAcquired = false;

                    currentTalkTime = talkTime;  //���������� ������ ��� ���������� ���������
                    agent.enabled = true;
                    readyToWork = false; // ������ �������
                    breakTimer = 1;
                    GoToRandomNavPoint();
                    if (turnSpeedMultiplier > 7)
                    {
                        turnSpeedMultiplier = 5;
                    }
                }
            }
        }

        if (References.targetElectors.Count == 0)
        {
            agent.enabled = true; //���� ������ ��� �� ��������
            targetAcquired = false;
            /*
            //flip the coin 0.1
            //if gamesCount >=2
            //set secretFlight
            if (References.gamesCount >= 2)
            {
                if (!coinFlipped)
                {
                    float chance = Random.value;
                    if (chance < 0.1) //���� �������� � 10% �� ����� ��������� ����
                    {
                        secretFlight = true;
                    }
                    coinFlipped = true;
                }
            }
            if (secretFlight)
            {
                agent.enabled = false;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                    audioSource.volume = 0;
                }
                flightSpeedMultiplier += Time.deltaTime * 0.2f; //��������
                transform.position += Vector3.up * Time.deltaTime * flightSpeedMultiplier;
                if (transform.position.y < 8 && transform.position.y > 1)
                {
                    audioSource.volume += Time.deltaTime * 0.18f;
                }
                else
                {
                    audioSource.volume -= Time.deltaTime * 0.25f;
                }
                if (transform.position.y > 25)
                {
                    Destroy(gameObject);
                }
            }*/
        }

        //Knockback
        if (beingKnocked)
        {
            knockbackTimer += Time.deltaTime;

            if (knockbackTimer >= knockbackTime)
            {
                beingKnocked = false;
                knockbackTimer = 0;
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                myRigidbody.isKinematic = true;
                readyToWork = true; //����� ������ ��������
                agent.enabled = true;
            }
        }
    }
    public void GetPushed()
    {
        Debug.Log("we did it boys");
        if (knockable)
        {
            if (talking)
            {
                References.targetElectors.Add(myTarget);
            }
            talking = false;
            readyToWork = false; //���� �� ������ ��������
            if (myTarget != null)
            {
                myTarget.LeaveTalk(); //���������� ������
            }
            currentTalkTime = 2;
            targetAcquired = false;
            myTarget = null; //���������� ����
            agent.enabled = false;
            myRigidbody.isKinematic = false;
            myRigidbody.constraints = RigidbodyConstraints.None;
            //Vector3 direc = transform.position - References.thePlayer.transform.position;
            myRigidbody.AddForce((transform.position - References.thePlayer.transform.position).normalized * knockBackForce + transform.up * upForce); //-player position
            beingKnocked = true;
        }
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = Random.Range(0, References.spawnPoints.Count);
        if (References.spawnPoints[randomNavPointIndex] != null) //����� �� ���� ������ �� ��������
        {
            agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
        }
    }
    private void OnDestroy()
    {
        References.pushables.Remove(gameObject);
    }
}
