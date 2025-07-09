using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VanScript : MonoBehaviour
{
    public Animator animationController;
    private float timer;
    private bool startTimer;
    public bool needTimer;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = 1;
    }
    public void CloseDoor()
    {
        if (needTimer)
        {
            startTimer = true;
        }
        else
        {
            animationController.SetBool("DoorOpen", true);
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            //audioSource.volume = 0;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (startTimer){
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                startTimer = false;
                animationController.SetBool("DoorOpen", true);
            }
        }
    }
}
