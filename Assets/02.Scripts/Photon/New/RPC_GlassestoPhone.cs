using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC_GlassestoPhone : MonoBehaviour
{
    public static UnityEvent event_OnOneTrialStart;
    void Start()
    {
        if (event_OnOneTrialStart == null)
            event_OnOneTrialStart = new UnityEvent();
    }

    [PunRPC]
    void RPC_OnOneTrialStart()
    {
        event_OnOneTrialStart.Invoke();
        Debug.Log("RPC_OnOneTrialStart");
    }
}
