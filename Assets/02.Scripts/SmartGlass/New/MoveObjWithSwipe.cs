using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithSwipe : MonoBehaviour
{
    public float positionGain;
    public float rotationGain;
    private bool isPointerDown = false;

    void Start()
    {
        RPC_PhonetoGlasses.event_OnPointerDown.AddListener(SetPointerDownTrue);
        RPC_PhonetoGlasses.event_OnPointerUp.AddListener(SetPointerDownTrueFalse);
    }

    private void Update()
    {
        if (ExperimentState.trialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.PhoneSwipe)
        {
            if (isPointerDown)
            {
                MoveWithSwipe();
            }
        } else if(ExperimentState.trialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.PhoneSwipe)
        {
            if (isPointerDown)
            {
                RoatateWithSwipe();
            }
        }
    }

    private void MoveWithSwipe()
    {
            transform.Translate(RPC_PhonetoGlasses.swipeDelta * positionGain, Space.World);
    }

    private void RoatateWithSwipe()
    {
            transform.Rotate(new Vector3(0, RPC_PhonetoGlasses.swipeDelta.x, 0) * rotationGain);
    }

    private void SetPointerDownTrue()
    {
        isPointerDown = true;
    }

    private void SetPointerDownTrueFalse()
    {
        isPointerDown = false;
    }
}
