using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElectorBehaviour : MonoBehaviour
{
    public bool neutralMood;
    NavMeshAgent agent;

    public GameObject myBody;
    public GameObject candidate1Body;
    public GameObject candidate2Body;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
    }
    void GoToRandomNavPoint()
    {
        int randomNavPointIndex = Random.Range(0, References.spawnPoints.Count);
        agent.destination = References.spawnPoints[randomNavPointIndex].transform.position;
    }
    public void TurnMe()
    {
        myBody.SetActive(false);
        candidate1Body.SetActive(true);

    }
}
