using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ElectorBehaviour : MonoBehaviour
{
    public bool neutralMood;
    public bool inTalkWithPlayer;
    //public bool inTalkWithEnemy;
    NavMeshAgent agent;
    public float talkTime;
    public float turnSpeed;
    public float turnSpeedMultiplier;


    public bool timeToVote;
    public float timeBeforeVote;

    public GameObject myBody;
    public GameObject enemyCandidateBody;
    public GameObject playerCandidateBody;
    private GameObject myLeaveAreaObject;
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
        //если могу ходить и не проголосовал - гуляю
        if (agent.enabled && !voted)
        {
            if (neutralMood && agent.remainingDistance < 2)
            {
                GoToRandomNavPoint();
            }
        }

        //Взаимодействие с игроком
        if (!agent.enabled) //значит кто-то остановил, если игрок, то измеряем дистанцию
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
                inTalkWithPlayer = false; // закончили разговор
            }
        }

        //voting mechanics
        //если меня 1 раз переключили, иду голосовать после короткой задержки
        if (!myBody.activeInHierarchy && !voted && timeBeforeVote > 0)
        {
            timeBeforeVote -= Time.deltaTime;
        }
        else if(!voted && timeBeforeVote < 0)
        {
            timeToVote = true;
            References.targetElectors.Remove(this); //don't turn me anymore
            agent.enabled = true; //на случай, если агитатор поймал, пока мы ожидали
        }

        if (timeToVote) //если пора идти
        {
            if (agent.enabled) //если не в разговоре
            {
                agent.destination = References.votingPost.transform.position; //идём к столу
                if (Vector3.Distance(transform.position, References.votingPost.transform.position) < 1) //вручную считаем расстояние
                {
                    if (!voted)
                    {
                        if (enemyCandidateBody.activeInHierarchy)
                        {
                            References.pointsForOppositeCandidate++;
                        }
                        else { References.pointsForPlayerCandidate++; }
                    }
                    timeToVote = false;
                    myLeaveAreaObject = References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody;
                    agent.destination = myLeaveAreaObject.transform.position;
                    voted = true;
                    References.electors.Remove(this); //when VOTED - Ignore Player
                    //if my body blue -> Make Bluer and same for RED
                }
            }
        }
        if (voted && Vector3.Distance(transform.position, myLeaveAreaObject.transform.position) < 1)
        {
            Destroy(gameObject);
        }
        if(agent.enabled == false)
        {
            //Rotate
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            turnSpeedMultiplier += Time.deltaTime; //ускоряем
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = Random.Range(0, References.spawnPoints.Count);
        if(References.spawnPoints[randomNavPointIndex] != null) //чтобы не было ошибки на рестарте
        {
            agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
        }
    }
    public void TurnMe(PlayerBehaviour playerContacted)
    {
        myBody.SetActive(false);
        if (playerContacted != null)
        {
            playerCandidateBody.SetActive(true);
            if (enemyCandidateBody.activeInHierarchy)
            {
                enemyCandidateBody.SetActive(false);
                if(References.targetElectors.Count != 0)//если игрок обращает не последнего
                {
                    References.targetElectors.Add(this);
                }
            }
        }
        else
        {
            playerCandidateBody.SetActive(false);
            enemyCandidateBody.SetActive(true);
        }
    }
    public void JoinTalk(PlayerBehaviour playerContacted)
    {
        agent.enabled = false;
        if (playerContacted != null)
        {
            References.thePlayer.canPromote = false;
            inTalkWithPlayer = true;
        }
    }

    public void LeaveTalk()
    {
        agent.enabled = true;
        if (inTalkWithPlayer)
        {
            inTalkWithPlayer = false;
            References.thePlayer.canPromote = true;
            References.thePlayer.turnSpeedMultiplier = 3;
        }
        talkTime = 2;  //сбрасываем таймер для следующего разговора
        turnSpeedMultiplier = 2;
        if (voted)
        {
            myLeaveAreaObject = References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody;
            agent.destination = myLeaveAreaObject.transform.position;
        }
    }
    private void OnDestroy()
    {
        References.electors.Remove(this);
        References.targetElectors.Remove(this);
    }
}
