using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mapPoint;    
    public GameObject mapPlayer;
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
                    References.currentlyHighlightedPad = null;
                }

                mapPoint.SetActive(true);
                References.currentlyHighlightedPad = gameObject;

            }
            else
            {
                mapPlayer.GetComponent<MapPlayer>().MoveToPad(mapPoint);
                mapPoint.SetActive(false);
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
