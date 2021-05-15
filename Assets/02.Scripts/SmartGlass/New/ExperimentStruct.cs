using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Distance
{
    Near,
    Far
}

enum Technique
{
    PhoneSwipe,
    PhoneGyro,
    GlassesHand
}

struct ExperimentCase
{
    public Distance distance;
    public Technique technique;
}

struct ExperimentState
{
    public int participantNum;
    public int curBlockNum;
    public Distance curBlockDistance;
    public Technique curBlockTechnique;
    public float curPositionOffset;
    public float curRotationOffset;
}

struct ExperimentRandomValue
{
    public List<float> NearPositionOffsetValues;
    public List<float> FarPositionOffsetValues;
    public List<float> RotationOffsetValues;
}