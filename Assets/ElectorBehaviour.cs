using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ElectorBehaviour : MonoBehaviour
{
    public bool neutralMood;
    public bool inTalkWithPlayer;
    NavMeshAgent agent;
    public float talkTime;

    public bool timeToVote;
    public float timeBeforeVote;

    public GameObject myBody;
    public GameObject candidate1Body;
    public GameObject candidate2Body;
    public bool voted;
    private void Awake()
    {
        References.electors.Add(this);
        References.targetElectors.Add(this);

    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        talkTime = 2;
    }

    void Update()
    {
        //Moving from navPoint to navPoint at Random
        if (agent.enabled)
        {
            if (neutralMood && agent.remainingDistance < 2)
            {
                GoToRandomNavPoint();
            }
        }
        if (!agent.enabled)
        {
            if (inTalkWithPlayer && Vector3.Distance(transform.position, References.thePlayer.transform.position) > 2)
            {
                LeaveTalk();
            }
        }
        if (inTalkWithPlayer)
        {
            talkTime -= Time.deltaTime;
            if (talkTime < 0)
            {
                TurnMe(References.thePlayer.GetComponent<PlayerBehaviour>());
                LeaveTalk();
                talkTime = 1;  //сбрасываем таймер для следующего разговора
                inTalkWithPlayer = false; // закончили разговор
            }
        }

        //voting mechanics
        if (!myBody.activeInHierarchy && timeBeforeVote > 0)
        {
            timeBeforeVote -= Time.deltaTime;
        }
        else if(timeBeforeVote < 0)
        {
            timeToVote = true;
            References.targetElectors.Remove(this); //don't turn me anymore
            agent.enabled = true; //на случай, если говорим с агитатором
        }

        if (timeToVote) //I go vote
        {
            if (agent.enabled)
            {
                agent.destination = References.votingPost.transform.position;
                if (agent.remainingDistance < 2)
                {
                    if (!voted)
                    {
                        if (candidate1Body.activeInHierarchy)
                        {
                            References.pointsForOppositeCandidate++;

                        }
                        else { References.pointsForPlayerCandidate++; }
                    }

                    timeToVote = false;
                    agent.destination = References.leaveArea.transform.position;
                    voted = true;
                    
                }
            }
        }
        if (voted && Vector3.Distance(transform.position, References.leaveArea.transform.position) < 1)
        {
            Destroy(gameObject);
        }
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = Random.Range(0, References.spawnPoints.Count);
        agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
    }
    public void TurnMe(PlayerBehaviour playerContacted)
    {
        myBody.SetActive(false);
        if (playerContacted != null)
        {
            candidate2Body.SetActive(true);
            if (candidate1Body.activeInHierarchy)
            {
                candidate1Body.SetActive(false);
                if(References.targetElectors.Count != 0)//если игрок обращает не последнего
                {
                    References.targetElectors.Add(this);
                }
            }

        }
        else
        {
            candidate2Body.SetActive(false);
            candidate1Body.SetActive(true);
        }
    }
    public void JoinTalk(PlayerBehaviour playerContacted)
    {
        agent.enabled = false;
        if (playerContacted != null)
        {
            inTalkWithPlayer = true;
        }
    }

    public void LeaveTalk()
    {
        agent.enabled = true;
        inTalkWithPlayer = false;
    }
    private void OnDestroy()
    {
        References.electors.Remove(this);
        References.targetElectors.Remove(this);
    }
}
