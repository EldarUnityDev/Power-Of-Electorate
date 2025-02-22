using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject spawnPoint;
    private void OnEnable()
    {
        References.spawnPoints.Add(this);
    }
    void Start()
    {
        spawnPoint.GetComponent<MeshRenderer>().enabled = false;
    }
}
