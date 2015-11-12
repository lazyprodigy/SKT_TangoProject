using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour 
{
    public Camera CamL;
    public Camera CamR;

    public Transform MedicalObject;
    public Transform UserSet1p;
    public Transform UserSet2p;
    //public Transform UsersHand1p;
    //public Transform UsersHand2p;

    public static int playerWhoIsIt = 0;
    string roomName = "R";
    public string GetRoomName()
    {
        return roomName;
    }

    private static PhotonView ScenePhotonView;
    CameraTriggerSystem cameraTriggerSystem;
    ScriptManager Scriptmanager;
    HandController Handcontrol;
    GazeInputModule gazeInputModule;
    //public int playerCount = 0;
    //string tDebug = "";
   
    GameObject UsersHead1p;
    GameObject UsersHead2p;
    GameObject otherUsersHand1p;
    GameObject otherUsersHand2p;
    public GameObject UsersHandL1p;
    public GameObject UsersHandR1p;
    public GameObject UsersHandL2p;
    public GameObject UsersHandR2p;
    Vector3 originPosition;
    Vector3 originRotation;
    Vector3 originScale;
    //Quaternion resultRotation;
    public RoomInfo[] roomList;
    bool currSpin = false;
    bool isObserver = false;
    public bool check = false;
    bool tempHandStatus = false;
    //extend functions-------------------------------------------------------
    void Awake()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)
        ScenePhotonView = this.GetComponent<PhotonView>();
        cameraTriggerSystem = FindObjectOfType<CameraTriggerSystem>();
        Scriptmanager = FindObjectOfType<ScriptManager>();
        Handcontrol = FindObjectOfType<HandController>();
        gazeInputModule = FindObjectOfType<GazeInputModule>();
        gazeInputModule.SetGazePointerEnable(false);
        //Debug.Log("oriPos: " + originPosition + ", oriRot: " + originRotation + ", oriScale: " + originScale);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateRoom();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            JoinRoom(false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            JoinRoom(true);
        }
        
        if (check == true)
        {
            /*
            if (PhotonNetwork.isMasterClient)
            {
                if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == true && Handcontrol.b_isRight == false)
                {
                    ScenePhotonView.RPC("HandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == false && Handcontrol.b_isRight == true)
                {
                    ScenePhotonView.RPC("HandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == true && Handcontrol.b_isRight == true)
                {
                    ScenePhotonView.RPC("HandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Delete!" && Handcontrol.b_isLeft == false && Handcontrol.b_isRight == false)
                {
                    ScenePhotonView.RPC("HandControll", PhotonTargets.Others, "Delete", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                if (Scriptmanager.strChecker == "Ready")
                {
                    ScenePhotonView.RPC("HandControll", PhotonTargets.Others, "Ready", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
            }
            else
            {
                if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == true && Handcontrol.b_isRight == false)
                {
                    ScenePhotonView.RPC("CHandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == false && Handcontrol.b_isRight == true)
                {
                    ScenePhotonView.RPC("CHandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Appear!" && Handcontrol.b_isLeft == true && Handcontrol.b_isRight == true)
                {
                    ScenePhotonView.RPC("CHandControll", PhotonTargets.Others, "Appear", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                else if (Scriptmanager.strChecker == "Delete!" && Handcontrol.b_isLeft == false && Handcontrol.b_isRight == false)
                {
                    ScenePhotonView.RPC("CHandControll", PhotonTargets.Others, "Delete", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
                if (Scriptmanager.strChecker == "Ready")
                {
                    ScenePhotonView.RPC("CHandControll", PhotonTargets.Others, "Ready", Handcontrol.b_isLeft, Handcontrol.b_isRight);
                }
            }
             */ 
            //Debug.Log("strChecker: " + Scriptmanager.strChecker);
            if (Scriptmanager.strChecker == "Appear!")
            {
                tempHandStatus = true;
                ScenePhotonView.RPC("RpcSetSpin", PhotonTargets.All, false);
            }
            else if (tempHandStatus)
            {
                ScenePhotonView.RPC("RpcSetSpin", PhotonTargets.All, true);
                tempHandStatus = false;
            }

        }
        else
        {
            if (Scriptmanager.strChecker == "Appear!")
            {
                RpcSetSpin(false);
            }
            else
            {
                RpcSetSpin(true);
            }
        }

    }
    //events----------------------------------------------------------------
    //-common--------------------------------------------------------------
    void OnConnectedToPhoton()
    {
        //Debug.Log("Connected To Photon");
        //tDebug += "Connected To Photon";
    }

    void OnFailedToConnectToPhoton()
    {
        //Debug.Log("Failed To Connect To Photon");
        //tDebug += "Failed To Connect To Photon";
    }

    void OnJoinedLobby()
    {
        //PhotonNetwork.JoinRandomRoom();
        //Debug.Log("Joined Lobby");
        //tDebug += "Joined lobby.\n";
        
    }

    public void FindPlaneComplete()
    {
        RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
        bool _isRoomExist = false;
        for (var i = 0; i < PhotonNetwork.GetRoomList().Length; ++i)
        {

            //DebugPanel.Log("Room in Lobby: ", roomInfo[i].name);
            if (roomInfo[i].name == roomName)
            {
                PhotonNetwork.JoinRoom(roomName);
                _isRoomExist = true;
            }
        }
        if (!_isRoomExist)
        {
            PhotonNetwork.CreateRoom(roomName);
        }
    }

    void OnReceivedRoomListUpdate()
    {
        //Debug.Log("list Update");
        roomList = PhotonNetwork.GetRoomList();
        //DebugPanel.Log("room List Length: ", PhotonNetwork.GetRoomList().Length);
        //Debug.Log("roomList Length: " + PhotonNetwork.GetRoomList().Length);
        //GameObject.Find("ARCamera").GetComponent<CameraTriggerSystem>().SetRoomList(roomList);
    }
    void OnJoinedRoom()
    {
        playerWhoIsIt = PhotonNetwork.player.ID;

        //Debug.Log("Joined Room: " + playerWhoIsIt);
        //tDebug += "Player " + PhotonNetwork.player.ID + " has joined room.\n";
        //tDebug += "Room Name: " + roomName + "\n";
        if (PhotonNetwork.player.ID >= 3)
        {
            isObserver = true;
        }
        StartGame();
    }
    void OnPhotonCreateRoomFailed()
    {
        //Debug.Log("Create Room Failed");
        //tDebug += "Create Room Failed | Room List Count: " + PhotonNetwork.GetRoomList().Length;
        //if (PhotonNetwork.GetRoomList().Length <= 1) PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.CreateRoom(null);
    }

    void OnPhotonJoinRoomFailed()
    {
        //Debug.Log("OnPhotonJoinRoomFailed");
        //tDebug += "join room failed: " + roomName + "\n";
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    
    //-master---------------------------------------------------
    void OnCreatedRoom()
    {
        //tDebug += "Player " + PhotonNetwork.player.ID + " has created room. This player will be MasterClient.\n";
        //tDebug += "RoomName: " + roomName + "\n";
        //GameObject brainObj = PhotonNetwork.Instantiate("brain_real", Vector3.zero, Quaternion.identity, 0);
    }
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        //Debug.Log("OnPhotonPlayerConnected: " + player);
        //tDebug += "Player Connected: " + player + "\n";
        // when new players join, we send "who's it" to let them know
        // only one player will do this: the "master"
        
        if (PhotonNetwork.isMasterClient)
        {
            TagPlayer(playerWhoIsIt);
        }

    }

    void OnDisconnectedFromPhoton()
    {
        //Debug.LogWarning("OnDisconnectedFromPhoton");
        //tDebug += "OnDisconnectedFromPhoton";
    }
    
    //RPC----------------------------------------------------------
    [PunRPC]
    public void HandControll(string Hand, bool left, bool right)
    {
        Debug.Log("Hand: " + Hand + ", left: " + left + ", right: " + right);
        //tDebug = "CHand: " + Hand + ", left: " + left + ", right: " + right;
        if (Hand == "Ready")
        {
            UsersHandL1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        if (Hand == "Appear" && left == true && right == false)
        {
            UsersHandL1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            UsersHandR1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        else if (Hand == "Appear" && left == false && right == true)
        {
            UsersHandL1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (Hand == "Appear" && left == true && right == true)
        {
            UsersHandL1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            UsersHandR1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (Hand == "Delete" && left == false && right == false)
        {
            UsersHandL1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR1p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        
    }
    [PunRPC]
    public void CHandControll(string Hand, bool left, bool right)
    {
        Debug.Log("CHand: " + Hand + ", left: " + left + ", right: " + right);
        //tDebug = "CHand: " + Hand + ", left: " + left + ", right: " + right;
        if (Hand == "Ready")
        {
            UsersHandL2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        if (Hand == "Appear" && left == true && right == false)
        {
            UsersHandL2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            UsersHandR2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        else if (Hand == "Appear" && left == false && right == true)
        {
            UsersHandL2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (Hand == "Appear" && left == true && right == true)
        {
            UsersHandL2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            UsersHandR2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (Hand == "Delete" && left == false && right == false)
        {
            UsersHandL2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            UsersHandR2p.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        
    }
    [PunRPC]
    public void TaggedPlayer(int playerID)
    {
        playerWhoIsIt = playerID;
        //Debug.Log("TaggedPlayer: " + playerID);
        //tDebug += "TaggedPlayer: " + playerID;
    }
    
    [PunRPC]
    public void PlayerConnect()
    {
        UsersHead2p.SetActive(true);
        check = true;
    }


    [PunRPC]
    //public void RpcCameraInfo(int playerID, Vector3 pos, Quaternion rot, Vector3 planePos)
    public void RpcCameraInfo(int playerID, Vector3 pos, Quaternion rot)
    {
        //Debug.Log("RpcCameraInfo | id: " + playerID + ", pos: " + pos + ", rot: " + rot);
        if (PhotonNetwork.player.ID == playerID) return;
        if (playerID == 2)
        {
            if (UserSet2p == null) return;

            UsersHead2p.GetComponent<OtherUsersHead>().SetCameraPosition(pos, rot);
        }
        else if (playerID == 1)
        {
            if (UserSet1p == null) return;
            UsersHead1p.GetComponent<OtherUsersHead>().SetCameraPosition(pos, rot);
        }
        else return;
    }
    
    [PunRPC]
    void RpcSetSpin(bool spin)
    {
        if (FindObjectOfType<ClickForRotation>() == null) return;
        spin = !spin;
        //Debug.Log("RpcSetSpin: " + spin);
        currSpin = spin;
        
        FindObjectOfType<ClickForRotation>().SendMessage("SetCurrentSpin", currSpin);
    }
    //coroutine-------------------------------------------------------
    IEnumerator OnLeftRoom()
    {
       
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        Application.LoadLevel(Application.loadedLevel);

    }
    void StartGame()
    {
        //GameObject brainObj = PhotonNetwork.Instantiate("brain_real", Vector3.zero, Quaternion.identity, 0);
        GameObject brainObj = Instantiate(MedicalObject.gameObject);
        cameraTriggerSystem.SetPositionToPlane(brainObj.transform);
        
        Vector3 dir = brainObj.transform.position - cameraTriggerSystem.transform.position;
        dir.y = 0;
        dir.Normalize();
        brainObj.transform.rotation = Quaternion.LookRotation(dir);
        if (PhotonNetwork.isMasterClient)
        {

            //UserSet2p = PhotonNetwork.Instantiate("OtherUsersHead", Vector3.zero, Quaternion.identity, 0);
            //UsersHand2p = PhotonNetwork.Instantiate("OtherUsersHand", Vector3.zero, Quaternion.identity, 0);
            UsersHead2p = Instantiate(UserSet2p.gameObject);
            //otherUsersHand2p = Instantiate(UsersHand2p.gameObject);
            UsersHead2p.SetActive(false);
            //otherUsersHand2p.SetActive(false);
            UsersHandL2p = UsersHead2p.transform.GetChild(1).GetChild(0).gameObject;
            UsersHandR2p = UsersHead2p.transform.GetChild(2).GetChild(0).gameObject;
            
        }
        else if(!isObserver)
        {
            ScenePhotonView.RPC("PlayerConnect", PhotonTargets.MasterClient);
            //UserSet1p = PhotonNetwork.Instantiate("OtherUsersHead", Vector3.zero, Quaternion.identity, 0);
            //UsersHand1p = PhotonNetwork.Instantiate("OtherUsersHand", Vector3.zero, Quaternion.identity, 0);
            UsersHead1p = Instantiate(UserSet1p.gameObject);
            //otherUsersHand1p = Instantiate(UsersHand1p.gameObject);
            UsersHandL1p = UsersHead1p.transform.GetChild(1).GetChild(0).gameObject;
            UsersHandR1p = UsersHead1p.transform.GetChild(2).GetChild(0).gameObject;
            check = true;
        }
        else
        {
            UsersHead1p = Instantiate(UserSet1p.gameObject);
            //otherUsersHand1p = Instantiate(UsersHand1p.gameObject);
            UsersHead2p = Instantiate(UserSet2p.gameObject);
            //otherUsersHand2p = Instantiate(UsersHand2p.gameObject);
            UsersHead2p.SetActive(true);
            //otherUsersHand2p.SetActive(true);
            UsersHandL1p = UsersHead1p.transform.GetChild(1).GetChild(0).gameObject;
            UsersHandR1p = UsersHead1p.transform.GetChild(2).GetChild(0).gameObject;
            UsersHandL2p = UsersHead2p.transform.GetChild(1).GetChild(0).gameObject;
            UsersHandR2p = UsersHead2p.transform.GetChild(2).GetChild(0).gameObject;
            //check = true;
        }
    }
    //public functions---------------------------------------------------------------------------------------------
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 10 }, TypedLobby.Default);
    }

    public void JoinRoom(bool _isObserver)
    {
        isObserver = _isObserver;
        PhotonNetwork.JoinRoom(roomName);
    }
    public void myCameraInfo()
    {
        ScenePhotonView.RPC("RpcCameraInfo", PhotonTargets.All, PhotonNetwork.player.ID, GetMirrorPosition(), GetMirrorRotation());
    }
    
    //getter/setter------------------------------------------------
    public int GetCountOfPlayers()
    {
        return PhotonNetwork.countOfPlayersInRooms;
    }
    public int GetPlayerID()
    {
        return PhotonNetwork.player.ID;
    }

    public static void TagPlayer(int playerID)
    {
        //Debug.Log("TagPlayer: " + playerID);
        ScenePhotonView.RPC("TaggedPlayer", PhotonTargets.All, playerID);
    }
   
    
#if false
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 200, 10, 200, 20), tDebug);
        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room

        GUILayout.BeginArea(new Rect((Screen.width - 800) / 2, (Screen.height - 300) / 2, 800, 300));

        GUILayout.Label("<size=30>Main Menu</size>");

        //Player name
        GUILayout.BeginHorizontal();
        GUILayout.Label("<size=30>Player name:</size>", GUILayout.Width(400));
        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
        if (GUI.changed)//Save name
            PlayerPrefs.SetString("<size=30>playerName</size>", PhotonNetwork.playerName);
        GUILayout.EndHorizontal();

        GUILayout.Space(30);


        //Join room by title
        GUILayout.BeginHorizontal();
        GUILayout.Label("<size=30>Join Room By Player Mode:</size>", GUILayout.Width(400));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("<size=30>GO</size>"))
        {
            
            isObserver = false;
            PhotonNetwork.JoinRoom(roomName);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("<size=30>Join Room By Observer Mode:</size>", GUILayout.Width(400));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("<size=30>GO</size>"))
        {
            isObserver = true;
            PhotonNetwork.JoinRoom(roomName);
        }
        GUILayout.EndHorizontal();

        //Create a room (fails if exist!)
        GUILayout.BeginHorizontal();
        GUILayout.Label("<size=30>Create Room:</size>", GUILayout.Width(400));
        roomName = GUILayout.TextField(roomName);
        if (GUILayout.Button("<size=30>GO</size>"))
        {
            // using null as TypedLobby parameter will also use the default lobby
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 10 }, TypedLobby.Default);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
#endif
    string strInput = "R";
    bool b_ui = true;

    bool b_camera = true;

    public void CameraSwitch()
    {
        b_camera ^= true;

        if (b_camera)
        {
            CamR.enabled = true;
            CamL.rect = new Rect(0.1f, 0.1f, 0.4f, 0.8f);
            CamR.rect = new Rect(0.5f, 0.1f, 0.4f, 0.8f);

            AugmentedRealityGUIController.Instance.t = new Vector2(575, 595);
        }
        else
        {
            CamR.enabled = false;
            CamL.rect = new Rect(0f, 0f, 1f, 1f);

            AugmentedRealityGUIController.Instance.t = new Vector2(965, 595);
        }
    }

    void OnGUI()
    {
        if (b_ui)
        {
            //GUI.Label(new Rect(Screen.width - 400, 10, 400, Screen.height), "<size=25><color=#ffffff>" + tDebug + "</color></size>");// 오른쪽 상단
            strInput = GUI.TextField(new Rect((Screen.width - 300) / 2, Screen.height - 200, 300, 200), strInput);
            if (GUI.Button(new Rect(Screen.width - 300, Screen.height - 100, 100, 100), "Ready"))
            {
                roomName = strInput;
            }
            if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 100, 100, 100), "Camera"))
            {
                CameraSwitch();
            }
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), "Start"))
            {
                b_ui = false;
                AugmentedRealityGUIController.Instance.ShowHideRader(true, b_camera);
                               
            }

        }

    }

    Vector3 GetMirrorPosition()
    {
        Vector3 tcPos = CamL.GetComponent<TangoDeltaPoseControllerEx>().GetTangoCameraPosition();
        Vector3 fpPos = AugmentedRealityGUIController.Instance.GetPlaneCenterPosition();
        Vector3 distPos = new Vector3(Mathf.Abs(tcPos.x - fpPos.x), Mathf.Abs(tcPos.y - fpPos.y), Mathf.Abs(tcPos.z - fpPos.z));
        Vector3 rPos = new Vector3(fpPos.x - distPos.x, fpPos.y + distPos.y, fpPos.z + distPos.z);

        return rPos;
    }

    Quaternion GetMirrorRotation()
    {
        Quaternion tcRot = CamL.GetComponent<TangoDeltaPoseControllerEx>().GetTangoCameraRotation();
        Quaternion invRot = Quaternion.Inverse(tcRot);
        Quaternion rRot = new Quaternion(tcRot.x, tcRot.y, tcRot.z, invRot.w);

        return rRot;
    }
     
}
