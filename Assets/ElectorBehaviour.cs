using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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

    public GameObject auraOutline;
    public Slider auraOutlineSlider;
    public GameObject playerAura;
    public GameObject enemyAura;

    public float talkDistance;
    public float talkAbandonTimer;

    Vector3 myAuraDef;
    public GameObject sliderGameObject;
    public GameObject questionMark;
    private void Awake()
    {
        References.electors.Add(this);
        References.targetElectors.Add(this);
    }
    void Start()
    {
        myAuraDef = playerAura.transform.localScale;
        agent = GetComponent<NavMeshAgent>();
        talkTime = 2;
        talkAbandonTimer = 1;
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
        //if (!agent.enabled) //������ ���-�� ���������, ���� �����, �� �������� ���������
        if (questionMark.activeInHierarchy)
        {
            //Quaternion lookRotation = Camera.main.transform.rotation;
            questionMark.transform.rotation = Quaternion.Euler(questionMark.transform.localRotation.x, Camera.main.transform.rotation.y, questionMark.transform.localRotation.z); ;
        }

        if (inTalkWithPlayer)
        {
            //Rotate throughout the whole event
            Vector3 lateralOffset = transform.right * Time.deltaTime;
            transform.LookAt(transform.position + transform.forward + lateralOffset * turnSpeed * turnSpeedMultiplier);

            //if (talkAbandonTimer == 1) //���� �� � ������ � ������� + ������ ������ �� ������, �� ����� �����
            if (Vector3.Distance(transform.position, References.thePlayer.transform.position) < talkDistance)
            {
                References.thePlayer.GetComponent<PlayerBehaviour>().canPromote = false;

                talkTime -= Time.deltaTime;
                GetComponent<SliderForConversion>().ShowFraction(1 - talkTime / 2);
                //If player close enough, continue spinning
                turnSpeedMultiplier += Time.deltaTime; //��������
            }

            if (talkTime < 0)
            {
                TurnMe(References.thePlayer.gameObject);
                LeaveTalk();
                inTalkWithPlayer = false; // ��������� ��������
            }

            //Check distance
            if (Vector3.Distance(transform.position, References.thePlayer.transform.position) > talkDistance)
            {
                References.thePlayer.GetComponent<PlayerBehaviour>().canPromote = true;
                StartCoroutine(QuestionMarkAppear()); // Animation
                questionMark.SetActive(true);
                //// Rewind
                if (talkTime < 2)
                {
                    talkTime += 0.5f * Time.deltaTime;
                }
                GetComponent<SliderForConversion>().ShowFraction(1 - talkTime / 2);

                if(turnSpeedMultiplier > 2)
                {
                    turnSpeedMultiplier -= Time.deltaTime; //
                }
                ///

                /*talkAbandonTimer -= Time.deltaTime;
                questionMark.SetActive(true);
                //Debug.Log("ABANON IN: " + talkAbandonTimer);
                Debug.Log("questn??? " + questionMarkReady);
                playerAura.transform.localScale = new Vector3(playerAura.transform.localScale.x - Time.deltaTime * 2, playerAura.transform.localScale.y, playerAura.transform.localScale.z - Time.deltaTime * 2);
                if (talkAbandonTimer < 0)
                {
                    LeaveTalk();
                }*/
            }
            else
            {
                questionMark.SetActive(false);
                StopAllCoroutines();
                GetComponent<SliderForConversion>().timerSlider.GetComponent<CanvasGroup>().alpha = 1;
                auraOutlineSlider.GetComponent<CanvasGroup>().alpha = 1;
                talkAbandonTimer = 1;
                playerAura.transform.localScale = myAuraDef;
            }
        }

        //voting mechanics
        //���� ���� 1 ��� �����������, ��� ���������� ����� �������� ��������
        if (!myBody.activeInHierarchy && !voted && timeBeforeVote > 0)
        {
            timeBeforeVote -= Time.deltaTime;
        }
        else if (!voted && timeBeforeVote < 0)
        {
            timeToVote = true;
            References.targetElectors.Remove(this); //don't turn me anymore
            agent.enabled = true; //�� ������, ���� �������� ������, ���� �� �������
        }

        if (timeToVote) //���� ���� ����
        {
            if (agent.enabled) //���� �� � ���������
            {
                References.electors.Remove(this);
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
                        voted = true;
                    }
                    timeToVote = false;
                    myLeaveAreaObject = References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody;
                    agent.destination = myLeaveAreaObject.transform.position;
                    References.electors.Remove(this); //when VOTED - Ignore Player
                    //if my body blue -> Make Bluer and same for RED
                }
            }
        }
        if (voted && Vector3.Distance(transform.position, myLeaveAreaObject.transform.position) < 1)
        {
            Destroy(gameObject);
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
    public void TurnMe(GameObject turner)
    {
        myBody.SetActive(false);
        if (turner.GetComponent<PlayerBehaviour>() != null)
        {
            playerCandidateBody.SetActive(true);
            if (enemyCandidateBody.activeInHierarchy)
            {
                enemyCandidateBody.SetActive(false);
                if (References.targetElectors.Count != 0)//���� ����� �������� �� ����������
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
    public void JoinTalk(GameObject interlocutor)
    {
        agent.enabled = false;
        auraOutline.SetActive(true);

        if (interlocutor.GetComponent<PlayerBehaviour>() != null)
        {
            //activate Player CIRCLE
            //playerAura.SetActive(true);
            References.thePlayer.canPromote = false;
            inTalkWithPlayer = true;
            sliderGameObject.SetActive(true);
        }
        else
        { //activate Enemy Circle
            enemyAura.SetActive(true);
        }
    }

    public void LeaveTalk()
    {

        auraOutline.SetActive(false);
        sliderGameObject.SetActive(false);
        questionMark.SetActive(false);
        agent.enabled = true;
        if (inTalkWithPlayer)
        {
            playerAura.SetActive(false);
            inTalkWithPlayer = false;
            References.thePlayer.canPromote = true;
            References.thePlayer.turnSpeedMultiplier = 3;
        }
        else { enemyAura.SetActive(false); }
        talkTime = 2;  //���������� ������ ��� ���������� ���������
        turnSpeedMultiplier = 2;
        if (voted)
        {
            myLeaveAreaObject = References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody;
            agent.destination = myLeaveAreaObject.transform.position;
        }
    }
    private IEnumerator QuestionMarkAppear()
    {
        float elapsedTime = 0;
        float talkAbandonTimerCo = 0;

        // "?" appear Animation
        //Set scale 0 - Start 
        questionMark.transform.localScale = Vector3.zero;

        //Make scale 1.5 - Overshoot
        while (elapsedTime < 0.5f)
        {
            questionMark.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1.5f, 1.5f, 1.5f), elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Make scale 1 - Correct
        while (elapsedTime > 0.5f && elapsedTime < 0.9f)
        {
            questionMark.transform.localScale = Vector3.Lerp(questionMark.transform.localScale, Vector3.one, elapsedTime / 0.9f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Vector3 A = new Vector3(playerAura.transform.localScale.x, playerAura.transform.localScale.y, playerAura.transform.localScale.z);
        //Vector3 B = new Vector3(0.5f, playerAura.transform.localScale.y, 0.5f);
        //Debug.Log("ABANON IN: " + talkAbandonTimer);
        while (talkAbandonTimerCo < 2)
        {
            // playerAura.transform.localScale = Vector3.Lerp(A, B, talkAbandonTimerCo/2);
            GetComponent<SliderForConversion>().timerSlider.GetComponent<CanvasGroup>().alpha = 1 - talkAbandonTimerCo / 2;
            auraOutlineSlider.GetComponent<CanvasGroup>().alpha = 1 - talkAbandonTimerCo / 2;

            talkAbandonTimerCo += Time.deltaTime;
            yield return null;
        }
        LeaveTalk();
    }

    private void OnDestroy()
    {
        References.electors.Remove(this);
        References.targetElectors.Remove(this);
    }
}
