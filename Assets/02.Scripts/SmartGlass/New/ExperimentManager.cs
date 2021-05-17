using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    //실험 수치값 유니티를통해 받기
    public float NearLength;
    public float FarLength;
    public float MinAngle;
    public float MaxAngle;

    //수치값 최소 최대값을 10등분 하기위해 사이값 저장
    private float NearOffsetGap;
    private float FarOffsetGap;
    private float AngleOffsetGap;

    //실험동안 필요한 자료구조
    static ExperimentState expState;    //현재 실험 진행상황
    List<ExperimentCase> expDefaultFlow;    //실험의 가장 기본 흐름
    List<List<ExperimentCase>> expAllFlow;  //실험 흐름을 6가지로 섞음 (counter balanced with Latin Square)
    ExperimentRandomValue expRandomValue;   //Near, Far에서의 거리, Angle을 10등분한 값들을 가지고있는 구조체

    //한번의 블럭의 반복에 필요한 자료구조
    List<ExperimentCase> curExpFlow;    //전체 흐름에서 선택한 하나의 흐름을 가지고있음
    ExperimentCase curExpCase;  //curExpFlow에서 하나의 블록
    List<float> nearRandomValue = new List<float>();  //랜덤한 index를 pop하는 식으로 랜덤값을 쓸거기때문에 한블럭을 시작할때마다 새로 복사해서 시작해야함, 원본을 지워버리면 이후에 못하기때문
    List<float> farRandomValue = new List<float>();
    List<float> rotationRandomValue = new List<float>();


    void Start()
    {
        //Initinalize experiment set, we will order this set to make different experiment sequence just below.
        expDefaultFlow = new List<ExperimentCase>();
        ExperimentCase aCase;
        aCase.distance = Distance.Near;
        aCase.technique = Technique.PhoneSwipe;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.PhoneSwipe;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Near;
        aCase.technique = Technique.PhoneGyro;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.PhoneGyro;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Near;
        aCase.technique = Technique.GlassesHand;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.GlassesHand;
        expDefaultFlow.Add(aCase);

        //Debug Code
        /*
        for (int i = 0; i < 6; i++)
        {
            Debug.Log(experimentCase[i].distance);
            Debug.Log(experimentCase[i].technique);
        }
        */


        //Initinalize list of list, we will make difference experiment order here.
        expAllFlow = new List<List<ExperimentCase>>();
        for(int i = 0; i < 6; i++)
        {
            expAllFlow.Add(new List<ExperimentCase>());
        }
        expAllFlow[0].Add(expDefaultFlow[0]);
        expAllFlow[0].Add(expDefaultFlow[1]);
        expAllFlow[0].Add(expDefaultFlow[2]);
        expAllFlow[0].Add(expDefaultFlow[3]);
        expAllFlow[0].Add(expDefaultFlow[4]);
        expAllFlow[0].Add(expDefaultFlow[5]);

        expAllFlow[1].Add(expDefaultFlow[0]);
        expAllFlow[1].Add(expDefaultFlow[1]);
        expAllFlow[1].Add(expDefaultFlow[4]);
        expAllFlow[1].Add(expDefaultFlow[5]);
        expAllFlow[1].Add(expDefaultFlow[2]);
        expAllFlow[1].Add(expDefaultFlow[3]);

        expAllFlow[2].Add(expDefaultFlow[2]);
        expAllFlow[2].Add(expDefaultFlow[3]);
        expAllFlow[2].Add(expDefaultFlow[0]);
        expAllFlow[2].Add(expDefaultFlow[1]);
        expAllFlow[2].Add(expDefaultFlow[4]);
        expAllFlow[2].Add(expDefaultFlow[5]);

        expAllFlow[3].Add(expDefaultFlow[2]);
        expAllFlow[3].Add(expDefaultFlow[3]);
        expAllFlow[3].Add(expDefaultFlow[4]);
        expAllFlow[3].Add(expDefaultFlow[5]);
        expAllFlow[3].Add(expDefaultFlow[0]);
        expAllFlow[3].Add(expDefaultFlow[1]);

        expAllFlow[4].Add(expDefaultFlow[4]);
        expAllFlow[4].Add(expDefaultFlow[5]);
        expAllFlow[4].Add(expDefaultFlow[0]);
        expAllFlow[4].Add(expDefaultFlow[1]);
        expAllFlow[4].Add(expDefaultFlow[2]);
        expAllFlow[4].Add(expDefaultFlow[3]);

        expAllFlow[5].Add(expDefaultFlow[4]);
        expAllFlow[5].Add(expDefaultFlow[5]);
        expAllFlow[5].Add(expDefaultFlow[2]);
        expAllFlow[5].Add(expDefaultFlow[3]);
        expAllFlow[5].Add(expDefaultFlow[0]);
        expAllFlow[5].Add(expDefaultFlow[1]);

        //Debug Code
        /*
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Debug.Log(expAllFlow[i][j].distance + "  " + expAllFlow[i][j].technique);
            }
        }
        */
        

        // Initialize data structure for generating random values.
        expRandomValue.NearPositionOffsetValues = new List<float>();
        expRandomValue.FarPositionOffsetValues = new List<float>();
        expRandomValue.RotationOffsetValues = new List<float>();

        NearOffsetGap = NearLength / 9;
        FarOffsetGap = FarLength / 9;
        AngleOffsetGap = (MaxAngle-MinAngle) / 9;

        float NearMinOffSet = -NearLength / 2;
        float FarMinOffSet = -FarLength / 2;
        for(int i=0; i<10; i++)
        {
            expRandomValue.NearPositionOffsetValues.Add(NearMinOffSet + NearOffsetGap * i);
            expRandomValue.FarPositionOffsetValues.Add(FarMinOffSet + FarOffsetGap * i);
            expRandomValue.RotationOffsetValues.Add(90f + AngleOffsetGap * i);
        }
        //Debug Code
        /*
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(expRandomValue.NearPositionOffsetValues[i]);
        }
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(expRandomValue.FarPositionOffsetValues[i]);
        }
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(expRandomValue.RotationOffsetValues[i]);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartExperiment()
    {
        expState.participantNum = ExpCtrPanel.expNumber;
        curExpFlow = expAllFlow[expState.participantNum - 1];
        expState.curBlockNum = 1;
        SetOneBlock();
    }

    public void SetOneBlock()
    {
        for (int i = 0; i < 6; i++)
        {
            //랜덤한 index를 pop하는 식으로 랜덤값을 쓸거기때문에 한블럭을 시작할때마다 새로 복사해서 시작해야함, 원본을 지워버리면 이후에 못하기때문
            nearRandomValue.Add(expRandomValue.NearPositionOffsetValues[i]);
            farRandomValue.Add(expRandomValue.FarPositionOffsetValues[i]);
            rotationRandomValue.Add(expRandomValue.RotationOffsetValues[i]);
        }

        //한블럭 시작 초기화
        curExpCase = curExpFlow[expState.curBlockNum - 1];
        expState.curBlockDistance = curExpCase.distance;
        expState.curBlockTechnique = curExpCase.technique;

        SetOneTrial();
    }

    //한블럭 10회 수행
    public void SetOneTrial()
    {
        if (nearRandomValue.Count == 0)
        {
            expState.curBlockNum++;
            if (expState.curBlockNum > 6)
            {
                //실험 종료
            }
        }

        System.Random random = new System.Random();
        int index;

        if (expState.curBlockDistance == Distance.Near)
        {
            index = random.Next(nearRandomValue.Count);
            expState.curPositionOffset = nearRandomValue[index];
            nearRandomValue.RemoveAt(index);
        }
        else
        {
            index = random.Next(farRandomValue.Count);
            expState.curPositionOffset = farRandomValue[index];
            farRandomValue.RemoveAt(index);
        }
        index = random.Next(rotationRandomValue.Count);
        expState.curRotationOffset = rotationRandomValue[index];
        rotationRandomValue.RemoveAt(index);




        // 현재 된거 : 전체 흐름 컨트롤과 그에따른 변수들 초기화
        // 해야할거
        // SetOne Trial때마다 흐릿하게 보여주기 구현 : 흐릿하게 보여주는거에는 인터랙션 달지말고, 생성한(그냥 투명하게해뒀다가 위치만 옮겨줘도될듯) 거에만 인터랙션모듈달아둬서 인풋에 영향받도록
        // 정답체크하는 파트
        // 인터랙션 모듈들 통합 + 모드에 따라 한가지 모드만 작동하도록
        // 이벤트에 기능 연결시키기 + csv 기록
    }

    public void CheckRoughPlament()
    {
        // 인터랙션 모드 변경
    }

    public void CheckFinePlacement()
    {
        // 각도조작으로 변경
    }

    public void CheckRotation()
    {
        // 다음 trial 실행
        SetOneTrial();
    }
}
