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
    public static UnityEvent event_OnPointerUp;

    void Start()
    {
        if (event_SyncSwipeDelta == null)
            event_SyncSwipeDelta = new UnityEvent();
        if (event_SyncGyroDelta == null)
            event_SyncGyroDelta = new UnityEvent();
        if (event_OnPointerUp == null)
            event_OnPointerUp = new UnityEvent();
    }

    [PunRPC]
    void RPC_SyncSwipeDelta(Vector3 input)
    {
        swipeDelta = input;
        event_SyncSwipeDelta.Invoke();
        //Debug.Log("RPC_SyncSwipeDelta :" + swipeDelta);
    }

    [PunRPC]
    void RPC_SyncGyroDelta(Vector3 input)
    {
        gyroDelta = input;
        event_SyncGyroDelta.Invoke();
        //Debug.Log("RPC_SyncGyroDelta :" + gyroDelta);
    }

    [PunRPC]
    void RPC_OnPointerDown()
    {
    }

    [PunRPC]
    void RPC_OnPointerUp()
    {
        event_OnPointerUp.Invoke();
        Debug.Log("OnPointerUp!");
    }

    [PunRPC]
    void RPC_OnClickDebug()
    {
    }
}
