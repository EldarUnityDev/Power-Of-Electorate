using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LeaveArea : MonoBehaviour
{
    public GameObject myBody;
    // Start is called before the first frame update
    void Start()
    {
        References.leaveAreaPoints.Add(this);
        myBody.GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnDestroy()
    {
        References.leaveAreaPoints.Remove(this);
    }
}
