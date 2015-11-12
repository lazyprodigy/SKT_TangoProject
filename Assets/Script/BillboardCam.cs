using UnityEngine;
using System.Collections;

public class BillboardCam : MonoBehaviour
{
    	
	void Start () 
    {
	
	}
	
	
	void Update () 
    {
        if (Camera.main != null)

            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
            Camera.main.transform.rotation * Vector3.up);

            //transform.LookAt(Camera.main.transform);
	}
}
