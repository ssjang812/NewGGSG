using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjWithHead : MonoBehaviour
{
    float rotY;
    float rotX;
    float prerotY;
    float prerotX;
    float delRotY;
    float delRotX;
    public float positionGain;
    public float rotationGain;

    void Start()
    {
        rotY = CameraCache.Main.transform.rotation.y;
        rotX = CameraCache.Main.transform.rotation.x;
        prerotY = rotY;
        prerotX = rotX;
    }


    void Update()
    {
        HeadRotDelYXUpdate();
        MoveWithHead();
        RotateWithHead();
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
        if (ExperimentState.trialPhase == TrialPhase.FinePlacement && ExperimentState.curBlockTechnique == Technique.GlassesHead)
        {
            transform.Translate(RotDelYXtoScrDelXZ() * positionGain);
            //Debug.Log(CameraCache.Main.transform.forward.x + " " + CameraCache.Main.transform.forward.y + " " + CameraCache.Main.transform.forward.z);
        }
    }

    private void RotateWithHead()
    {
        if (ExperimentState.trialPhase == TrialPhase.Rotation && ExperimentState.curBlockTechnique == Technique.GlassesHead)
        {
            transform.Rotate(new Vector3(0, delRotY * Time.deltaTime, 0) * rotationGain);
        }
    }
}
