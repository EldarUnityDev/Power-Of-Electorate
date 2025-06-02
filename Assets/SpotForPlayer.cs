using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotForPlayer : MonoBehaviour
{
    public GameObject playerSpot;
    private void OnEnable()
    {
        References.playerSpot = gameObject;
    }
    void Start()
    {
        playerSpot.GetComponent<MeshRenderer>().enabled = false;
    }
}
