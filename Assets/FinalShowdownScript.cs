using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShowdownScript : MonoBehaviour
{
    public float timeBeforeShowdown;
    public GameObject RedTeamFighter;
    public GameObject BlueTeamFighter;
    public bool fightersSpawned;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (References.electors.Count == 0)
        {
            timeBeforeShowdown -= Time.deltaTime;
            if (timeBeforeShowdown < 0)
            {
                if (!fightersSpawned)
                {
                    SpawnFighters();
                }
                fightersSpawned = true;

            }
        }
    }
    public void SpawnFighters()
    {
        {
            for (int i = 0; i > References.pointsForPlayerCandidate; i++)
            {
                Instantiate(BlueTeamFighter, References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody.transform.position, Quaternion.identity);
            }

            for (int i = 0; i > References.pointsForOppositeCandidate; i++)
            {
                Instantiate(RedTeamFighter, References.leaveAreaPoints[Random.Range(0, References.leaveAreaPoints.Count)].myBody.transform.position, Quaternion.identity);
            }
        }
    }
}

