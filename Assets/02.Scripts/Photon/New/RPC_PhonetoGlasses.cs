using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC_PhonetoGlasses : MonoBehaviour
{
    public GameObject debugCube1;
    public GameObject debugCube2;

    public static Vector3 swipeDelta;
    public static Vector3 gyroDelta;

    public static UnityEvent event_SyncSwipeDelta;
    public static UnityEvent event_SyncGyroDelta;

    void Start()
    {
        if (event_SyncSwipeDelta == null)
            event_SyncSwipeDelta = new UnityEvent();
        if (event_SyncGyroDelta == null)
            event_SyncGyroDelta = new UnityEvent();
    }

    [PunRPC]
    void RPC_SyncSwipeDelta(Vector3 input)
    {
        swipeDelta = input;
        //Debug.Log("RPC_SyncSwipeDelta :" + swipeDelta);
        debugCube1.transform.Translate(swipeDelta);
    }

    [PunRPC]
    void RPC_SyncGyroDelta(Vector3 input)
    {
        gyroDelta = input;
        //Debug.Log("RPC_SyncGyroDelta :" + gyroDelta);
        debugCube2.transform.Translate(gyroDelta);
    }
    
    [PunRPC]
    void RPC_OnClickDebug()
    {
        Debug.Log("OnClick!");
    }
}
