using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotingPostScript : MonoBehaviour
{
    private void Awake()
    {
        References.votingPosts.Add(gameObject);
    }
}
