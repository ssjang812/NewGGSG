using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    // 중요 : 한실험은 6개의 블록으로 이루어져있다, 한블록은 10회의 수행으로 이루어져있다, 1회 수행은 3단계 페이즈로 이루어져있다(시선배치-디테일배치-각도배치).

    public GameObject manipChair;
    public GameObject guideChair;
    public GameObject turnOffDuringExperiment;
    public GameObject instructionCanvas;
    public TextMeshProUGUI instruction;
    public GameObject nextBlockBtn;
    public PhotonView PV;
    private Vector3 defaultManipChairPosition;

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
        instructionCanvas.SetActive(false);
        defaultManipChairPosition = manipChair.transform.localPosition;
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
        aCase.technique = Technique.GlassesHead;
        expDefaultFlow.Add(aCase);
        aCase.distance = Distance.Far;
        aCase.technique = Technique.GlassesHead;
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
        turnOffDuringExperiment.SetActive(false);
        ExperimentState.participantNum = ExpCtrPanel.expNumber;
        curExpFlow = expAllFlow[ExperimentState.participantNum - 1];
        ExperimentState.curBlockNum = 1;
        SetOneBlock();
    }

    public void SetOneBlock()
    {
        instructionCanvas.SetActive(false);
        for (int i = 0; i < expRandomValue.NearPositionOffsetValues.Count; i++)
        {
            //랜덤한 index를 pop하는 식으로 랜덤값을 쓸거기때문에 한블럭을 시작할때마다 새로 복사해서 시작해야함, 원본을 지워버리면 이후에 못하기때문
            nearRandomValue.Add(expRandomValue.NearPositionOffsetValues[i]);
            farRandomValue.Add(expRandomValue.FarPositionOffsetValues[i]);
            rotationRandomValue.Add(expRandomValue.RotationOffsetValues[i]);
        }

        //한블럭 시작 초기화
        curExpCase = curExpFlow[ExperimentState.curBlockNum - 1];
        ExperimentState.curBlockDistance = curExpCase.distance;
        ExperimentState.curBlockTechnique = curExpCase.technique;

        SetOneTrial();
    }

    //한블럭 10회 수행
    public void SetOneTrial()
    {
        PV.RPC("RPC_OnOneTrialStart", RpcTarget.All); // trial 시작시마다 가림막 설치, 랜덤한 위치에 버튼생성을 위해 시작할때 신호를 보내줌

        // 한 trial의 첫 모드는 항상 시선으로 배치로 시작
        ExperimentState.trialPhase = TrialPhase.RoughPlacement;

        // Trial 10번하면 새로운 의자 생성을 멈춤, 설문시간을 가진후 다시 시작을 누르면 다음 Block을 실행
        if (nearRandomValue.Count == 0)
        {
            ExperimentState.curBlockDistance = Distance.Null;
            ExperimentState.curBlockTechnique = Technique.Null;
            ExperimentState.trialPhase = TrialPhase.Null;
            ExperimentState.curPositionOffset = 0;
            ExperimentState.curRotationOffset = 0;
            manipChair.transform.localPosition = defaultManipChairPosition;
            // 실험 종료
            if (ExperimentState.curBlockNum == 6)
            {
                instruction.SetText($"<size=35><b>All trials are over</b></size>\n\nPlease call coordinator and fill out the questionnaire.");
                nextBlockBtn.SetActive(false);
            } else if(ExperimentState.curBlockNum < 6)
            {
                instruction.SetText($"<size=35><b>Block{ExperimentState.curBlockNum - 1} is over</b></size>\n\nPlease call coordinator and fill out the questionnaire.");
                ExperimentState.curBlockNum++;
            }
            instructionCanvas.SetActive(true);
            return;
        }

        // 랜덤 값 리스트에서 이번 Trial에 사용될 랜덤값 선정
        System.Random random = new System.Random();
        int index;

        if (ExperimentState.curBlockDistance == Distance.Near)
        {
            index = random.Next(nearRandomValue.Count);
            ExperimentState.curPositionOffset = nearRandomValue[index];
            nearRandomValue.RemoveAt(index);
        }
        else
        {
            index = random.Next(farRandomValue.Count);
            ExperimentState.curPositionOffset = farRandomValue[index];
            farRandomValue.RemoveAt(index);
        }
        index = random.Next(rotationRandomValue.Count);
        ExperimentState.curRotationOffset = rotationRandomValue[index];
        rotationRandomValue.RemoveAt(index);

        // 배치와 돌리는 과정을 나눠서하기때문에 Que도 나눠서 두단계에 걸쳐서 줘야함, 여기서는 포지션 큐만, 로테이션은 상세배치 끝난후에
        guideChair.transform.localPosition = Vector3.zero;
        guideChair.transform.Translate(new Vector3(ExperimentState.curPositionOffset, 0, 0));
        // 기록기능
        // 추가로 중간부터 시작할수있는 기능 있어야할듯 : 원하는 블록번호 넣는 칸 + 중간부터 시작버튼만 있으면 쉽게 만들듯하다
    }


    // 한번의 trial에는 3가지의 페이즈가 있음, 사용자 입력때마다 상태체크 or 정답체크하는 파트, 특정상태 or 정답이면 상호작용모드만 바꿔주면 된다.
    public void PhaseCheck()
    {
        if(ExperimentState.trialPhase == TrialPhase.RoughPlacement)
        {
            // 그냥 시선배치니까 어느정도 근처에 배치되면 ok
            if(Vector3.Distance(guideChair.transform.position, manipChair.transform.position) < 1f)
            {
                ExperimentState.trialPhase = TrialPhase.FinePlacement;
            }
        }
        else if(ExperimentState.trialPhase == TrialPhase.FinePlacement)
        {
            // 정답체크해서 맞으면 각도모드로 바꿔주고 아니면 말고
            if (Vector3.Distance(guideChair.transform.position, manipChair.transform.position) < 0.15f)
            {
                guideChair.transform.rotation = Quaternion.identity;
                guideChair.transform.Rotate(new Vector3(0, ExperimentState.curRotationOffset, 0));
                ExperimentState.trialPhase = TrialPhase.Rotation;
            }
        }
        else
        {
            // 정답체크해서 맞으면 다음 trial 호출 (세번째 페이즈까지 완료하면 다음 trial 호출)
            if (Vector3.Angle(guideChair.transform.forward, manipChair.transform.forward) < 5f)
            {
                SetOneTrial();
            }
        }
    }
}
