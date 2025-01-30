using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Useable : MonoBehaviour
{

    public UnityEvent whenUsed;
    public bool canBeReused;
    public string displayName;
    public void Use()
    {
        whenUsed.Invoke();
        if (canBeReused == false)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        References.useables.Add(this);
    }
    // Update is called once per frame
    private void OnDisable()
    {
        References.useables.Remove(this);

    }

}