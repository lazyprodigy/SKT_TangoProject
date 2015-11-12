using UnityEngine;
using System.Collections;


public class NetworkPlayerSync : Photon.MonoBehaviour
{
    //������ ������Ʈ�� �Ҵ��� ����
    private Transform tr;
    //PhotonView ������Ʈ�� �Ҵ��� ����
    private PhotonView pv = null;
    //��ġ ������ �ۼ����� �� ����� ���� ���� �� �ʱ갪 ����
    //private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;
    
    void Awake()
    {
        //������Ʈ �Ҵ�
        tr = GetComponent<Transform>();
        //PhotonView ������Ʈ �Ҵ�
        pv = GetComponent<PhotonView>();
        //������ ���� Ÿ���� ����
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        //PhotonView Observed Components �Ӽ��� NetworkPlayerSync ��ũ��Ʈ�� ����
        pv.ObservedComponents[0] = this;

        //���� ĳ���� ��ġ �� ȸ�� ���� ó���� ������ �ʱ갪 ����
        //currPos = tr.position;
        currRot = tr.rotation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���� �÷��̾��� ��ġ ���� �۽�
        if (stream.isWriting)
        {
            //stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else//���� �÷��̾��� ��ġ ���� ����
        {
            //currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        //���� �÷��̾� �� �� ����
        if (!pv.isMine)
        {
            //���� �÷��̾��� ĳ���͸� ���� ���� ��ġ���� �ε巴�� �̵���Ŵ
           // tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            //���� �÷��̾��� ĳ���͸� ���� ���� ������ŭ �ε巴�� ȸ����Ŵ
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

}