using UnityEngine;
using System.Collections;

public class LerpTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public float tt;

    public GameObject objPivot;
    public GameObject planeCenter;

	// Update is called once per frame
	void Update () 
    {

        Vector3 temp = objPivot.transform.position;
        objPivot.transform.position = Vector3.Lerp(temp, planeCenter.transform.position, tt);
	
	}
}
