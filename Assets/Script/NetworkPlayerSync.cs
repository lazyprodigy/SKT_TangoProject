using UnityEngine;
using System.Collections;


public class NetworkPlayerSync : Photon.MonoBehaviour
{
    //참조할 컴포넌트를 할당할 변수
    private Transform tr;
    //PhotonView 컴포넌트를 할당할 변수
    private PhotonView pv = null;
    //위치 정보를 송수신할 때 사용할 변수 선언 및 초깃값 설정
    //private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;
    
    void Awake()
    {
        //컴포넌트 할당
        tr = GetComponent<Transform>();
        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
        //데이터 전송 타입을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        //PhotonView Observed Components 속성에 NetworkPlayerSync 스크립트를 연결
        pv.ObservedComponents[0] = this;

        //원격 캐릭터 위치 및 회전 값을 처리할 변수의 초깃값 설정
        //currPos = tr.position;
        currRot = tr.rotation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 송신
        if (stream.isWriting)
        {
            //stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else//원격 플레이어의 위치 정보 수신
        {
            //currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        //원격 플레이어 일 때 수행
        if (!pv.isMine)
        {
            //원격 플레이어의 캐릭터를 수신 받은 위치까지 부드럽게 이동시킴
           // tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            //원격 플레이어의 캐릭터를 수신 받은 각도만큼 부드럽게 회전시킴
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

}