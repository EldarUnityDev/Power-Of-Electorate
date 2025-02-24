using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mapPoint;    
    public GameObject mapPlayer;
    public GameObject startLevelButton;
    public string levelToStartName; 
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(mapPoint != null && mapPoint!= References.currentPad)
        {
            if (!mapPoint.activeInHierarchy)
            {
                //turn off current outline
                if (References.currentlyHighlightedPad != null)
                {
                    References.currentlyHighlightedPad.GetComponent<EventClick>().mapPoint.SetActive(false);
                    startLevelButton.SetActive(false);

                    References.currentlyHighlightedPad = null;
                }

                mapPoint.SetActive(true);
                References.currentlyHighlightedPad = gameObject;

            }
            else
            {
                mapPlayer.GetComponent<MapPlayer>().MoveToPad(mapPoint);
                mapPoint.SetActive(false);
                startLevelButton.SetActive(true);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //
    }

    // Start is called before the first frame update
    private void Awake()
    {
        References.pads.Add(gameObject);

    }
    private void OnDestroy()
    {
        References.pads.Remove(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
