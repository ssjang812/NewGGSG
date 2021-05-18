using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC_PhonetoGlasses : MonoBehaviour
{
    public static Vector3 swipeDelta;
    public static Vector3 gyroDelta;

    public static UnityEvent event_SyncSwipeDelta;
    public static UnityEvent event_SyncGyroDelta;
    public static UnityEvent event_OnChairButtonDown;
    public static UnityEvent event_OnChairButtonUp;

    void Start()
    {
        if (event_SyncSwipeDelta == null)
            event_SyncSwipeDelta = new UnityEvent();
        if (event_SyncGyroDelta == null)
            event_SyncGyroDelta = new UnityEvent();
        if (event_OnChairButtonDown == null)
            event_OnChairButtonDown = new UnityEvent();
        if (event_OnChairButtonUp == null)
            event_OnChairButtonUp = new UnityEvent();
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
    void RPC_OnChairButtonDown()
    {
        event_OnChairButtonDown.Invoke();
        Debug.Log("RPC_OnChairButtonDown!");
    }

    [PunRPC]
    void RPC_OnChairButtonUp()
    {
        event_OnChairButtonUp.Invoke();
        Debug.Log("RPC_OnChairButtonUp!");
    }
}
