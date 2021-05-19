﻿using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithHead : MonoBehaviour
{
    public float positionGain;
    public float rotationGain;
    float rotY;
    float rotX;
    float prerotY;
    float prerotX;
    float delRotY;
    float delRotX;
    private bool isPointerDown = false;

    void Start()
    {
        rotY = CameraCache.Main.transform.rotation.y;
        rotX = CameraCache.Main.transform.rotation.x;
        prerotY = rotY;
        prerotX = rotX;
        RPC_PhonetoGlasses.event_OnPointerDown.AddListener(SetPointerDownTrue);
        RPC_PhonetoGlasses.event_OnPointerUp.AddListener(SetPointerDownTrueFalse);
    }


    void Update()
    {
        HeadRotDelYXUpdate();

        if (ExperimentState.trialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.GlassesHead)
        {
            if (isPointerDown)
            {
                MoveWithHead();
            }
        }
        else if (ExperimentState.trialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.GlassesHead)
        {
            if (isPointerDown)
            {
                RotateWithHead();
            }
        }
    }

    private void HeadRotDelYXUpdate()
    {
        rotY = CameraCache.Main.transform.rotation.y;
        rotX = CameraCache.Main.transform.rotation.x;
        delRotY = rotY - prerotY;
        delRotX = rotX - prerotX;
        prerotY = rotY;
        prerotX = rotX;
    }

    public Vector3 RotDelYXtoScrDelXZ()
    {
        return new Vector3(delRotY * Time.deltaTime, 0, -delRotX * Time.deltaTime);
    }

    private void MoveWithHead()
    {
        transform.Translate(RotDelYXtoScrDelXZ() * positionGain, Space.World);
        Debug.Log(CameraCache.Main.transform.forward.x + " " + CameraCache.Main.transform.forward.y + " " + CameraCache.Main.transform.forward.z);
    }

    private void RotateWithHead()
    {
        transform.Rotate(new Vector3(0, delRotY * Time.deltaTime, 0) * rotationGain);
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
