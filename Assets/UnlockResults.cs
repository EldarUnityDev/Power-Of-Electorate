using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockResults : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UnlockResultsScene()
    {
        PlayerPrefs.SetInt("resultsReady", 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
