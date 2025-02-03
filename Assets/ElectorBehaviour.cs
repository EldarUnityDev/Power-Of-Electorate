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
        //���� ���� ������ � �� ������������ - �����
        if (agent.enabled && !voted)
        {
            if (neutralMood && agent.remainingDistance < 2)
            {
                GoToRandomNavPoint();
            }
        }

        //�������������� � �������
        if (!agent.enabled) //������ ���-�� ���������, ���� �����, �� �������� ���������
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
                inTalkWithPlayer = false; // ��������� ��������
            }
        }

        //voting mechanics
        //���� ���� 1 ��� �����������, ��� ���������� ����� �������� ��������
        if (!myBody.activeInHierarchy && !voted && timeBeforeVote > 0)
        {
            timeBeforeVote -= Time.deltaTime;
        }
        else if(!voted && timeBeforeVote < 0)
        {
            timeToVote = true;
            References.targetElectors.Remove(this); //don't turn me anymore
            agent.enabled = true; //�� ������, ���� �������� ������, ���� �� �������
        }

        if (timeToVote) //���� ���� ����
        {
            if (agent.enabled) //���� �� � ���������
            {
                agent.destination = References.votingPost.transform.position; //��� � �����
                if (Vector3.Distance(transform.position, References.votingPost.transform.position) < 1) //������� ������� ����������
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
            turnSpeedMultiplier += Time.deltaTime; //��������
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);
        }
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = Random.Range(0, References.spawnPoints.Count);
        if(References.spawnPoints[randomNavPointIndex] != null) //����� �� ���� ������ �� ��������
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
                if(References.targetElectors.Count != 0)//���� ����� �������� �� ����������
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
        talkTime = 2;  //���������� ������ ��� ���������� ���������
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
