using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheFaithfulPush : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blackScreen;
    public GameObject thePushed;
    public GameObject pusher;
    Rigidbody myRigidbody;
    public float knockBackForce;
    public float upForce;
    bool turning;
    public float turnTimer;
    public GameObject outline;
    public GameObject buttonToDisable;
    public int counterForOutline;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        BlackScreenOff();
        counterForOutline = 0;
    }
    public void BlackScreenOn()
    {
        blackScreen.SetActive(true);
    }
    public void BlackScreenOff()
    {
        blackScreen.SetActive(false);
    }
    public void LineCounter()
    {
        counterForOutline++;
        if(counterForOutline == 5)
        {
            outline.SetActive(true);
        }
    }
    public void PunchSequence()
    {
        //BlackScreenOn();
        //audioSource ON
        //Player.AddForce
        buttonToDisable.SetActive(false);
        outline.SetActive(false);
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        myRigidbody.AddForce((transform.position - pusher.transform.position).normalized * knockBackForce + transform.up * upForce); //-player position
        turning = true;
    }
    public void OutlineSwitch()
    {
        if (outline.activeInHierarchy)
        {
            outline.SetActive(false);
        }
        if (!outline.activeInHierarchy)
        {
            outline.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (turning && turnTimer > 0)
        {
            transform.LookAt(transform.position + transform.forward + transform.up * Time.deltaTime * 2);
            turnTimer -= Time.deltaTime;
        }
    }
}
