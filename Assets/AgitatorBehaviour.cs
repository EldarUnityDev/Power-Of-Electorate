using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AgitatorBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public ElectorBehaviour myTarget;
    public bool targetAcquired;
    public bool talking;
    public float talkTime;
    public bool readyToWork;
    public float breakTimer;
    public float turnSpeed;
    public float turnSpeedMultiplier;
    public float flightSpeedMultiplier;

    public bool coinFlipped;
    public bool secretFlight;

    public GameObject myBody;

    AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        talkTime = 2;
        audioSource = GetComponent<AudioSource>();
        flightSpeedMultiplier = 1;
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
            //References.canvas.usePromptSignal = true; 
            {
                agent.destination = nearestElectorSoFar.transform.position;
                myTarget = nearestElectorSoFar;
                targetAcquired = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!readyToWork) //если на перерыве
        {
            breakTimer -= Time.deltaTime;
            if(breakTimer < 0)
            {
                readyToWork = true;
            }
        }
        if(!targetAcquired && readyToWork && References.electors.Count > 0) //если нет цели
        {
            ChooseTarget(); //выбери
        }
        if(References.electors.Count == 0 && myBody.activeInHierarchy)
        {
            agent.destination = References.levelManager.redFightersSpawns[Random.Range(0, References.levelManager.redFightersSpawns.Count)].transform.position;
            //transform.position = References.levelManager.redFightersSpawns[Random.Range(0, References.levelManager.redFightersSpawns.Count)].transform.position;
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
            if(talking)
            {
                talkTime -= Time.deltaTime;
                if (talkTime < 0)
                {
                    myTarget.TurnMe(gameObject);
                    targetAcquired = false;
                    References.targetElectors.Remove(myTarget); //больше его не ищем
                    myTarget.LeaveTalk();

                    myTarget = null;    //сбросить цель
                    talkTime = 2;  //сбрасываем таймер для следующего разговора
                    talking = false; // закончили разговор
                    agent.enabled = true;
                    readyToWork = false; // делаем перерыв
                    if(turnSpeedMultiplier > 7)
                    {
                        turnSpeedMultiplier = 5;
                    }
                }
            }
        }

        if (agent.enabled == false)
        {
            //Rotate
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            turnSpeedMultiplier += Time.deltaTime; //ускоряем
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }

        if (References.targetElectors.Count == 0)
        {
            agent.enabled = true; //чтоб просто так не крутился

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
            targetAcquired = false;
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
            }
        }
    }
}
