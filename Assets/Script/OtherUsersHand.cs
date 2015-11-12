using UnityEngine;
using System.Collections;

public class OtherUsersHand : MonoBehaviour {
    PhotonView ScenePhotonView;
    GameManager gameManager;
    int _playerID;
    Vector3 originPosition;
    Vector3 originRotation;
    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _playerID = gameManager.GetPlayerID();
        GameObject handController = FindObjectOfType<HandController>().gameObject;
        originPosition = handController.transform.localPosition;
        originRotation = handController.transform.localEulerAngles;
        ScenePhotonView = gameManager.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHandPosition(Vector3 pos, Quaternion rot)
    {
        transform.position = originPosition + pos;
        Vector3 eulerRot = -originRotation + Quaternion.ToEulerAngles(rot);
        transform.rotation = Quaternion.Euler(eulerRot);
    }

}
