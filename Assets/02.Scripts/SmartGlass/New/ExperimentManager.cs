using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public float NearLength;
    public float FarLength;
    public float MinAngle;
    public float MaxAngle;
    private float NearOffsetGap;
    private float FarOffsetGap;
    private float AngleOffsetGap;

    ExperimentState experimentState;
    List<ExperimentCase> experimentCase;
    List<List<ExperimentCase>> experimentFlow;
    ExperimentRandomValue experimentRandomValue;

    void Start()
    {
        //Initinalize experiment set, we will order this set to make different experiment sequence just below.
        experimentCase = new List<ExperimentCase>();
        ExperimentCase aCase;
        aCase.distance = Distance.Near;
        aCase.technique = Technique.PhoneSwipe;
        experimentCase.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.PhoneSwipe;
        experimentCase.Add(aCase);
        aCase.distance = Distance.Near;
        aCase.technique = Technique.PhoneGyro;
        experimentCase.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.PhoneGyro;
        experimentCase.Add(aCase);
        aCase.distance = Distance.Near;
        aCase.technique = Technique.GlassesHand;
        experimentCase.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.GlassesHand;
        experimentCase.Add(aCase);

        //Debug Code
        /*
        for (int i = 0; i < 6; i++)
        {
            Debug.Log(experimentCase[i].distance);
            Debug.Log(experimentCase[i].technique);
        }
        */


        //Initinalize list of list, we will make difference experiment order here.
        experimentFlow = new List<List<ExperimentCase>>();
        for(int i = 0; i < 6; i++)
        {
            experimentFlow.Add(new List<ExperimentCase>());
        }
        experimentFlow[0].Add(experimentCase[0]);
        experimentFlow[0].Add(experimentCase[1]);
        experimentFlow[0].Add(experimentCase[2]);
        experimentFlow[0].Add(experimentCase[3]);
        experimentFlow[0].Add(experimentCase[4]);
        experimentFlow[0].Add(experimentCase[5]);

        experimentFlow[1].Add(experimentCase[0]);
        experimentFlow[1].Add(experimentCase[1]);
        experimentFlow[1].Add(experimentCase[4]);
        experimentFlow[1].Add(experimentCase[5]);
        experimentFlow[1].Add(experimentCase[2]);
        experimentFlow[1].Add(experimentCase[3]);

        experimentFlow[2].Add(experimentCase[2]);
        experimentFlow[2].Add(experimentCase[3]);
        experimentFlow[2].Add(experimentCase[0]);
        experimentFlow[2].Add(experimentCase[1]);
        experimentFlow[2].Add(experimentCase[4]);
        experimentFlow[2].Add(experimentCase[5]);

        experimentFlow[3].Add(experimentCase[2]);
        experimentFlow[3].Add(experimentCase[3]);
        experimentFlow[3].Add(experimentCase[4]);
        experimentFlow[3].Add(experimentCase[5]);
        experimentFlow[3].Add(experimentCase[0]);
        experimentFlow[3].Add(experimentCase[1]);

        experimentFlow[4].Add(experimentCase[4]);
        experimentFlow[4].Add(experimentCase[5]);
        experimentFlow[4].Add(experimentCase[0]);
        experimentFlow[4].Add(experimentCase[1]);
        experimentFlow[4].Add(experimentCase[2]);
        experimentFlow[4].Add(experimentCase[3]);

        experimentFlow[5].Add(experimentCase[4]);
        experimentFlow[5].Add(experimentCase[5]);
        experimentFlow[5].Add(experimentCase[2]);
        experimentFlow[5].Add(experimentCase[3]);
        experimentFlow[5].Add(experimentCase[0]);
        experimentFlow[5].Add(experimentCase[1]);

        //Debug Code
        /*
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Debug.Log(experimentFlow[i][j].distance + "  " + experimentFlow[i][j].technique);
            }
        }
        */
        

        // Initialize data structure for generating random values.
        experimentRandomValue.NearPositionOffsetValues = new List<float>();
        experimentRandomValue.FarPositionOffsetValues = new List<float>();
        experimentRandomValue.RotationOffsetValues = new List<float>();

        NearOffsetGap = NearLength / 9;
        FarOffsetGap = FarLength / 9;
        AngleOffsetGap = (MaxAngle-MinAngle) / 9;

        float NearMinOffSet = -NearLength / 2;
        float FarMinOffSet = -FarLength / 2;
        for(int i=0; i<10; i++)
        {
            experimentRandomValue.NearPositionOffsetValues.Add(NearMinOffSet + NearOffsetGap * i);
            experimentRandomValue.FarPositionOffsetValues.Add(FarMinOffSet + FarOffsetGap * i);
            experimentRandomValue.RotationOffsetValues.Add(90f + AngleOffsetGap * i);
        }
        //Debug Code
        /*
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(experimentRandomValue.NearPositionOffsetValues[i]);
        }
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(experimentRandomValue.FarPositionOffsetValues[i]);
        }
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(experimentRandomValue.RotationOffsetValues[i]);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
