using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithSwipe : MonoBehaviour
{
    void Start()
    {
        RPC_PhonetoGlasses.event_SyncSwipeDelta.AddListener(MoveWithSwipe);
    }

    private void MoveWithSwipe()
    {
        transform.Translate(RPC_PhonetoGlasses.swipeDelta);
    }
}
