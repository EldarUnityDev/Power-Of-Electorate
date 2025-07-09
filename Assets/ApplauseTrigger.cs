using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplauseTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayMe()
    {
        //Audio applause
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
