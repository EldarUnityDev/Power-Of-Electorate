using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingHead : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainCam;
    void Start()
    {
    }
    public void Appear()
    {

        transform.LookAt(mainCam.transform.position);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
