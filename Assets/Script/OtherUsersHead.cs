using UnityEngine;
using System.Collections;

public class OtherUsersHead : MonoBehaviour {
    GameManager gameManager;
    PhotonView ScenePhotonView;
    int _playerID;
	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        _playerID = gameManager.GetPlayerID();
        ScenePhotonView = gameManager.GetComponent<PhotonView>();
       
	}
	
	// Update is called once per frame
	void Update () 
    {
        gameManager.myCameraInfo();
	}

   // public void SetCameraPosition(Vector3 pos, Quaternion rot, Vector3 distancePos)
    public void SetCameraPosition(Vector3 pos, Quaternion rot)
    {
        //transform.position = GetCalculatedPosition(pos, distancePos);
        //transform.position = pos + distancePos;
        transform.position = pos;
        transform.rotation = rot;
    }
    /*
    Vector3 GetCalculatedPosition(Vector3 pos, Vector3 distancePos)
    {
        Vector3 planeCenterPos = AugmentedRealityGUIController.Instance.GetPlaneCenterPosition();
        float tempPosX = planeCenterPos.x + pos.x + distancePos.x;
        float tempPosY = planeCenterPos.y + pos.y + distancePos.y;
        float tempPosZ = planeCenterPos.z + pos.z + distancePos.z;
        Vector3 newPos = new Vector3(tempPosX, tempPosY, tempPosZ);
        DebugPanel.Log("planeCenterPos: ", planeCenterPos);
        DebugPanel.Log("originPos: ", pos);
        DebugPanel.Log("calculatedPos: ", newPos);
        return newPos;
    }
     */ 
}
