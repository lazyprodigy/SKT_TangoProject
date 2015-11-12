using UnityEngine;
using System.Collections;

public class ClickForRotation : MonoBehaviour {
    GameManager gameManager;
    bool spin = false;
    bool currSpin = false;
    float speed = 10.0f;
    string tDebug = "";
	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        spin = currSpin;
        if (spin)
        {
            transform.Rotate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        }
	}
    /*
    [PunRPC]
    void RpcSetSpin(bool spin)
    {
        currSpin = spin;
    }
     */ 
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 400, Screen.height / 2, 400, Screen.height / 2), "<size=25><color=#ffffff>" + tDebug + "</color></size>");// 오른쪽 중단
        string buttonText = "";
        if (spin)
        {
            buttonText = "Stop spinning";
        }
        else
        {
            buttonText = "Spin your Brain";
        }
    }

    public void SetCurrentSpin(bool spin)
    {
        //Debug.Log("SetCurrentSpin: " + spin);
        currSpin = spin;
    }

}
