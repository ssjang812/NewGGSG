using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneButtonSender : MonoBehaviour
{
    public PhotonView PV;

    public void OnPointerDown()
    {
        PV.RPC("RPC_OnPointerDown", RpcTarget.All);
    }
    public void OnPointerUp()
    {
        PV.RPC("RPC_OnPointerUp", RpcTarget.All);
    }

    public void OnClickDebug()
    {
        PV.RPC("RPC_OnClickDebug", RpcTarget.All);
    }
}
