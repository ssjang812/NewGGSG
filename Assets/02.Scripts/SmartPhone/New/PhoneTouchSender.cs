using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTouchSender : MonoBehaviour
{
    public PhotonView PV;
    float timer = 0.0f;
    bool isPointerDown = false;
    public float pressThreshHold = 1f;
    //int seconds = 0;

    private void Update()
    {
        if(isPointerDown)
        {
            timer += Time.deltaTime;
        }
        if(timer > pressThreshHold)
        {
            Debug.Log("1 sec!!!!");
            timer = 0;
        }
        
    }
    public void OnPointerDown()
    {
        PV.RPC("RPC_OnPointerDown", RpcTarget.All);
        timer = 0.0f;
        isPointerDown = true;
    }
    public void OnPointerUp()
    {
        PV.RPC("RPC_OnPointerUp", RpcTarget.All);
        timer = 0.0f;
        isPointerDown = false;
    }

    public void OnClickDebug()
    {
        PV.RPC("RPC_OnClickDebug", RpcTarget.All);
    }
}
