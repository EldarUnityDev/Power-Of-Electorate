using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FighterScript : MonoBehaviour
{
    public FighterScript myTarget;
    public bool targetAcquired;
    NavMeshAgent agent;
    public GameObject blueBody;
    public GameObject redBody;
    Rigidbody ourRigidBody;
    public bool knockedOut;
    public float knockedOutTime;

    void Start()
    {
        knockedOutTime = 1;
        agent = GetComponent<NavMeshAgent>();
        ourRigidBody = GetComponent<Rigidbody>();
        if (blueBody.activeInHierarchy != false)
        {
            References.blueFighters.Add(this);
        }
        else
        {
            References.redFighters.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<PlayerBehaviour>() == null && !targetAcquired)
        {
            ChooseTarget();
        }
        if (targetAcquired) //���� ���� ����
        {
            if (agent.enabled)
            {
                if(myTarget != null)
                {
                    agent.destination = myTarget.transform.position; //��������� ����� ����������, ������ ��� ���� ��������
                }
            }
        }
        if (knockedOut)
        {
            knockedOutTime -= Time.deltaTime;
            if (knockedOutTime < 0)
            {
                agent.enabled = true;
                ourRigidBody.isKinematic = false;
                knockedOut = false;
                knockedOutTime = 1;
            }
        }

        //� ����� ���� ����� ��� ������ �� ������
        if (agent.enabled && References.fightEnded && agent.remainingDistance < 2)
        {
            GoToRandomNavPoint();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<FighterScript>() != null && !References.fightEnded)
        {
            bool touchedEnemy = false;
            if (blueBody.activeInHierarchy && collision.gameObject.GetComponent<FighterScript>().redBody.activeInHierarchy)
            {
                touchedEnemy = true;
            }
            if (redBody.activeInHierarchy && collision.gameObject.GetComponent<FighterScript>().blueBody.activeInHierarchy)
            {
                touchedEnemy = true;
            }
            if (touchedEnemy)
            {
                //������� ������
                agent.enabled = false;
                ourRigidBody.isKinematic = false;
                ourRigidBody.constraints = RigidbodyConstraints.None;
                ourRigidBody.AddForce((-transform.forward * 800) + transform.up * 10);
                knockedOut = true;
                targetAcquired = false;

                //����� ��������� ����� �����
                float totalNumberOfFighters = References.blueFighters.Count + References.redFighters.Count;
                float chance;
                if (blueBody.activeInHierarchy != false)
                {
                    chance = References.blueFighters.Count / totalNumberOfFighters;
                }
                else
                {
                    chance = References.redFighters.Count / totalNumberOfFighters;
                }

                if (UnityEngine.Random.value < chance) //��������� �������� ������ ������� � ��� ����
                {
                    if (collision.gameObject.GetComponentInParent<PlayerBehaviour>() != null)
                    {
                        Debug.Log("PLAYER DIED");
                        collision.gameObject.GetComponentInParent<PlayerBehaviour>().enabled = false;
                    }
                    collision.gameObject.GetComponent<FighterScript>().SwitchTeams();
                }
            }
        }
    }
    public void SwitchTeams()
    {
        if (blueBody.activeInHierarchy != false) //���� � �����
        {
            References.blueFighters.Remove(this); //� ������ �� �����
            blueBody.SetActive(false);            //���� �� �� �����
            redBody.SetActive(true);              //�� ���� �������
            References.redFighters.Add(this);    //� ������ �������

        }
        else
        {
            References.redFighters.Remove(this);
            blueBody.SetActive(true);
            redBody.SetActive(false);
            References.blueFighters.Add(this);
        }
    }
    public void ChooseTarget()
    {
        FighterScript nearestOpponentSoFar = null;
        float nearestDistance = 20; //max search distance
        List<FighterScript> oppontentsList;

        if (blueBody.activeInHierarchy != false)
        {
            oppontentsList = References.redFighters;
        }
        else
        {
            oppontentsList = References.blueFighters;
        }

        foreach (FighterScript thisFighter in oppontentsList)
        {
            if (thisFighter != null)
            {
                //how far is this one from the agitator?
                float thisDistance = Vector3.Distance(transform.position, thisFighter.transform.position);
                //is it closer than anything else we've found?
                if (thisDistance <= nearestDistance)
                {
                    //if it's THIS now it's the closest one
                    nearestOpponentSoFar = thisFighter;
                    nearestDistance = thisDistance;
                }
            }
        }

        if (nearestOpponentSoFar != null && agent.enabled)
        {
            //References.canvas.usePromptSignal = true; 
            {
                agent.destination = nearestOpponentSoFar.transform.position;
                myTarget = nearestOpponentSoFar;
                targetAcquired = true;
            }
        }
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = UnityEngine.Random.Range(0, References.spawnPoints.Count);
        if (References.spawnPoints[randomNavPointIndex] != null) //����� �� ���� ������ �� ��������
        {
            agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
        }
    }
    private void OnDestroy()
    {
        References.blueFighters.Remove(this);
        References.redFighters.Remove(this);

    }
}
