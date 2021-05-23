﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithGyro : MonoBehaviour
{
    public float positionGain;
    public float rotationGain;
    private bool isPointerDown = false;

    void Start()
    {
        RPC_PhonetoGlasses.event_pointerDown.AddListener(SetPointerDownTrue);
        RPC_PhonetoGlasses.event_pointerUp.AddListener(SetPointerDownTrueFalse);
    }

    private void Update()
    {
        if (ExperimentState.curTrialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.PhoneGyro)
        {
            if (isPointerDown)
            {
                MoveWithGyro();
            }
        }
        else if (ExperimentState.curTrialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.PhoneGyro)
        {
            if (isPointerDown)
            {
                RotateWithGyro();
            }
        }
    }

    private void MoveWithGyro()
    {
        transform.Translate(RPC_PhonetoGlasses.gyroDelta * positionGain);      
    }

    private void RotateWithGyro()
    {
        transform.Rotate(new Vector3(0, RPC_PhonetoGlasses.gyroDelta.x, 0) * rotationGain);
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
