using UnityEngine;
using System.Collections;

public class TangoDeltaPoseControllerEx : TangoDeltaPoseController, ITangoPose {

    public Vector3 GetTangoCameraPosition()
    {
        return transform.position;
    }

    public Quaternion GetTangoCameraRotation()
    {
        return transform.rotation;
    }

}
