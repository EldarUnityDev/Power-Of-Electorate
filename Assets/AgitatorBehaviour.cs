using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AgitatorBehaviour : MonoBehaviour
{
    Rigidbody myRigidbody;
    NavMeshAgent agent;
    public ElectorBehaviour myTarget;
    public bool targetAcquired;
    public bool talking;
    public float talkTime;
    public float currentTalkTime;
    public bool readyToWork;
    public bool justWalking;
    public float breakTimer;
    public float breakBetweenVoters;
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
       // References.pushables.Add(gameObject); //moved to START
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
        if (!beingKnocked && !readyToWork && References.levelManager.chatEnded) //перерыв
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer < 0)
            {
                readyToWork = true;
            }
        }
        if (!targetAcquired && readyToWork && References.electors.Count > 0) //если нет цели
        {
            ChooseTarget(); //выбери
        }
        
        if (targetAcquired) //если есть цель
        {
            if (agent.enabled)
            {
                agent.destination = myTarget.transform.position; //обновляем пункт назначения, потому что цель движется
            }
            if (!talking && Vector3.Distance(transform.position, myTarget.transform.position) < 1.3f) //если не говорим и мы достаточно близко
            {
                talking = true; //запускаем таймер на разговор
                agent.enabled = false;
                myTarget.JoinTalk(gameObject);

            }
            if (talking)
            {
                //Rotate
                Vector3 lateralOffset = transform.right * Time.deltaTime;
                turnSpeedMultiplier += Time.deltaTime; //ускоряем
                transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
                myTarget.transform.LookAt(myTarget.transform.position + myTarget.transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);

                currentTalkTime -= Time.deltaTime;
                myTarget.GetComponent<SliderForConversion>().ShowFraction(1 - currentTalkTime / talkTime);

            }
            if (currentTalkTime < 0)
            {
                References.targetElectors.Remove(myTarget); //больше его не ищем
                myTarget.TurnMe(gameObject);
                myTarget.LeaveTalk();
                myTarget = null;    //сбросить цель

                talking = false; // закончили разговор
                targetAcquired = false;

                currentTalkTime = talkTime;  //сбрасываем таймер для следующего разговора
                agent.enabled = true;
                readyToWork = false; // делаем перерыв
                breakTimer = breakBetweenVoters;
                GoToRandomNavPoint();
                if (turnSpeedMultiplier > 7)
                {
                    turnSpeedMultiplier = 5;
                }
            }
        }

        if (References.targetElectors.Count == 0 && !talking)
        {
            agent.enabled = true; //чтоб просто так не крутился
            targetAcquired = false;

            GetComponent<NavMeshAgent>().speed = 3;
            if (!justWalking)
            {
                GoToRandomNavPoint();
                justWalking = true;
            }
            if(Vector3.Distance(transform.position, agent.destination) < 1.3f)
            {
                justWalking = false;
            }

            /*
            //flip the coin 0.1
            //if gamesCount >=2
            //set secretFlight
            if (References.gamesCount >= 2)
            {
                if (!coinFlipped)
                {
                    float chance = Random.value;
                    if (chance < 0.1) //если попадаем в 10% то будет секретный полёт
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
                flightSpeedMultiplier += Time.deltaTime * 0.2f; //ускоряем
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
                readyToWork = true; //снова готовы работать
                agent.enabled = true;
            }
        }
    }
    public void GetPushed()
    {
        Debug.Log("PIZDA");
        if (knockable) //&& DISTANCE 
        {
            if (talking)
            {
                References.targetElectors.Add(myTarget);
            }
            talking = false;
            readyToWork = false; //пока не готовы работать
            if (myTarget != null)
            {
                myTarget.LeaveTalk(); //прекращаем диалог
            }
            currentTalkTime = talkTime;
            targetAcquired = false;
            myTarget = null; //сбрасываем цель
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
        if (References.spawnPoints[randomNavPointIndex] != null) //чтобы не было ошибки на рестарте
        {
            agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
        }
    }
    private void OnDestroy()
    {
        References.pushables.Remove(gameObject);
    }
}
