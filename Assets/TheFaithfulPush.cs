using GLTF.Schema;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject prompt;
    public GameObject finalText;
    public TextMeshProUGUI text;
    public bool cutsceneEnded;
    public bool goodbyeShown;
    AudioSource audioSource;
    AudioSource tunkAudio;
    public GameObject tunkContainer;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tunkAudio = tunkContainer.GetComponent<AudioSource>();
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
        tunkAudio.Play();
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
        if(turning && turnTimer <= 0)
        {
            StartCoroutine(CutsceneCoroutine());
            turning = false;
        }
        if (cutsceneEnded)
        {
            if (Input.GetButtonDown("Menu") || Input.GetButtonDown("Use") || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
            {
                if (!goodbyeShown)
                {
                    text.text = "THANKS FOR PLAYING";
                    goodbyeShown = true;
                }
                else
                {
                    References.levelManager.MainMenu();
                }
            }
        }
    }
    IEnumerator CutsceneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        BlackScreenOn();
        finalText.SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(1);
        prompt.SetActive(true);
        cutsceneEnded = true;
    }
}
