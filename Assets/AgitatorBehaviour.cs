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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        talkTime = 2;
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

        /*int randomElectorIndex = Random.Range(0, References.electors.Count);
        agent.destination = References.electors[randomElectorIndex].transform.position;*/


    }
    // Update is called once per frame
    void Update()
    {
        if(!targetAcquired) //���� ��� ����
        {
            ChooseTarget(); //������
        }

        if (targetAcquired) //���� ���� ����
        {
            if (agent.enabled)
            {
                agent.destination = myTarget.transform.position; //��������� ����� ����������, ������ ��� ���� ��������
            }
            if (!talking && Vector3.Distance(transform.position, myTarget.transform.position) < 1.3f) //���� �� ������� � �� ���������� ������
            {
                talking = true; //��������� ������ �� ��������
                                //startTurning{agent.enabled = false;}
                agent.enabled = false;
                myTarget.JoinTalk(null);
                
            }
            if(talking)
            {
                talkTime -= Time.deltaTime;
                if (talkTime < 0)
                {
                    myTarget.TurnMe(null);
                    targetAcquired = false;
                    References.targetElectors.Remove(myTarget); //������ ��� �� ����
                    myTarget.LeaveTalk();

                    myTarget = null;    //�������� ����
                    talkTime = 2;  //���������� ������ ��� ���������� ���������
                    talking = false; // ��������� ��������
                    agent.enabled = true;
                }
            }
        }
        if (References.targetElectors.Count == 0)
        {
            agent.enabled = false;
            transform.position += Vector3.up * Time.deltaTime;
        }
    }
}
