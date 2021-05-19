﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTouchSender : MonoBehaviour
{
    public PhotonView PV;

    public void OnChairButtonDown()
    {
        PV.RPC("RPC_OnChairButtonDown", RpcTarget.All);
    }
    public void OnChairButtonUp()
    {
        PV.RPC("RPC_OnChairButtonUp", RpcTarget.All);
    }

    public void OnPointerDown()
    {
        PV.RPC("RPC_OnPointerDown", RpcTarget.All);
    }

    public void OnPointerUp()
    {
        PV.RPC("RPC_OnPointerUp", RpcTarget.All);
    }
}
