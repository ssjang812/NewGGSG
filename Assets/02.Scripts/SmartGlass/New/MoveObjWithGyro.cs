using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithGyro : MonoBehaviour
{
    void Start()
    {
        RPC_PhonetoGlasses.event_SyncGyroDelta.AddListener(MoveWithGyro);
    }

    private void MoveWithGyro()
    {
        transform.Translate(RPC_PhonetoGlasses.gyroDelta);
    }
}
