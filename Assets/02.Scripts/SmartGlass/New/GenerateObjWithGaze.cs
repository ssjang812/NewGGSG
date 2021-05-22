using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 본실험에서는 한번에 하나의 의자만 생성하고 조작하므로, generate하는게 아닌 하나의 의자를 시선위치로 이동시키는 식으로 활용
public class GenerateObjWithGaze : MonoBehaviour
{
    //public GameObject prefab; // 생성하는 식으로할게 아니면 안쓰임
    private IMixedRealityEyeGazeProvider EyeTrackingProvider => eyeTrackingProvider ?? (eyeTrackingProvider = CoreServices.InputSystem?.EyeGazeProvider);
    private IMixedRealityEyeGazeProvider eyeTrackingProvider = null;

    // Start is called before the first frame update
    void Start()
    {
        RPC_PhonetoGlasses.event_chairButtonUp.AddListener(PlaceWithGaze);
    }

    /*
    public void GenerateWithGaze()
    {
        Vector3 hitp = EyeTrackingProvider.HitPosition;
        Instantiate(prefab, hitp, Quaternion.identity);
        Debug.Log("GenerateWithGaze");
    }
    */

    public void PlaceWithGaze()
    {
        if (ExperimentState.curTrialPhase == TrialPhase.RoughPlacement)
        {
            if (EyeTrackingProvider.GazeTarget != null)
            {
                if (EyeTrackingProvider.GazeTarget.tag == "PlaceableWithGaze")
                {
                    Vector3 hitp = EyeTrackingProvider.HitPosition;
                    transform.position = hitp;
                    transform.rotation = Quaternion.identity;
                }
            }
        }
    }
}
