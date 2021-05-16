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
    public float gain;

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
        transform.Translate(RotDelYXtoScrDelXZ());
        //Debug.Log(CameraCache.Main.transform.forward.x + " " + CameraCache.Main.transform.forward.y + " " + CameraCache.Main.transform.forward.z);
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
        return new Vector3(delRotY * Time.deltaTime * gain, 0, -delRotX * Time.deltaTime * gain);
    }
}
