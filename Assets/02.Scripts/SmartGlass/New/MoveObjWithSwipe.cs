using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithSwipe : MonoBehaviour
{
    public float positionGain;
    public float rotationGain;

    void Start()
    {
        RPC_PhonetoGlasses.event_SyncSwipeDelta.AddListener(MoveWithSwipe);
        RPC_PhonetoGlasses.event_SyncSwipeDelta.AddListener(RoatateWithSwipe);
    }

    private void MoveWithSwipe()
    {
        if (ExperimentState.trialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.PhoneSwipe)
        {
            transform.Translate(RPC_PhonetoGlasses.swipeDelta * positionGain);
        }
    }

    private void RoatateWithSwipe()
    {
        if (ExperimentState.trialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.PhoneSwipe)
        {
            transform.Rotate(new Vector3(0, RPC_PhonetoGlasses.swipeDelta.x, 0) * rotationGain);
        }
    }
}
