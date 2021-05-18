using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithGyro : MonoBehaviour
{
    public float positionGain;
    public float rotationGain;

    void Start()
    {
        RPC_PhonetoGlasses.event_SyncGyroDelta.AddListener(MoveWithGyro);
        RPC_PhonetoGlasses.event_SyncGyroDelta.AddListener(RotateWithGyro);
    }

    private void MoveWithGyro()
    {
        if(ExperimentState.trialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.PhoneGyro)
        {
            transform.Translate(RPC_PhonetoGlasses.gyroDelta * positionGain);
        }        
    }

    private void RotateWithGyro()
    {
        if (ExperimentState.trialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.PhoneGyro)
        {
            transform.Rotate(new Vector3(0, RPC_PhonetoGlasses.gyroDelta.x, 0) * rotationGain);
        }
    }
}
