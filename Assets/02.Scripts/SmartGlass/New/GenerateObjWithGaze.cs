using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjWithGaze : MonoBehaviour
{
    public GameObject prefab;
    private IMixedRealityEyeGazeProvider EyeTrackingProvider => eyeTrackingProvider ?? (eyeTrackingProvider = CoreServices.InputSystem?.EyeGazeProvider);
    private IMixedRealityEyeGazeProvider eyeTrackingProvider = null;

    // Start is called before the first frame update
    void Start()
    {
        RPC_PhonetoGlasses.event_OnPointerUp.AddListener(GenerateWithGaze);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateWithGaze()
    {
        Vector3 hitp = EyeTrackingProvider.HitPosition;
        Instantiate(prefab, hitp, Quaternion.identity);
        Debug.Log("GenerateWithGaze");
    }
}
