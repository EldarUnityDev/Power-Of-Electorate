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
    private void Awake()
    {
        References.electors.Add(this);
        References.targetElectors.Add(this);

    }
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
    public void TurnMe(PlayerBehaviour playerContacted)
    {
        myBody.SetActive(false);
        if (playerContacted != null)
        {
            Debug.Log("player got me");
            candidate2Body.SetActive(true);
            if (candidate1Body.activeInHierarchy)
            {
                candidate1Body.SetActive(false);
                References.targetElectors.Add(this);
            }

        }
        else {
            candidate2Body.SetActive(false);
            candidate1Body.SetActive(true); 
        }
    }
    public void JoinTalk()
    {
        agent.enabled = false;

    }
    public void LeaveTalk()
    {
        agent.enabled = true;

    }
}
