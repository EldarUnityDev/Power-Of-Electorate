using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotingPostScript : MonoBehaviour
{
    private void Awake()
    {
    }
    private void Start()
    {
        References.votingPosts.Add(gameObject);
    }
}
